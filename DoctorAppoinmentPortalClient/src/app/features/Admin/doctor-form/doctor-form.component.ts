import { Component } from '@angular/core';
import { DoctorFormService } from './doctor-form.service';
import { DoctorInfo } from '../../../core/models/doctor-info.model';
import { UserRoles } from '../../../core/enums/user-roles.enum';
import { ToastService } from '../../../core/services/toast.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { AlertService } from '../../../core/services/alert.service';

@Component({
  selector: 'app-doctor-form',
  templateUrl: './doctor-form.component.html',
  styleUrl: './doctor-form.component.css',
})
export class DoctorFormComponent {
  countries = [];
  states = [];
  districts = [];
  talukas = [];
  specializations = [];
  qualifications = [];
  bloodGroups = [];
  genders = [];
  daysOfWeek = [
    'Monday',
    'Tuesday',
    'Wednesday',
    'Thursday',
    'Friday',
    'Saturday',
    'Sunday',
  ];

  endTimeError: boolean = false;
  showPassword = false;

  isFormSubmitting: boolean = false;

  todayDate: Date = new Date();

  formMode: string = '';

  doctorDetails: DoctorInfo = new DoctorInfo();

  slotOverlaps: boolean[] = [];

  constructor(
    private doctorFormService: DoctorFormService,
    private toastService: ToastService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDoctorDropDowns();

    this.formMode = this.route.snapshot.data['mode'] || 'add';

    const doctorId = this.route.snapshot.paramMap.get('doctorId');
    if (doctorId) {
      this.loadDoctorInfo(Number(doctorId));
    } else {
      this.doctorDetails = new DoctorInfo();
    }
  }

  loadDoctorInfo(doctorId: number) {
    this.doctorFormService.loadDoctor(doctorId).subscribe({
      next: (res: any) => {
        if (res.data) {
          this.doctorDetails = res.data;

          this.isFormSubmitting = false;
        } else {
          this.doctorDetails = new DoctorInfo();
          this.toastService.infoToastr('No doctor data found.', 'Info');
          this.isFormSubmitting = false;
        }
      },
      error: (err) => {
        console.error('Error loading doctor info:', err);
        this.doctorDetails = new DoctorInfo();
        this.isFormSubmitting = false;
        this.toastService.errorToastr(
          'Failed to load doctor information. Please try again later.',
          'Doctor Info Error'
        );
      },
    });
  }

  loadDoctorDropDowns() {
    this.doctorFormService.getDoctorDropDowns().subscribe({
      next: (response: any) => {
        this.countries = response?.data.CountriesList ?? [];
        this.states = response?.data?.StatesList ?? [];
        this.districts = response?.data?.DistrictsList ?? [];
        this.talukas = response?.data?.TalukasList ?? [];
        this.bloodGroups = response?.data?.BloodGroupsList ?? [];
        this.genders = response?.data?.GendersList ?? [];
        this.qualifications = response?.data?.QualificationsList ?? [];
        this.specializations = response?.data?.SpecializationsList ?? [];

        if (
          this.countries.length === 0 &&
          this.states.length === 0 &&
          this.districts.length === 0
        ) {
          this.toastService.infoToastr(
            'Some dropdown data is missing.',
            'DropDown Info'
          );
        }
      },
      error: (err) => {
        console.error('Error loading doctor drop downs:', err);

        this.countries = [];
        this.states = [];
        this.districts = [];
        this.talukas = [];
        this.bloodGroups = [];
        this.genders = [];
        this.qualifications = [];
        this.specializations = [];

        this.toastService.errorToastr(
          'Failed to load doctor dropdowns. Please try again later.',
          'DropDowns Error'
        );
      },
    });
  }

  addSlot() {
    if (this.hasTooManySlots()) {
      alert('You cannot add more slots. No day should have more than 3 slots.');
      return;
    }
    this.doctorDetails.DoctorAvailableSlots.push({
      AvailableSlotId: 0,
      DayOfWeek: null,
      StartTime: '',
      EndTime: '',
      IsAvailable: true,
    });
    this.slotOverlaps.push(false);
  }

  removeSlot(index: any) {
    this.doctorDetails.DoctorAvailableSlots.splice(index, 1);
    this.slotOverlaps.splice(index, 1);
  }

  getSlotCountForDay(day: string | null): number {
    return this.doctorDetails.DoctorAvailableSlots.filter(
      (slot) => slot.DayOfWeek === day
    ).length;
  }

  hasTooManySlots(): boolean {
    return this.daysOfWeek.some((day) => this.getSlotCountForDay(day) >= 4);
  }

  validateOverlap(index: number) {
    const currentSlot = this.doctorDetails.DoctorAvailableSlots[index];

    if (
      !currentSlot.DayOfWeek ||
      !currentSlot.StartTime ||
      !currentSlot.EndTime
    ) {
      this.slotOverlaps[index] = false;
      return;
    }

    const currentStart = this.convertTimeToMinutes(currentSlot.StartTime);
    let currentEnd = this.convertTimeToMinutes(currentSlot.EndTime);

    if (currentEnd <= currentStart) {
      currentEnd += 24 * 60;
    }

    const overlaps = this.doctorDetails.DoctorAvailableSlots.some((slot, i) => {
      if (i === index) return false;
      if (slot.DayOfWeek !== currentSlot.DayOfWeek) return false;
      if (!slot.StartTime || !slot.EndTime) return false;

      const slotStart = this.convertTimeToMinutes(slot.StartTime);
      let slotEnd = this.convertTimeToMinutes(slot.EndTime);

      if (slotEnd <= slotStart) {
        slotEnd += 24 * 60;
      }

      return currentStart < slotEnd && currentEnd > slotStart;
    });

    this.slotOverlaps[index] = overlaps;
  }

  hasOverlap(index: number): boolean {
    return this.slotOverlaps[index] === true;
  }

  convertTimeToMinutes(time: string): number {
    if (!time) return 0;
    const [hours, minutes] = time.split(':').map(Number);
    return hours * 60 + minutes;
  }

  isTimeSlotValid(slot: any): boolean {
    if (!slot?.StartTime || !slot?.EndTime) return false;

    const startMinutes = this.convertTimeToMinutes(slot.StartTime);
    const endMinutes = this.convertTimeToMinutes(slot.EndTime);

    const adjustedEndMinutes =
      endMinutes < startMinutes ? endMinutes + 24 * 60 : endMinutes;

    return adjustedEndMinutes - startMinutes >= 60;
  }

  async onSubmit(form: any) {
    if (form.invalid) {
      console.log('error toast appear');

      this.toastService.errorToastr(
        'Fill all required information',
        'Validation Error'
      );
      return;
    }

    const userId = this.authService.getUserId();
    if (!userId) {
      this.toastService.errorToastr(
        'User session expired. Please log in again.',
        'Authorization Error'
      );
      this.authService.logout();
      return;
    }

    if (!this.isAgeValid()) {
      this.toastService.errorToastr(
        'Doctor must be at least 25 years old.',
        'Invalid Age'
      );
      return;
    }

    const hasOverlapSlots = this.slotOverlaps.some(
      (overlap) => overlap === true
    );
    if (hasOverlapSlots) {
      this.toastService.errorToastr(
        'One or more time slots overlap. Please fix them before submitting.',
        'Invalid Time Slots'
      );
      return;
    }

    const invalidDuration = this.doctorDetails.DoctorAvailableSlots.some(
      (slot) => slot.StartTime && slot.EndTime && !this.isTimeSlotValid(slot)
    );
    if (invalidDuration) {
      this.toastService.errorToastr(
        'Each slot must be at least 1 hour long.',
        'Invalid Slot Duration'
      );
      return;
    }

    if (this.hasTooManySlots()) {
      this.toastService.errorToastr(
        'You cannot have more than 3 slots per day.',
        'Slot Limit Exceeded'
      );
      return;
    }

    const hasIncompleteSlots = this.doctorDetails.DoctorAvailableSlots.some(
      (slot) => !slot.DayOfWeek || !slot.StartTime || !slot.EndTime
    );
    if (hasIncompleteSlots) {
      this.toastService.errorToastr(
        'Please fill all slot details (Day, Start Time, End Time).',
        'Incomplete Slots'
      );
      return;
    }

    const actionText =
      this.formMode === 'add' ? 'add this doctor?' : 'update doctor details?';

    const confirmed = await this.alertService.confirm(
      'Please Confirm',
      `Are you sure you want to ${actionText}`,
      'Yes, Proceed',
      'Cancel'
    );

    if (!confirmed) {
      this.toastService.infoToastr('Action cancelled by user.', 'Cancelled');
      return;
    }

    this.doctorDetails.RoleShortCode = UserRoles.Doctor;
    this.submittingDoctorForm();
  }

  submittingDoctorForm() {
    if (this.isFormSubmitting) return;

    this.isFormSubmitting = true;

    this.doctorFormService.insertOrUpdateDoctor(this.doctorDetails).subscribe({
      next: (response) => {
        if (response.success) {
          this.toastService.successToastr(response.message, 'Success');
          this.isFormSubmitting = false;
          this.router.navigate(['/admin/doctorsList']);
        } else {
          this.isFormSubmitting = false;
          this.toastService.errorToastr(response.message, 'Failed');
        }
      },
      error: (error) => {
        this.isFormSubmitting = false;
        const msg = error?.error?.message || 'Unexpected error occurred.';
        this.toastService.errorToastr(msg, 'Error');
      },
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  resetDoctorInfo(doctorId: number) {
    const confirmReset = window.confirm(
      'Are you sure you want to reset the doctor information to the last saved data?'
    );

    if (!confirmReset) {
      this.toastService.infoToastr('Doctor info reset cancelled.', 'Cancelled');
      return;
    }

    this.loadDoctorInfo(doctorId);
    this.toastService.successToastr(
      'Doctor information reloaded successfully.',
      'Data Reset'
    );
  }

  get minDateForAge100(): Date {
    const today = new Date();
    const maxDate = new Date(
      today.getFullYear() - 100,
      today.getMonth(),
      today.getDate()
    );
    return maxDate;
  }

  get minDateForAge25(): Date {
    const today = new Date();
    const minDate = new Date(
      today.getFullYear() - 25,
      today.getMonth(),
      today.getDate()
    );
    return minDate;
  }

  calculateAge(dateOfBirth: string): number {
    if (!dateOfBirth) return 0;

    const birthDate = new Date(dateOfBirth);
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (
      monthDiff < 0 ||
      (monthDiff === 0 && today.getDate() < birthDate.getDate())
    ) {
      age--;
    }

    return age;
  }

  isAgeValid(): boolean {
    const age = this.calculateAge(this.doctorDetails.DateOfBirth.toString());
    return age >= 25;
  }

  getTimeDifferenceInHours(startTime: string, endTime: string): number {
    if (!startTime || !endTime) return 0;

    const [startHours, startMinutes] = startTime.split(':').map(Number);
    const [endHours, endMinutes] = endTime.split(':').map(Number);

    const startTotalMinutes = startHours * 60 + startMinutes;
    const endTotalMinutes = endHours * 60 + endMinutes;

    const diffMinutes = endTotalMinutes - startTotalMinutes;
    return diffMinutes / 60;
  }

  isEndTimeAfterStart(slot: any): boolean {
    if (!slot.StartTime || !slot.EndTime) return true;

    const diffHours = this.getTimeDifferenceInHours(
      slot.StartTime,
      slot.EndTime
    );
    return diffHours > 0;
  }

  clearDoctorForm(form: any) {
    const confirmClear = window.confirm(
      'Are you sure you want to clear all entered details?'
    );

    if (!confirmClear) {
      this.toastService.infoToastr('Form clear action cancelled.', 'Cancelled');
      return;
    }

    this.doctorDetails = new DoctorInfo();
    form.reset();
    this.loadDoctorDropDowns();

    this.toastService.successToastr('Form cleared successfully.', 'Form Reset');
  }
}

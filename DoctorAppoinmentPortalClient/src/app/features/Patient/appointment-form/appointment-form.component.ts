import { Component } from '@angular/core';
import { AppointmentFormService } from './appointment-form.service';
import { AvailableSlots } from '../../../core/models/availableSlots.model';
import { PreferredSlots } from '../../../core/models/preferred-slots.model';
import { PatientAppointmentRequest } from '../../../core/models/patient-appointment-request.model';
import { ToastService } from '../../../core/services/toast.service';
import { Router } from '@angular/router';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { finalize } from 'rxjs';
import { AlertService } from '../../../core/services/alert.service';

@Component({
  selector: 'app-appointment-form',
  templateUrl: './appointment-form.component.html',
  styleUrl: './appointment-form.component.css',
})
export class AppointmentFormComponent {
  preferredDate: string;
  selectedSpecialization: number;

  availableSlots: AvailableSlots[] = [];
  selectedFiles: File[] = [];

  patientAppointment: PatientAppointmentRequest =
    new PatientAppointmentRequest();

  doctors = [];
  countries = [];
  states = [];
  districts = [];
  talukas = [];
  bloodGroups = [];
  genders = [];
  specializations = [];

  isSelectedSlot: boolean = false;
  isHasPreviousReport: boolean = false;
  isHasInsurances: boolean = false;
  isSpecializationSelected: boolean = false;
  isSubmitting: boolean = false;

  fileErrorMessage: string = '';
  maxFileSizeMB: number = 5;
  allowedFileTypes: string[] = ['application/pdf', 'image/png', 'image/jpeg'];

  yesNoOptions = [
    { label: 'Yes', value: true },
    { label: 'No', value: false },
  ];

  today: Date = new Date();
  maxDate: Date = new Date();

 

  constructor(
    private appointmentFormService: AppointmentFormService,
    private toastService: ToastService,
    private alertService:AlertService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadFormDropDowns();
    this.maxDate.setDate(new Date().getDate() + 14);
  }

  loadFormDropDowns() {
    this.appointmentFormService.getAppointmentFormDropDrowns().subscribe({
      next: (res: any) => {
        if (res.Data) {
          this.genders = res.Data.GendersList || [];
          this.bloodGroups = res.Data.BloodGroupsList || [];
          this.specializations = res.Data.SpecializationsList || [];
          this.talukas = res.Data.TalukasList || [];
          this.districts = res.Data.DistrictsList || [];
          this.states = res.Data.StatesList || [];
          this.countries = res.Data.CountriesList || [];
        } else {
          this.toastService.infoToastr('No dropdown data found.', 'Info');
        }
      },
      error: (err) => {
        console.error('Error loading form dropdowns:', err);
        const msg =
          err.error?.message ||
          'Failed to load form dropdowns. Please try again later.';
        this.toastService.errorToastr(msg, 'Server Error');
      },
    });
  }

  loadDoctorsBySpecialization(specializationId: number) {
    this.appointmentFormService
      .getDoctorsListBySpecialization(specializationId)
      .subscribe({
        next: (res: any) => {
          if (res.Data && res.Data.length > 0) {
            this.doctors = res.Data;
            this.isSpecializationSelected = true;
          } else {
            this.doctors = [];
            this.isSpecializationSelected = true;
            this.toastService.infoToastr(
              'No doctors found for the selected specialization.',
              'No Doctors'
            );
          }
        },
        error: (err) => {
          console.error('Error fetching doctors by specialization:', err);
          const msg =
            err.error?.message ||
            'Failed to load doctors. Please try again later.';
          this.toastService.errorToastr(msg, 'Server Error');
        },
      });
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

  fetchAvailableSlots(doctorId: number, date: string) {
    this.appointmentFormService.getAvailableSlots(doctorId, date).subscribe({
      next: (res: any) => {
        this.availableSlots = res.Data;
        if (!res || res.Data.length === 0) {
          this.toastService.infoToastr(
            'No available slots for the selected date.',
            'No Slots'
          );
        }
      },
      error: (err) => {
        console.error('Error fetching available slots:', err);
        this.toastService.errorToastr(
          'Failed to load available slots. Please try again later.',
          'Server Error'
        );
        this.availableSlots = [];
      },
    });
  }

  onSpecializationChange() {
    console.log(this.selectedSpecialization);

    if (!this.selectedSpecialization) {
      this.doctors = [];
      this.isSpecializationSelected = false;
      return;
    }

    this.preferredDate = '';
    this.availableSlots = [];
    this.doctors = [];

    this.loadDoctorsBySpecialization(this.selectedSpecialization);
  }

  async onSubmit(form: any) {
    if (!form.valid) {
      this.toastService.errorToastr(
        'Please fill all required fields correctly.',
        'Validation Error'
      );

      return;
    }

    if (this.isFutureDOB(this.patientAppointment.Patient.DateOfBirth)) {
      this.toastService.errorToastr(
        'Date of birth cannot be in the future.',
        'Invalid DOB'
      );
      return;
    }

    const slots = this.patientAppointment.Patient.PreferredSlotsList;
    if (!slots || slots.length === 0) {
      this.toastService.errorToastr(
        'Please select at least one preferred slot.',
        'Slot Missing'
      );
      return;
    }

    if (slots.length > 3) {
      this.toastService.errorToastr(
        'You can select up to 3 preferred slots only.',
        'Slot Limit Exceeded'
      );
      return;
    }

    if (this.isHasInsurances) {
      const p = this.patientAppointment.Patient;

      if (!p.ProviderName || !p.PolicyName || !p.PolicyNumber || !p.ValidTill) {
        this.toastService.errorToastr(
          'Please fill all insurance details completely.',
          'Insurance Details Missing'
        );
        return;
      }

      const validTillDate = new Date(p.ValidTill);
      if (validTillDate < new Date()) {
        this.toastService.errorToastr(
          'Insurance validity must be in the future.',
          'Invalid Insurance Date'
        );
        return;
      }
    }

    if (this.isHasPreviousReport && this.selectedFiles.length === 0) {
      this.toastService.errorToastr(
        'Please upload at least one medical report.',
        'File Missing'
      );
      return;
    }

    if (this.selectedFiles.length > 0) {
      const invalidFile = this.selectedFiles.find(
        (file) =>
          !['application/pdf', 'image/png', 'image/jpeg'].includes(file.type) ||
          file.size > 5 * 1024 * 1024
      );

      if (invalidFile) {
        this.toastService.errorToastr(
          `Invalid file "${invalidFile.name}". Only PDF, JPG, or PNG files under ${this.maxFileSizeMB} are allowed.`,
          'Invalid File'
        );
        return;
      }
    }

    const confirmSubmit =await this.alertService.confirm("Appointment Submit","Are you sure you want to submit this appointment request?","Yes","Cancel")
    if (!confirmSubmit) {
      this.toastService.infoToastr(
        'Appointment submission cancelled.',
        'Cancelled'
      );

      return;
    }

    const patientAppointmentData = this.prepareAppointmentFormData();

    this.submitPatientAppointment(patientAppointmentData);
  }

  submitPatientAppointment(formData: FormData) {
    if (this.isSubmitting) return;

    this.isSubmitting = true;

    this.appointmentFormService
      .insertPatientAppointment(formData)
      .pipe(
        finalize(() => {
          this.isSubmitting = false;
        })
      )
      .subscribe({
        next: (res: any) => {
          if (res.success) {
            this.toastService.successToastr(res.message, 'Appointment Success');
            this.router.navigate(['/landingPage']);
          } else {
            this.toastService.errorToastr(res.message, 'Appointment Error');
          }
        },
        error: (err) => {
          console.error('Appointment submission error:', err);
          this.isSubmitting = true;
          this.toastService.errorToastr(
            'Error while submitting appointment. Please try again later.',
            'Appointment Error'
          );
        },
      });
  }

  prepareAppointmentFormData(): FormData {
    const formData = new FormData();
    formData.append(
      'PatientAppointmentRequest',
      JSON.stringify(this.patientAppointment)
    );

    for (let file of this.selectedFiles) {
      formData.append('Files', file);
    }

    return formData;
  }

  onPreferredDateSelect() {
    const selectedDate = new Date(this.preferredDate);
    const today = new Date(this.today);
    const maxDate = new Date(this.maxDate);

    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);
    maxDate.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
      this.showDateError('Preferred date cannot be earlier than today.');
      return;
    }

    if (selectedDate > maxDate) {
      this.showDateError(
        'Preferred date cannot be more than 14 days from today.'
      );
      return;
    }

    this.fetchAvailableSlots(
      this.patientAppointment.Appointment.DoctorId,
      this.preferredDate
    );
  }

  validateSelectedFiles(maxSizeMB: number = 5): boolean {
    if (!this.selectedFiles || this.selectedFiles.length === 0) {
      this.fileErrorMessage = '';
      return true;
    }

    const maxSize = this.maxFileSizeMB * 1024 * 1024;
    let validFiles: File[] = [];
    let errors: string[] = [];

    for (const file of this.selectedFiles) {
      const sizeMB = (file.size / (1024 * 1024)).toFixed(2);

      if (!this.allowedFileTypes.includes(file.type)) {
        errors.push(`X "${file.name}" - Invalid file type`);
        this.toastService.errorToastr(
          `Invalid file type: ${file.name}. Only PDF, PNG, JPG are allowed.`,
          'File Type Error'
        );
        continue;
      }

      if (file.size > maxSize) {
        errors.push(
          `X "${file.name}" - ${sizeMB} MB (exceeds ${this.maxFileSizeMB} MB limit)`
        );
        this.toastService.errorToastr(
          `File "${file.name}" exceeds ${this.maxFileSizeMB} MB limit.`,
          'File Size Error'
        );
        continue;
      }

      validFiles.push(file);
    }

    this.selectedFiles = validFiles;

    this.fileErrorMessage = errors.length ? errors.join('<br>') : '';

    return errors.length === 0;
  }

  showDateError(message: string) {
    this.toastService.errorToastr(message, 'Requested Date Error');
    this.preferredDate = '';
    this.availableSlots = [];
  }

  toggleSlotSelection(slot: any) {
    const index = this.patientAppointment.Patient.PreferredSlotsList.findIndex(
      (s) => s.AvailableSlotId === slot.AvailableSlotId
    );

    if (index !== -1) {
      this.patientAppointment.Patient.PreferredSlotsList.splice(index, 1);
    } else {
      if (this.patientAppointment.Patient.PreferredSlotsList.length < 3) {
        const newPreferedSlot: PreferredSlots = {
          PreferredSlotId: 0,
          AvailableSlotId: slot.AvailableSlotId,
          PreferredDate: new Date(this.preferredDate),
          PreferredStartTime: slot.StartTime,
          PreferredEndTime: slot.EndTime,
          IsApproved: false,
          IsAlternateSlot: false,
          IsActive: true,
        };
        this.patientAppointment.Patient.PreferredSlotsList.push(
          newPreferedSlot
        );
      } else {
        this.toastService.errorToastr(
          'You can select up to 3 preferred slots only.',
          'Preferred slot Error'
        );
      }
    }
  }

  isSelected(slot: any): boolean {
    return this.patientAppointment.Patient.PreferredSlotsList.some(
      (s) => s.AvailableSlotId === slot.AvailableSlotId
    );
  }

  isFutureDOB(value: any): boolean {
    if (!value) return false;
    return new Date(value) > new Date();
  }

  isTooOldDOB(date: string): boolean {
    return date ? new Date(date) < this.minDateForAge100 : false;
  }
  onDoctorChange() {
    this.preferredDate = '';
    this.availableSlots = [];
  }

  onInsranceOptionChange() {
    this.patientAppointment.Patient.ProviderName = '';
    this.patientAppointment.Patient.PolicyName = '';
    this.patientAppointment.Patient.PolicyNumber = '';
    this.patientAppointment.Patient.ValidTill = null;
  }

  onExistingReportChange() {
    this.selectedFiles = [];
  }

  onFilesSelected(event: any): void {
    const files: FileList = event.target.files;

    for (let i = 0; i < files.length; i++) {
      this.selectedFiles.push(files[i]);
    }

    const isValid = this.validateSelectedFiles();

    if (!isValid) {
      event.target.value = '';
    } else {
      this.toastService.successToastr(
        'All selected files are valid.',
        'File Upload'
      );
    }

    event.target.value = '';
  }

 async removeFile(index: number) {
    const fileName = this.selectedFiles[index].name;


    const removeConform=await this.alertService.confirm(`File Remove","Are you sure you want to remove the file "${fileName}"?`,"Yes","Cancel")
    if ( removeConform) {
      this.selectedFiles.splice(index, 1);
    }
  }

  previewFile(file: any) {
    if (!file) {
      this.toastService.errorToastr(
        'Cannot preview file.',
        'Preview File Error'
      );
      return;
    }

    const blobUrl = URL.createObjectURL(file);
    window.open(blobUrl);

    // this.appointmentFormService.getFileBlob(fileName,folderId).subscribe({
    //   next: (blob) => {
    //     const blobUrl = URL.createObjectURL(blob);
    //     window.open(blobUrl);
    //   },
    //   error: (err) => {
    //     console.error('File preview failed:', err);
    //     this.toastService.errorToastr('Cannot preview file.',"Preview File Error");
    //   }
    // });
  }

 async clearForm(form: any) {
    const confirmClear =await this.alertService.confirm("Form Clear","Are you sure you want to clear the form? All entered data will be lost.","Yes","Cancel")
    if (!confirmClear) {
      this.toastService.infoToastr('Form clear action cancelled.', 'Cancelled');
      return;
    }

    this.patientAppointment = new PatientAppointmentRequest();
    this.selectedFiles = [];
    this.availableSlots = [];
    this.isSpecializationSelected = false;

    form.reset();
    this.toastService.successToastr('Form cleared successfully.', 'Form Reset');
  }
}

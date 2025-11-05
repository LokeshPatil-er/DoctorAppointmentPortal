import { Component } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { PatientAppointmentRequest } from '../../../core/models/patient-appointment-request.model';
import { AppointmentsListFilters } from '../../../core/models/appointments-list-filters.model';
import { AppointmentStatusUpdate } from '../../../core/models/appointment-status-update.model';
import { AvailableSlots } from '../../../core/models/availableSlots.model';
import { AppointmentsListService } from './appointments-list.service';
import { AuthService } from '../../../core/services/auth.service';
import { ToastService } from '../../../core/services/toast.service';
import { AppointmentStatus } from '../../../core/enums/appointment-status.enum';
import { AppointmentStatusHelper } from '../../../core/helper/appointment-status-helper';
import { AppointmentDetailsComponent } from '../../../shared/appointment-details/appointment-details.component';

@Component({
  selector: 'app-appointments-list',
  templateUrl: './appointments-list.component.html',
  styleUrl: './appointments-list.component.css',
})
export class AppointmentsListComponent {
  appointments: PatientAppointmentRequest[] = [];
  appointmentFilters: AppointmentsListFilters = new AppointmentsListFilters();
  appointmentStatusUpdate: AppointmentStatusUpdate =
    new AppointmentStatusUpdate();

  totalAppointmentRecord: number;
  startRecord = 0;
  endRecord = 0;

  selectedAppointment: PatientAppointmentRequest =
    new PatientAppointmentRequest();
  selectedPreferredSlotId: number | null = null;
  selectedAlternateSlotId: number | null = null;
  userId: number;

  alternateSlotAppointment: PatientAppointmentRequest =
    new PatientAppointmentRequest();
  altSlotModalRef: NgbModalRef | null = null;
  altSlotDate: string = '';
  availableAltSlots: AvailableSlots[] = [];

  appointmentStatusList = [];

  constructor(
    private modalService: NgbModal,
    private appointmentListService: AppointmentsListService,
    private authService: AuthService,
    private toastService: ToastService
  ) {}

  appointmentStatus = AppointmentStatus;
  statusHelper = AppointmentStatusHelper;

  ngOnInit() {
    this.userId = this.authService.getUserId();

    this.loadAppointmentStatusList();
    this.loadAppointments();
  }

  loadAppointmentStatusList() {
    this.appointmentListService.getAppointmentStatus().subscribe({
      next: (res: any) => {
        if (res.Data && res.Data.length > 0) {
          this.appointmentStatusList = res.Data;
        } else {
          this.appointmentStatusList = [];
          this.toastService.infoToastr(
            'No appointment statuses found.',
            'Info'
          );
        }
      },
      error: (err) => {
        const msg =
          err.error?.message ||
          'Failed to load appointment statuses. Please try again later.';
        this.toastService.errorToastr(msg, 'Load Error');
        console.error('Error loading appointment statuses:', err);
      },
    });
  }

  loadAppointments() {
    this.appointmentListService
      .getAppointmentByDoctorId(this.appointmentFilters)
      .subscribe({
        next: (res: any) => {
          if (res && res.Data && res.Data.patientsAppointmentsList.length > 0) {
            this.appointments = res.Data.patientsAppointmentsList;
            this.totalAppointmentRecord =
              res.Data.TotalRecored || res.Data.length;
            this.calculateRecordRange();
          } else {
            this.appointments = [];
            this.totalAppointmentRecord = 0;
            this.toastService.infoToastr(
              'No appointments found for the selected filters.',
              'Info'
            );
          }
        },
        error: (err) => {
          const msg =
            err.error?.message ||
            'Failed to load appointments. Please try again later.';
          this.toastService.errorToastr(msg, 'Server Error');
          console.error('Error loading appointments:', err);
        },
      });
  }

  loadAvailableSlots() {
    this.appointmentListService.getAvailableSlots(this.altSlotDate).subscribe({
      next: (res: any) => {
        if (res.Data && res.Data.length > 0) {
          this.availableAltSlots = res.Data;
          console.log(this.availableAltSlots);
        } else {
          this.availableAltSlots = [];
          this.toastService.infoToastr(
            'No available slots found for the selected date.',
            'No Slots'
          );
        }
      },
      error: (err) => {
        console.error('Error loading available slots:', err);
        const msg =
          err.error?.message ||
          'Failed to load available slots. Please try again later.';
        this.toastService.errorToastr(msg, 'Server Error');
      },
    });
  }

  openAlternateSlotModal(appt: any, content: any) {
    this.alternateSlotAppointment = appt;
    this.altSlotDate = '';
    this.availableAltSlots = [];
    this.altSlotModalRef = this.modalService.open(content, {
      size: 'md',
      centered: true,
    });
  }

  viewAppointmentDetails(appt: any) {
    const modalRef = this.modalService.open(AppointmentDetailsComponent, {
      size: 'lg',
      centered: true,
      scrollable: true,
    });

    modalRef.componentInstance.selectedAppointment = appt;
    modalRef.componentInstance.appointmentStatus = this.appointmentStatus;
    modalRef.componentInstance.statusHelper = this.statusHelper;
    modalRef.componentInstance.previewFileEvent.subscribe(
      (fileData: { folderId: number; fileName: string }) => {
        this.previewFile(fileData.fileName, fileData.folderId);
      }
    );

  }

  onAltSlotDateChange() {
    if (!this.altSlotDate) {
      this.selectedAlternateSlotId = null;
      this.availableAltSlots = [];
      return;
    }
    this.selectedAlternateSlotId = null;
    this.availableAltSlots = [];
    this.loadAvailableSlots();
  }

  saveAlternateSlot(actionId: number) {
    if (!this.altSlotDate) {
      this.toastService.errorToastr(
        'Please select an alternate slot date.',
        'Alternate Slot Error'
      );
      return;
    }

    if (!this.selectedAlternateSlotId) {
      this.toastService.errorToastr(
        'Please select a slot timing for the selected date.',
        'Alternate Slot Error'
      );
      return;
    }

    const selectedSlot = this.availableAltSlots.find(
      (slot) => slot.AvailableSlotId === this.selectedAlternateSlotId
    );

    if (!selectedSlot) {
      this.toastService.errorToastr(
        'Invalid slot selection.',
        'Alternate Slot Error'
      );
      return;
    }

    const confirmSave = window.confirm(
      'Are you sure you want to save this alternate slot?'
    );

    if (!confirmSave) {
      this.toastService.infoToastr(
        'Alternate slot save cancelled.',
        'Cancelled'
      );
      return;
    }

    this.appointmentStatusUpdate = {
      PreferredSlotId: 0,
      ActionId: actionId,
      AppointmentId: this.alternateSlotAppointment.Appointment.AppointmentId,
      NewAppointmentStatus: this.appointmentStatus.ALTERNATESLOT,
      AlternateDate: new Date(this.altSlotDate),
      AlternateStartTime: selectedSlot.StartTime,
      AlternateEndTime: selectedSlot.EndTime,
    };

    this.updateAppointment();
    this.toastService.successToastr(
      'Alternate slot saved successfully.',
      'Alternate Slot Saved'
    );
  }

  resetFilters() {
    const confirmReset = window.confirm(
      'Are you sure you want to reset all filters?'
    );

    if (!confirmReset) {
      this.toastService.infoToastr('Filter reset cancelled.', 'Cancelled');
      return;
    }

    this.appointmentFilters = new AppointmentsListFilters();
    this.loadAppointments();

    this.toastService.successToastr(
      'Filters reset successfully.',
      'Filters Cleared'
    );
  }

  acceptAppointment(appt: any, actionId: number) {
    if (this.selectedPreferredSlotId == null) {
      this.toastService.errorToastr(
        'Select preferred slot first',
        'Accept Failed'
      );
      return;
    }

    const confirmAccept = window.confirm(
      'Are you sure you want to accept this appointment?'
    );

    if (!confirmAccept) {
      this.toastService.infoToastr(
        'Appointment acceptance cancelled.',
        'Cancelled'
      );
      return;
    }

    this.appointmentStatusUpdate.NewAppointmentStatus =
      this.appointmentStatus.ACCEPT;
    this.appointmentStatusUpdate.AppointmentId = appt.Appointment.AppointmentId;
    this.appointmentStatusUpdate.PreferredSlotId =
      this.selectedPreferredSlotId ?? 0;
    this.appointmentStatusUpdate.ActionId = actionId;

    this.updateAppointment();
    this.loadAppointments();

    this.toastService.successToastr(
      'Appointment accepted successfully.',
      'Accepted'
    );
  }

  rejectAppointment(appt: any, actionId: number) {
    const confirmReject = window.confirm(
      'Are you sure you want to reject this appointment?'
    );

    if (!confirmReject) {
      this.toastService.infoToastr(
        'Appointment rejection cancelled.',
        'Cancelled'
      );
      return;
    }

    this.appointmentStatusUpdate.NewAppointmentStatus =
      this.appointmentStatus.REJECT;
    this.appointmentStatusUpdate.AppointmentId = appt.Appointment.AppointmentId;
    this.appointmentStatusUpdate.ActionId = actionId;

    this.updateAppointment();
    this.loadAppointments();

    this.toastService.successToastr(
      'Appointment rejected successfully.',
      'Rejected'
    );
  }

  updateAppointment() {
    this.appointmentListService
      .updateAppointmentStatus(this.appointmentStatusUpdate)
      .subscribe({
        next: (res: any) => {
          if (res.Success) {
            if (this.altSlotModalRef) {
              this.altSlotModalRef.close();
            }

            this.toastService.successToastr(
              res.Message || 'Appointment status updated successfully.',
              'Status Success'
            );
            this.appointmentStatusUpdate = new AppointmentStatusUpdate();
            this.loadAppointments();
          } else {
            this.toastService.errorToastr(
              res.Message || 'No appointment record was updated.',
              'Status Error'
            );
          }
        },
        error: (err) => {
          console.error(err);
          this.toastService.errorToastr(
            err.error?.Message ||
              'Something went wrong while updating the appointment.',
            'Server Error'
          );
        },
      });
  }

  getSlotListByStatus(appt: any) {
    const status = appt.Appointment.AppointmentStatus;
    const slots = appt.Patient.PreferredSlotsList ?? [];

    let filteredSlots: any[] = [];

    switch (status) {
      case this.appointmentStatus.ACCEPT:
        filteredSlots = slots.filter((s: any) => s.IsApproved);

        this.selectedPreferredSlotId =
          filteredSlots.length > 0 ? filteredSlots[0].PreferredSlotId : null;
        break;

      case this.appointmentStatus.ALTERNATESLOT:
        filteredSlots = slots.filter((s: any) => s.IsAlternateSlot);

        this.selectedPreferredSlotId =
          filteredSlots.length > 0 ? filteredSlots[0].PreferredSlotId : null;
        break;
      case this.appointmentStatus.PENDING:
      case this.appointmentStatus.REJECT:
        filteredSlots = slots;
        this.selectedPreferredSlotId = null;
        break;

      default:
        filteredSlots = [];
        this.selectedPreferredSlotId = null;
        break;
    }

    return filteredSlots;
  }

  previewFile(fileName: string, folderId: number) {
    this.appointmentListService.getFileBlob(fileName, folderId).subscribe({
      next: (blob) => {
        const blobUrl = URL.createObjectURL(blob);
        window.open(blobUrl);
      },
      error: (err) => {
        console.error('File preview failed:', err);
        this.toastService.errorToastr(
          'Cannot preview file.',
          'Preview File Error'
        );
      },
    });
  }

  getRowNumber(index: number): number {
    return (
      (this.appointmentFilters.PageNumber - 1) *
        this.appointmentFilters.PageSize +
      index +
      1
    );
  }

  getAge(dob: string): number {
    const birth = new Date(dob);
    const diff = Date.now() - birth.getTime();
    return Math.floor(diff / (1000 * 60 * 60 * 24 * 365.25));
  }

  calculateRecordRange(): void {
    if (this.totalAppointmentRecord === 0 || this.appointments.length === 0) {
      this.startRecord = 0;
      this.endRecord = 0;
      return;
    }

    this.startRecord =
      (this.appointmentFilters.PageNumber - 1) *
        this.appointmentFilters.PageSize +
      1;
    this.endRecord =
      this.appointmentFilters.PageNumber * this.appointmentFilters.PageSize;
  }
}

import { Component } from '@angular/core';
import { AppointmentDetailsComponent } from '../../../shared/appointment-details/appointment-details.component';
import { AppointmentsListFilters } from '../../../core/models/appointments-list-filters.model';
import { AppointmentStatus } from '../../../core/enums/appointment-status.enum';
import { AppointmentStatusHelper } from '../../../core/helper/appointment-status-helper';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { ToastService } from '../../../core/services/toast.service';
import { AppointmentsListService } from './appointments-list.service';
import { PatientAppointmentRequest } from '../../../core/models/patient-appointment-request.model';

@Component({
  selector: 'app-appointments-list',
  templateUrl: './appointments-list.component.html',
  styleUrl: './appointments-list.component.css',
})
export class AppointmentsListComponent {
  appointments: PatientAppointmentRequest[] = [];
  doctorList: any[] = [];
  specializationList: any[] = [];
  appointmentStatusList: any[] = [];
  appointmentFilters: AppointmentsListFilters = new AppointmentsListFilters();
  appointmentStatus = AppointmentStatus;
  statusHelper = AppointmentStatusHelper;

  totalAppointmentRecord = 0;
  startRecord = 0;
  endRecord = 0;

  constructor(
    private modalService: NgbModal,
    private appointmentListService: AppointmentsListService,
    private toastService: ToastService
  ) {}

  ngOnInit() {
    this.loadSearchDropDrowns();
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentListService
      .getAppointmentListWithFilter(this.appointmentFilters)
      .subscribe({
        next: (res: any) => {
          this.appointments = res?.data.patientsAppointmentsList ?? [];
          this.totalAppointmentRecord = res?.data.TotalRecored ?? 0;

          if (this.appointments.length === 0) {
            this.toastService.infoToastr(
              'No appointments found for the selected filters.',
              'No Data'
            );
          }

          this.calculateRecordRange();
        },
        error: (err) => {
          console.error('Error loading appointments:', err);

          this.appointments = [];
          this.totalAppointmentRecord = 0;

          this.toastService.errorToastr(
            'Failed to load appointments. Please try again later.',
            'Appointments Error'
          );
        },
      });
  }

  loadSearchDropDrowns() {
    this.appointmentListService.getSearchFiltersDropDrownsList().subscribe({
      next: (res: any) => {
        this.doctorList = res?.data?.doctorsList ?? [];
        this.specializationList = res?.data?.specializationsList ?? [];
        this.appointmentStatusList = res?.data?.statusList ?? [];

        if (
          !res?.data?.doctorsList?.length &&
          !res?.data?.specializationsList?.length &&
          !res?.data?.statusList?.length
        ) {
          this.toastService.infoToastr(
            'No dropdown data found.',
            'DropDrowns Info'
          );
        }
      },
      error: (err) => {
        console.error('Error fetching dropdowns:', err);

        this.doctorList = [];
        this.specializationList = [];
        this.appointmentStatusList = [];

        this.toastService.errorToastr(
          'Failed to load search filters. Please try again later.',
          'DropDrowns Error'
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

        break;

      case this.appointmentStatus.ALTERNATESLOT:
        filteredSlots = slots.filter((s: any) => s.IsAlternateSlot);

        break;
      case this.appointmentStatus.PENDING:
      case this.appointmentStatus.REJECT:
        filteredSlots = slots;

        break;

      default:
        filteredSlots = [];

        break;
    }

    return filteredSlots;
  }

  onPageChange(page: number) {
    this.appointmentFilters.PageNumber = page;
    this.loadAppointments();
  }

  onPageSizeChange() {
    this.appointmentFilters.PageNumber = 1;
    this.loadAppointments();
  }

  resetFilters(form: any) {
    this.appointmentFilters = new AppointmentsListFilters();
    form.resetForm();
    this.appointments = [];
    this.loadAppointments();
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

  getRowNumber(i: number): number {
    return i + 1;
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
}

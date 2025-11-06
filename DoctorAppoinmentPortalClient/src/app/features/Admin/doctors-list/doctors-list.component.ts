import { Component } from '@angular/core';
import { DoctorsListService } from './doctors-list.service';
import { DoctorInfo } from '../../../core/models/doctor-info.model';
import { DoctorListFilter } from '../../../core/models/doctor-list-filter.model';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';
import { AlertService } from '../../../core/services/alert.service';

@Component({
  selector: 'app-doctors-list',
  templateUrl: './doctors-list.component.html',
  styleUrl: './doctors-list.component.css',
})
export class DoctorsListComponent {
  SpcializationsList: any[] = [];

  totalRecords: number;
  startRecord: number = 0;
  endRecord: number = 0;

  doctorFilter: DoctorListFilter = new DoctorListFilter();

  doctorsList: DoctorInfo[] = [];

  constructor(
    private doctorListService: DoctorsListService,
    private toastService: ToastService,
    private alertService:AlertService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadDropDown();

    let storedFiltes = this.doctorListService.getDoctorListFilters();
    if (storedFiltes !== null) {
      this.doctorFilter = storedFiltes;
    }

    this.loadDoctorsList();
  }

  loadDropDown() {
    this.doctorListService.getDoctorListDropDown().subscribe({
      next: (res: any) => {
        if (res.data && res.data.length > 0) {
          this.SpcializationsList = res.data;
        } else {
          this.SpcializationsList = [];
          this.toastService.infoToastr('No specializations found.', 'Info');
        }
      },
      error: (err) => {
        console.error('Error loading specializations:', err);
        this.SpcializationsList = [];
        this.toastService.errorToastr(
          'Failed to load specializations. Please try again later.',
          'DropDown Error'
        );
      },
    });
  }

  loadDoctorsList() {
    this.doctorListService.saveDoctorListFilter(this.doctorFilter);

    this.doctorListService.getAllDoctorsInfo(this.doctorFilter).subscribe({
      next: (res: any) => {
        if (res.data && res.data.Doctors && res.data.Doctors.length > 0) {
          this.doctorsList = res.data.Doctors.map((d: any) => ({
            ...d,
            DoctorspecializationsString:
              d.DoctorSpecializationsList?.map(
                (s: any) => s.Specialization
              ).join(', ') || '',
          }));
          this.totalRecords = res.data.TotalRecords || 0;

          this.calculateRecordRange();
        } else {
          this.doctorsList = [];
          this.totalRecords = 0;
          this.toastService.infoToastr(
            'No doctors found with the applied filters.',
            'Info'
          );
        }
      },
      error: (err) => {
        console.error('Error loading doctors list:', err);
        this.doctorsList = [];
        this.totalRecords = 0;
        this.toastService.errorToastr(
          'Failed to load doctors list. Please try again later.',
          'Doctors List Error'
        );
      },
    });
  }

  isSearchEnable(): boolean {
    if (
      this.doctorFilter.SearchDoctorName ||
      this.doctorFilter.DoctorSpecializationsIdList.length > 0
    )
      return true;

    return false;
  }

  async resetFilter() {
    const confirmReset =await this.alertService.confirm(
      'Please Confirm',
      'Are you sure you want to reset all doctor filters?',
      'Yes, Proceed',
      'Cancel'
    ); 

    if (!confirmReset) {
      this.toastService.infoToastr(
        'Doctor filter reset cancelled.',
        'Cancelled'
      );
      return;
    }

    this.doctorFilter = new DoctorListFilter();
    this.loadDoctorsList();

    this.toastService.successToastr(
      'Doctor filters reset successfully.',
      'Filters Cleared'
    );
  }

  async deleteDoctor(doctorId: number) {
  
    const confirmRemove =await this.alertService.confirm(
      'Please Confirm',
      'Are you sure you want to remove this doctor?',
      'Yes, Proceed',
      'Cancel'
    ); 

    if (!confirmRemove) {
      this.toastService.infoToastr(
        'Doctor remove cancelled.',
        'Cancelled'
      );
      return;
    }


    this.doctorListService
      .deleteDoctorById(doctorId, this.authService.getUserId())
      .subscribe({
        next: (res: any) => {
          if (res && res.success) {
            this.toastService.successToastr(
              'Doctor removed successfully',
              'Remove Success'
            );
            this.loadDoctorsList();
          } else {
            this.toastService.errorToastr(
              'Doctor removal failed. Please try again.',
              'Remove Error'
            );
          }
        },
        error: (err) => {
          console.error('Error deleting doctor:', err);
          this.toastService.errorToastr(
            'An unexpected error occurred while removing the doctor. Please try again later.',
            'Remove Error'
          );
        },
      });
  }

  calculateRecordRange(): void {
    if (this.totalRecords === 0 || this.doctorsList.length === 0) {
      this.startRecord = 0;
      this.endRecord = 0;
      return;
    }

    this.startRecord =
      (this.doctorFilter.PageNumber - 1) * this.doctorFilter.PageSize + 1;
    this.endRecord = this.doctorFilter.PageNumber * this.doctorFilter.PageSize;
  }
}

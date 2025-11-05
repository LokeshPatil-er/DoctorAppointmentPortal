import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { AdminDashboardComponent } from "./admin-dashboard/admin-dashboard.component";
import { AdminRoutingModule } from "./admin-routing.module";
import { FormsModule } from "@angular/forms";
import { DoctorFormComponent } from './doctor-form/doctor-form.component';
import { HttpClientModule } from "@angular/common/http";
import { NgSelectModule } from '@ng-select/ng-select';
import { ToastrModule } from "ngx-toastr";
import { DoctorsListComponent } from './doctors-list/doctors-list.component';
import { NgbDropdownModule, NgbPagination, NgbPaginationModule } from "@ng-bootstrap/ng-bootstrap";
import { AppointmentsListComponent } from './appointments-list/appointments-list.component';

@NgModule({
    declarations: [
        AdminDashboardComponent,
        DoctorFormComponent,
        DoctorsListComponent,
        AppointmentsListComponent
    ],
    imports: [
      CommonModule,
      AdminRoutingModule,
      FormsModule,
      HttpClientModule,
      NgSelectModule,
      NgbPaginationModule,
      NgbDropdownModule
    ]
  })
  export class AdminModule { }
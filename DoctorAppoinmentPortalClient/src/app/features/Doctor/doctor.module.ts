import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { DoctorDashboardComponent } from "./doctor-dashboard/doctor-dashboard.component";
import { DoctorRoutingModule } from "./doctor-routing.module";

import { FormsModule } from "@angular/forms";
import { NgSelectModule } from "@ng-select/ng-select";
import { HttpClientModule } from "@angular/common/http";
import { NgbDropdownModule, NgbPaginationModule } from "@ng-bootstrap/ng-bootstrap";
import { AppointmentDetailsComponent } from "../../shared/appointment-details/appointment-details.component";
import { AppointmentsListComponent } from "./appointments-list/appointments-list.component";

@NgModule({
    declarations: [
        DoctorDashboardComponent,
        AppointmentsListComponent,
        AppointmentDetailsComponent
    ],
    imports: [
      CommonModule,
      FormsModule,
      DoctorRoutingModule,
      NgSelectModule,
      HttpClientModule,
      NgbDropdownModule,
      NgbPaginationModule
    ]
  })
  export class DoctorModule { }
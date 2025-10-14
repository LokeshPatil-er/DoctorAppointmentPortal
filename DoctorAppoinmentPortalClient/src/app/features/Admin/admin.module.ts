import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { AdminDashboardComponent } from "./admin-dashboard/admin-dashboard.component";
import { AdminRoutingModule } from "./admin-routing.module";
import { AppointmentsComponent } from "../../shared/appointments/appointments.component";
import { FormsModule } from "@angular/forms";
import { DoctorFormComponent } from './doctor-form/doctor-form.component';

@NgModule({
    declarations: [
        AdminDashboardComponent,
        AppointmentsComponent,
        DoctorFormComponent
    ],
    imports: [
      CommonModule,
      AdminRoutingModule,
      FormsModule
    ]
  })
  export class AdminModule { }
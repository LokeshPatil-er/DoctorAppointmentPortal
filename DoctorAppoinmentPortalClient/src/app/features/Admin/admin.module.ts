import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { AdminDashboardComponent } from "./admin-dashboard/admin-dashboard.component";
import { AdminRoutingModule } from "./admin-routing.module";
import { AppointmentsComponent } from "../../shared/appointments/appointments.component";

@NgModule({
    declarations: [
        AdminDashboardComponent,
        AppointmentsComponent
    ],
    imports: [
      CommonModule,
      AdminRoutingModule
    ]
  })
  export class AdminModule { }
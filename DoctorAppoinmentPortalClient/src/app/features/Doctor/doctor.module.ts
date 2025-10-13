import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { DoctorDashboardComponent } from "./doctor-dashboard/doctor-dashboard.component";
import { DoctorRoutingModule } from "./doctor-routing.module";

@NgModule({
    declarations: [
        DoctorDashboardComponent
    ],
    imports: [
      CommonModule,
      DoctorRoutingModule
    ]
  })
  export class DoctorModule { }
import { NgModule } from "@angular/core";
import {  RouterModule, Routes } from "@angular/router";
import { AdminDashboardComponent } from "./admin-dashboard/admin-dashboard.component";
import { DoctorFormComponent } from "./doctor-form/doctor-form.component";
import { DoctorRoutingModule } from "../Doctor/doctor-routing.module";
import { DoctorsListComponent } from "./doctors-list/doctors-list.component";


const routes:Routes=[
    {path:'dashboard',component:AdminDashboardComponent},
    {path:'addDoctor',component:DoctorFormComponent,data:{'mode':'add'}},
    {path:'editDoctor/:doctorId', component: DoctorFormComponent,data:{'mode':'edit'}},
    {path:'viewDoctor/:doctorId', component: DoctorFormComponent,data:{'mode':'view'}},
    {path:'doctorsList',component:DoctorsListComponent}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AdminRoutingModule{}
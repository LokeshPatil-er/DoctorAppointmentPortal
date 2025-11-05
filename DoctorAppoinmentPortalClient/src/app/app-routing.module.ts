import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './shared/layout/layout.component';
import { LoginComponent } from './features/Auth/login/login.component';
import { LandingPageComponent } from './features/landing-page/landing-page.component';
import { UserRoles } from './core/enums/user-roles.enum';
import { authGuard } from './core/guards/auth.guard';
const routes: Routes = [
  
  {
    path: '',
    redirectTo: 'landingPage',
    pathMatch: 'full'
  },
  {
    path: '',
    component: LayoutComponent, 
    children: [
      {
        path: 'admin',
        loadChildren: () =>
          import('./features/Admin/admin.module').then(m => m.AdminModule),
        canActivate:[authGuard],
        canLoad:[authGuard],
        data:{role: [UserRoles.Admin]}
      },
      {
        path: 'doctor',
        loadChildren: () =>
          import('./features/Doctor/doctor.module').then(m => m.DoctorModule),
        canActivate:[authGuard],
        canLoad:[authGuard],
        data:{role:[UserRoles.Doctor]}
      }
    ],
  },

  {
    path: 'appointmentRequest',
    loadChildren: () =>
      import('./features/Patient/patient.module').then(m => m.PatientModule)
  },

  { path: 'landingPage', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

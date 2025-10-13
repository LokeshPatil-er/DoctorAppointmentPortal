import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './shared/layout/layout.component';
import { LoginComponent } from './features/Auth/login/login.component';
import { LandingPageComponent } from './features/landing-page/landing-page.component';
const routes: Routes = [
  
  {
    path: '',
    redirectTo: 'login',
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
      },
      {
        path: 'doctor',
        loadChildren: () =>
          import('./features/Doctor/doctor.module').then(m => m.DoctorModule),
      },
    ],
  },
  { path: 'landingPage', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

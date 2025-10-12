import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './shared/layout/layout.component';
import { LandingPageComponent } from './features/landing-page/landing-page.component';
import { LoginComponent } from './features/Auth/login/login.component';

const routes: Routes = [
  
  { path: '', component: LayoutComponent},
  {path:'login',component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/Auth/login/login.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LayoutModule } from './shared/layout/layout.module';
import { LandingPageComponent } from './features/landing-page/landing-page.component';
import { FormsModule } from '@angular/forms';
import {HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import{NgxSpinnerModule} from 'ngx-spinner';
import { authInterceptor } from './core/interceptors/auth.interceptor';

import { CommonModule } from '@angular/common';






@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LandingPageComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgbModule,
    LayoutModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
    NgxSpinnerModule.forRoot({ type: 'ball-scale-multiple' }),
     ToastrModule.forRoot({
      timeOut: 3000,
      extendedTimeOut: 1000,
      closeButton: true,
      progressBar: true,
      positionClass: 'toast-top-center',
      preventDuplicates: true,
      newestOnTop: true,
      tapToDismiss: true,
      autoDismiss: true,
      disableTimeOut: false,
      enableHtml: true 
    })

  ],
  providers: [
    provideClientHydration(),
      provideHttpClient(withInterceptors([authInterceptor]))
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

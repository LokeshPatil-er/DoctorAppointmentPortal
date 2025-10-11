import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/Auth/login/login.component';
import { NavbarComponent } from './shared/layout/navbar/navbar.component';
import { FooterComponent } from './shared/layout/footer/footer.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LayoutModule } from './shared/layout/layout.module';
import { LandingPageComponent } from './features/landing-page/landing-page.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LandingPageComponent
   
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    LayoutModule,
    FormsModule
  ],
  providers: [
    provideClientHydration()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

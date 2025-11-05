import { Component } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { LoginService } from './login.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRoles } from '../../../core/enums/user-roles.enum';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  constructor(
    private loginService: LoginService,
    private authService: AuthService,
    private router: Router,
    private toastService: ToastService
  ) {}

  userEmail: string = '';
  userPassword: string = '';
  showPassword: boolean = false;

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onLogin(form: NgForm) {
    if (form.invalid) {
      this.toastService.errorToastr(
        'Please fill all required fields correctly.',
        'Validation Error'
      );
      return;
    }

    this.loginService
      .loginVerify({ Email: this.userEmail, Password: this.userPassword })
      .subscribe({
        next: (res: any) => {
          if (res.success && res.token) {
            this.authService.setAuthData(res.token);
            const userRole = this.authService.getUserRole();

            this.toastService.successToastr('Login Successful', 'Welcome!');

            if (userRole === UserRoles.Admin) {
              this.router.navigate(['admin/dashboard']);
            } else if (userRole === UserRoles.Doctor) {
              this.router.navigate(['doctor/dashboard']);
            }
          } else {
            const msg = res.message || 'Invalid email or password.';
            this.toastService.errorToastr(msg, 'Login Failed');
          }
        },
        error: (err) => {
          const msg = err.error?.message || 'Login failed due to server error.';
          this.toastService.errorToastr(msg, 'Login Failed');
          console.error('Login error:', err);
        },
      });
  }
}

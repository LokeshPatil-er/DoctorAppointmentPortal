import { Component } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from 'express';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  showPassword = false;
  selectedRole: 'doctor' | 'admin' | null = null;

  onLogin(form: NgForm) {
    if (form.invalid || !this.selectedRole) {
      form.control.markAllAsTouched();
      alert('Please fill all fields and select a role.');
      return;
    }

    const { email, password } = form.value;
    console.log('Logging in:', { email, password, role: this.selectedRole });

    // Role-based redirect
    if (this.selectedRole === 'doctor') {
      // Navigate to doctor dashboard
    } else {
      // Navigate to admin dashboard
    }
  }

}

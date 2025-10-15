import { Component } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { LoginService } from './login.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRoles } from '../../../core/models/user-roles.enum';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
 
  constructor(private loginService:LoginService,private authService:AuthService,private router:Router){

  }
  
    userEmail:string='';
    userPassword:string='';

  onLogin(form: NgForm) {
    if (form.invalid ) {
      form.control.markAllAsTouched();
      return;
    }

    this.loginService.loginVerify({Email:this.userEmail,Password:this.userPassword}).subscribe(res=>{
      if(res.success && res.token)
      {
         this.authService.setAuthData(res.token);
         const userRole=this.authService.getUserRole();
         if(userRole===UserRoles.Admin)
         {
            this.router.navigate(['admin/dashboard'])
         }
         else if(userRole === UserRoles.Doctor)
         {
          this.router.navigate(['doctor/dashboard'])
         }
      }
       
    })
   
  } 

}

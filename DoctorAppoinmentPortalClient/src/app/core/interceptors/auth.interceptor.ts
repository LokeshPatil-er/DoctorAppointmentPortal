import { HttpInterceptorFn } from '@angular/common/http';
import { Inject, inject } from '@angular/core';
import { SpinnerService } from '../services/spinner.service';
import { catchError, finalize } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';


export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const spinner=inject(SpinnerService);
  const authService=inject(AuthService);
  const toast=inject(ToastService);
  const router=inject(Router)

  const token=authService.getToken();
  let modifiedReq=req;

  if(token)
  {
    modifiedReq=req.clone({
      setHeaders:{
        Authorization:`Bearer ${token}`
      }
    })
  }

   spinner.showSpinner();

  return next(modifiedReq).pipe(
    catchError((error)=>{
         if(error.status===401)
          {
          toast.errorToastr("Unauthorized ","Access Error")
            router.navigate(['/login']);
          }else if (error.status === 400) {
            toast.errorToastr("Invalid credentials. Please try again.", "Login Failed");
          } else if (error.status === 404) {
            toast.errorToastr("User not found.", "Not Found");
          } else if (error.status === 500) {
            toast.errorToastr("Server error. Please try again later.", "Internal Server Error");
          } else {
            toast.errorToastr("Something went wrong. Please try again.", "Error");
          }
     
      return throwError(() => error);

    }),
    finalize(() => {
      spinner.hideSpinner();
    })
  )
};

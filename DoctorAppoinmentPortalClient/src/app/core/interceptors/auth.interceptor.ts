import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
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

  let hasShownExpiryToast = false;
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
    catchError((err: HttpErrorResponse) => {
     
      if (
        err.status === 401 &&
        (err.error?.message?.toLowerCase()?.includes('expired') ||
         err.error?.error?.toLowerCase()?.includes('token'))
      ) {
       
        if (!hasShownExpiryToast) {
          hasShownExpiryToast = true;
          toast.warningToastr('Session expired. Please login again.', 'Token Expired');
          authService.logout();
          setTimeout(() => (hasShownExpiryToast = false), 5000);
        }
      }

      return throwError(() => err);
    }),
    finalize(() => {
      spinner.hideSpinner();
    })

  )
};

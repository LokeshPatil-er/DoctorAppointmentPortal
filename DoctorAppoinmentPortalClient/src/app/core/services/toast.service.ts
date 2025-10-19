import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastrService:ToastrService) {}
   
  successToastr(message:string,title:string)
  {
    this.toastrService.success(message,title);
  }

  errorToastr(message:string,title:string)
  {
    this.toastrService.error(message,title);
  }

  warningToastr(message:string,title:string)
  {
    this.toastrService.warning(message,title);
  }

  infoToastr(message:string,title:string)
  {
    this.toastrService.info(message,title);
  }
}

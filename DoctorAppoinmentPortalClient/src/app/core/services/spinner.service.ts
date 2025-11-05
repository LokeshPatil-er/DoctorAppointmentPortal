import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  constructor(private ngxSpinnerService:NgxSpinnerService) { }

  showSpinner()
  {
    this.ngxSpinnerService.show();
  }

  hideSpinner()
  {
    setTimeout(()=>{
      this.ngxSpinnerService.hide();
    },2000)
  }
}

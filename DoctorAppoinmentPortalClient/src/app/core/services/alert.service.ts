import { Injectable } from '@angular/core';
import Swal, { SweetAlertOptions } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor() { }
  private defaultOptions: SweetAlertOptions = {
    buttonsStyling: true,
    confirmButtonColor: '#0d6efd',
    cancelButtonColor: '#6c757d',
    reverseButtons: true,
    heightAuto: false,
    customClass: {
      popup: 'rounded-4 shadow-lg',
      confirmButton: 'btn btn-primary px-3',
      cancelButton: 'btn btn-outline-secondary px-3'
    }
  };

  alert(title: string, text?: string, icon: SweetAlertOptions['icon'] = 'info') {
    return Swal.fire({ ...this.defaultOptions, title, text, icon });
  }

  confirm(
    title: string,
    text = 'Are you sure you want to continue?',
    confirmButtonText = 'Yes',
    cancelButtonText = 'Cancel'
  ): Promise<boolean> {
    return Swal.fire({
      ...this.defaultOptions,
      title,
      text,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText,
      cancelButtonText
    }).then(r => !!r.isConfirmed);
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AppointmentsListService {

  baseUrl:string=environment.apiBaseUrl + "Doctor/"
  constructor(private httpClient:HttpClient) { }

  getAppointmentByDoctorId(filters:any)
  {
    return this.httpClient.post<any>(this.baseUrl+"GetAppointmentsByDoctor",filters)
  }

  updateAppointmentStatus(appointmentUpdated:any)
  {
    return this.httpClient.post(this.baseUrl+"UpdateAppointmentStatus",appointmentUpdated);
  }


   getAvailableSlots(requestedDate:string)
  {
    const params=new HttpParams()
    .set("requestedDate",requestedDate);
    return this.httpClient.get<any>(this.baseUrl+"GetAvailableSlotsAtDoctor",{params})
  }

  getAppointmentStatus()
  {
    return this.httpClient.get<any>(this.baseUrl+"GetAppointmentStatusList");
  }
  
  getFileBlob(filename:string,folderId:number)
  {
    const params=new HttpParams()
    .set("fileName",filename)
    .set("folderId",folderId)

    return this.httpClient.get(this.baseUrl+"GetFileBlobAtDoctor",{params, responseType: 'blob'})
  }

}

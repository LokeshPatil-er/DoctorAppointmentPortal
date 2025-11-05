import { HttpClient, HttpContext, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AppointmentFormService {

  baseUrl:string=environment.apiBaseUrl +  "Patient/"
  constructor(private httpClient:HttpClient) { }

  getAppointmentFormDropDrowns()
  {
    return this.httpClient.get<any>(this.baseUrl+"GetAppointmentFormDropDowns");
  }

  getDoctorsListBySpecialization(specializationId:number)
  {
      const params=new HttpParams()
      .set("SpecializationId",specializationId)

      return this.httpClient.get<any>(this.baseUrl+"DoctorsBySpecialization",{params});
  }

  getAvailableSlots(doctorId:number,requestedDate:string)
  {
    const params=new HttpParams()
    .set("doctorId",doctorId)
    .set("requestedDate",requestedDate);
    return this.httpClient.get<any>(this.baseUrl+"GetAvailableSlotsAtPatient",{params})
  }

  insertPatientAppointment(patientAppointment:FormData)
  {
    return this.httpClient.post<any>(this.baseUrl+"InsertPatientAppointmentRequest",patientAppointment)
  }

  
}

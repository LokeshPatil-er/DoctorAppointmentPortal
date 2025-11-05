import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DoctorFormService {

  baseUrl:string=environment.apiBaseUrl + "Admin/";
  constructor(private httpClient:HttpClient) { }

  getDoctorDropDowns()
  {
    return this.httpClient.get<any>(this.baseUrl+"DoctorFormDropDowns");
  }

  insertOrUpdateDoctor(DoctorInfo:any)
  {
    return this.httpClient.post<any>(this.baseUrl+"DoctorInsertOrUpdate",DoctorInfo);
  }

   loadDoctor(doctorId:number)
  {
    const params=new HttpParams()
    .set("doctorId",doctorId)
    return this.httpClient.get<any>(this.baseUrl+"LoadDoctorInfo",{params});
  }

}

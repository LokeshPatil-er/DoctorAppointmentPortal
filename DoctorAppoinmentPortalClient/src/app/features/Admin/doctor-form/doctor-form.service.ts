import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DoctorFormService {

  baseUrl:string=environment.apiBaseUrl;
  constructor(private httpClient:HttpClient) { }

  getAllDropDownLists()
  {
    return this.httpClient.get<any>(this.baseUrl+"Admin/GetAllDropDownValue");
  }

}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentsListService {

  baseUrl:string=environment.apiBaseUrl + "Admin/"
  constructor(private httpClient:HttpClient) { }

  getSearchFiltersDropDrownsList()
  {
    return this.httpClient.get<any>(this.baseUrl+"AppointmentListFilterDropDown");
  }

  getAppointmentListWithFilter(listFilter:any)
  {
    return this.httpClient.post<any>(this.baseUrl+"AppointmentListWithFiter",listFilter);
  }

   getFileBlob(filename:string,folderId:number)
    {
      const params=new HttpParams()
      .set("fileName",filename)
      .set("folderId",folderId)
  
      return this.httpClient.get(this.baseUrl+"GetFileBlobAtAdmin",{params, responseType: 'blob'})
    }
  
}

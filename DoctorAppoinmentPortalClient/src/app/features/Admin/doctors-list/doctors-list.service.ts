import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { DoctorListFilter } from '../../../core/models/doctor-list-filter.model';
import { JsonPipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class DoctorsListService {

  baseUrl:string=environment.apiBaseUrl + "Admin/";
  constructor(private httpClient:HttpClient) { }

  getAllDoctorsInfo(doctorListFilter:DoctorListFilter)
  {
    return this.httpClient.post<any>(this.baseUrl+"GetDoctorsInfo",doctorListFilter);
  }

  getDoctorListDropDown()
  {
    return this.httpClient.get<any>(this.baseUrl+"DoctorListDropDowns");
  }

  deleteDoctorById(doctorId:number,userId:number)
  {
    const params=new HttpParams()
    .set("doctorId",doctorId)
    .set('deletedBy',userId)
    return this.httpClient.get<any>(this.baseUrl+"DeleteDoctor",{params});
  }
 
  saveDoctorListFilter(filterList:DoctorListFilter)
  {
      if(typeof window !== 'undefined' && !!window.localStorage)
      {
        sessionStorage.setItem("doctorListFilters",JSON.stringify(filterList));
      }
  }

  getDoctorListFilters():DoctorListFilter | null
  {
    let filters:DoctorListFilter | null=new DoctorListFilter();
    if(typeof window !== 'undefined' && !!window.localStorage)
      {
        const getfilters= sessionStorage.getItem("doctorListFilters");
        if (getfilters) 
          {
            filters = JSON.parse(getfilters) as DoctorListFilter;
          } 
      }

      return filters;
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class LoginService {

   baseUrl:string=environment.apiBaseUrl
  constructor(private httpClient:HttpClient) { }

  loginVerify(user:any){
     return this.httpClient.post<any>(this.baseUrl+"Account/Login",user);
  }
}

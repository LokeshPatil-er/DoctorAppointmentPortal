import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class LoginService {

   baseUrl:string=environment.apiBaseUrl + "Account/"
  constructor(private httpClient:HttpClient) { }

  loginVerify(user:any){
     return this.httpClient.post<any>(this.baseUrl+"Login",user);
  }
}

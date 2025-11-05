import { Injectable } from '@angular/core';
import {jwtDecode} from 'jwt-decode';
import { Router } from '@angular/router';
import { AuthUserModel } from '../models/authUser.model';
import { ToastService } from './toast.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private router:Router,private toastService:ToastService) { }

  private hasSessionStorage(): boolean {
    return (typeof window !== 'undefined' && !!window.localStorage);
  }

  getToken():string | null{
     let token:string | null="";
     if(this.hasSessionStorage())
     {
      token=sessionStorage.getItem("Token")
     }
      return token;
  }

  getDecodedToken(): AuthUserModel | null {
    const token = this.getToken();
    if (!token) return null;
    return jwtDecode<AuthUserModel>(token);
  }

  setAuthData(token: string): void {
    if (this.hasSessionStorage()) {
      sessionStorage.setItem("Token", token);

      const decodedToken = this.getDecodedToken();
      if(decodedToken)
      {
        console.log(decodedToken)
        sessionStorage.setItem("UserRole", decodedToken.role);
        sessionStorage.setItem("UserEmail", decodedToken.email);
        sessionStorage.setItem("UserId", decodedToken.userId);
      }
      
    }
  }

  getUserRole(): string | null {
    let userRole=null;
    if(this.hasSessionStorage())
    {
      userRole=sessionStorage.getItem("UserRole");
    }
    return userRole;
  }
  
  getUserId(): number  {
    let id = null
    if(this.hasSessionStorage())
    {
      id=sessionStorage.getItem("UserId");
    }
    
    return id ? parseInt(id) : 0;
  }
  
  getUserEmail(): string | null {

    let userEmail=null;

    if(this.hasSessionStorage())
    {
      userEmail=sessionStorage.getItem("UserEmail");
    }
    return userEmail;
  }

  isTokenExpired(): boolean {
   
    const decoded = this.getDecodedToken();

    if (!decoded || !decoded.exp) 
    {
      return true;
    }

    const expiryTime = decoded.exp * 1000; 
    const currentTime = Date.now();

    return currentTime > expiryTime; 
  }

  logout(){
    if(this.hasSessionStorage())
    {
      sessionStorage.clear();
      this.toastService.successToastr("Logout successfully ...","Logout Success")
      this.router.navigate(['/login']);
    }
  }

}

import { Injectable } from '@angular/core';
import {jwtDecode} from 'jwt-decode';
import { AuthUserModel } from '../models/AuthUser.model';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private router:Router) { }

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
    return sessionStorage.getItem("UserRole");
  }
  
  getUserId(): number | null {
    const id = sessionStorage.getItem("UserId");
    return id ? parseInt(id) : null;
  }
  
  getUserEmail(): string | null {
    return sessionStorage.getItem("UserEmail");
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
      this.router.navigate(['/login']);
    }
  }

}

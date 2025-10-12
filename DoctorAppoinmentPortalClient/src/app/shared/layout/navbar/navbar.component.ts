import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  userRole: string = ''; // 'admin' or 'doctor'
isLoggedIn: boolean = false;
userName: string = '';

 @Output() toggleSidebarEvent = new EventEmitter<void>();

  toggleSidebar() {
    this.toggleSidebarEvent.emit();
  }

ngOnInit() {
  const token = "";
  if (token) {
    this.isLoggedIn = true;
   // const decoded: any = this.decodeToken(token);
    this.userRole = "ADM";
    this.userName = "";
  }
}

logout() {
 
  this.isLoggedIn = false;
  // redirect to login
}

}

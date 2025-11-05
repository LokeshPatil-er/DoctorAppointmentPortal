import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { UserRoles } from '../../../core/enums/user-roles.enum';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  userRoleName: string | null = ''; 
  isLoggedIn: boolean = false;
  userEmail: string | null = '' ;

 @Output() toggleSidebarEvent = new EventEmitter<void>();

 constructor(private authService:AuthService){}

  toggleSidebar() {
    this.toggleSidebarEvent.emit();
  }

ngOnInit() {
  const token = this.authService.getToken();
  if (token) {
    this.isLoggedIn =true;
   this.loadUserInfo()
  }
}
loadUserInfo() {
  this.userEmail = this.authService.getUserEmail() || '';
  const userRoleCode = this.authService.getUserRole() || '';

  
  switch (userRoleCode) {
    case UserRoles.Admin:
      this.userRoleName = "Admin";
      break;
    case UserRoles.Doctor:
      this.userRoleName = "Doctor";
      break;
    default:
      this.userRoleName = userRoleCode; 
  }
}

logout() {
  this.authService.logout();
 
}

}

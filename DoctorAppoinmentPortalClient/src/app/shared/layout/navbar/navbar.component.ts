import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../core/serves/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  userRole: string | null = ''; 
  isLoggedIn: boolean = false;
  userName: string = '' ;

 @Output() toggleSidebarEvent = new EventEmitter<void>();

 constructor(private authService:AuthService){}

  toggleSidebar() {
    this.toggleSidebarEvent.emit();
  }

ngOnInit() {
  const token = "";
  if (token) {
    this.isLoggedIn =true;
    this.userRole = this.authService.getUserRole();
    this.userName = "";
  }
}

logout() {
  this.authService.logout();
 
}

}

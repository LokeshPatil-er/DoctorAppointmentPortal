import { Component, Input } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { UserRoles } from '../../../core/enums/user-roles.enum';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  isSidebarOpen:boolean;

  @Input() isMinimized: boolean = false; 

  userRoles=UserRoles

  constructor(protected authService:AuthService){}

  ngOninit():void{
    this.isSidebarOpen=true;
  }
}

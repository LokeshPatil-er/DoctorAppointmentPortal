import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  isSidebarOpen:boolean;
  @Input() isMinimized: boolean = false; 
  ngOninit():void{
    this.isSidebarOpen=true;
  }
}

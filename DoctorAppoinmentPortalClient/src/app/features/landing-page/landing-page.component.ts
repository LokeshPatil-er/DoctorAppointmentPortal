import { Component } from '@angular/core';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {

  currentYear:number;
  constructor(){}

  ngOnInit():void{
    this.currentYear=Date.now();
  }

  
}

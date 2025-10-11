import { Component } from '@angular/core';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {

  currentYear:number;

  ngOnInit():void{
    this.currentYear=Date.now();
  }

}

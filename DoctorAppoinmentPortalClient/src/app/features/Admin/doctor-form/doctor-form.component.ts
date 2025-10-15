import { Component } from '@angular/core';
import { DoctorFormService } from './doctor-form.service';
import { DoctorInfo } from '../../../core/models/doctor-info.model';

@Component({
  selector: 'app-doctor-form',
  templateUrl: './doctor-form.component.html',
  styleUrl: './doctor-form.component.css'
})
export class DoctorFormComponent {

  doctor: any = {};
  slots: any[] = [];

  countries = [];
  states = [];
  districts = [];
  talukas = [];
  specializations = [];
  qualifications = [];
  bloodGroups = [];
  genders = [];
  daysOfWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

  doctorDetails:DoctorInfo=new DoctorInfo();

  constructor(private doctorFormService:DoctorFormService){}

  ngOnInit():void{
     
      this.doctorFormService.getAllDropDownLists().subscribe(response=>{
          console.log(response)
          this.countries=response.CountriesList;
          this.states=response.StatesList;
          this.districts=response.DistrictsList;
          this.talukas=response.TalukasList;
          this.bloodGroups=response.BloodGroupsList;
          this.genders=response.GendersList;
          this.qualifications=response.QualificationsList;
          this.specializations=response.SpecializationsList;
      })
  }

  addSlot() {
    this.doctorDetails.AvaliableSlotesList.push({ dayOfWeek: '', startTime: '', endTime: '' });
  }

  removeSlot(index: number) {
    this.doctorDetails.AvaliableSlotesList.splice(index, 1);
  }

  onCountryChange(event: any) {
    console.log('Country changed:', event.target.value);
  }

  onStateChange(event: any) {
    console.log('State changed:', event.target.value);
  }

  onDistrictChange(event: any) {
    console.log('District changed:', event.target.value);
  }

  onSubmit(form: any) {
    if (form.valid) {
   
      console.log('Doctor Data:', this.doctorDetails);
    } else {
      alert('Please fill all required fields!');
    }
  }
}

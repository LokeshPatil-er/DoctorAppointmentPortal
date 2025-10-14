import { Component } from '@angular/core';

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

  addSlot() {
    this.slots.push({ dayOfWeek: '', startTime: '', endTime: '' });
  }

  removeSlot(index: number) {
    this.slots.splice(index, 1);
  }

  onCountryChange(event: any) {
    console.log('Country changed:', event.target.value);
    // Load states
  }

  onStateChange(event: any) {
    console.log('State changed:', event.target.value);
    // Load districts
  }

  onDistrictChange(event: any) {
    console.log('District changed:', event.target.value);
    // Load talukas
  }

  onSubmit(form: any) {
    if (form.valid) {
      const payload = { ...this.doctor, slots: this.slots };
      console.log('Doctor Data:', payload);
      // call service here
    } else {
      alert('Please fill all required fields');
    }
  }
}

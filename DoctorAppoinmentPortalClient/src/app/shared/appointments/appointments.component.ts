import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

interface Slot {
  date: string;
  day: string;
  startTime: string;
  endTime: string;
}

interface Appointment {
  id: string;
  patientName: string;
  doctorName: string;
  reason: string;
  preferredSlots: Slot[];
  selectedSlot?: Slot;
  status: 'Accepted' | 'Pending' | 'Rejected';
  alternateSlot?: Slot;
}


@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.css'
})
export class AppointmentsComponent {
  @Input() role: 'admin' | 'doctor' = 'admin';
  @ViewChild('alternateSlotModal') alternateSlotModal!: TemplateRef<any>;

  appointments: Appointment[] = [
    {
      id: 'APT1001',
      patientName: 'William Elmore',
      doctorName: 'Dr. Sarah Mehta',
      reason: 'Regular Checkup',
      preferredSlots: [
        { date: '14 Oct, 2025', day: 'Tuesday', startTime: '10:00 AM', endTime: '10:30 AM' },
      ],
      status: 'Accepted',
    },
    {
      id: 'APT1002',
      patientName: 'Georgie Winters',
      doctorName: 'Dr. Rahul Verma',
      reason: 'Skin Allergy',
      preferredSlots: [
        { date: '15 Oct, 2025', day: 'Wednesday', startTime: '11:30 AM', endTime: '12:00 PM' },
        { date: '16 Oct, 2025', day: 'Thursday', startTime: '2:00 PM', endTime: '2:30 PM' },
      ],
      status: 'Pending',
    },
  ];

  alternateSlot: Slot = { date: '', day: '', startTime: '', endTime: '' };
  selectedAppointment?: Appointment;
  modalRef?: NgbModalRef;

  constructor(private modalService: NgbModal) {}

  selectSlot(appt: Appointment, slot: Slot) {
    appt.selectedSlot = slot;
  }

  onAccept(appt: Appointment) {
    if (appt.selectedSlot) {
      appt.status = 'Accepted';
      alert(
        `Accepted slot for ${appt.patientName}:\n${appt.selectedSlot.date} (${appt.selectedSlot.day})\n${appt.selectedSlot.startTime} - ${appt.selectedSlot.endTime}`
      );
    } else {
      alert('Please select a slot first!');
    }
  }

  onReject(appt: Appointment) {
    appt.status = 'Rejected';
  }

  openAlternateSlotModal(appt: Appointment) {
    this.selectedAppointment = appt;
    this.alternateSlot = { date: '', day: '', startTime: '', endTime: '' };
    this.modalRef = this.modalService.open(this.alternateSlotModal, { centered: true });
  }

  saveAlternateSlot(modal: NgbModalRef) {
    if (this.selectedAppointment) {
      this.selectedAppointment.alternateSlot = { ...this.alternateSlot };
      this.selectedAppointment.status = 'Pending';
      modal.close();
      alert(
        `Alternate slot proposed for ${this.selectedAppointment.patientName}:\n${this.alternateSlot.date} (${this.alternateSlot.day})\n${this.alternateSlot.startTime} - ${this.alternateSlot.endTime}`
      );
    }
  }
}

import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-appointment-details',
  templateUrl: './appointment-details.component.html',
  styleUrl: './appointment-details.component.css',
})
export class AppointmentDetailsComponent {
  @Input() selectedAppointment: any;
  @Input() appointmentStatus: any;
  @Input() statusHelper: any;

  @Output() previewFileEvent = new EventEmitter<{ folderId: number, fileName: string }>();

  slotHeaderLabel = 'Preferred Slots';
  selectedPreferredSlotId: number | null = null;

  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit(): void {
    this.slotHeaderLabel = this.getSlotHeader(
      this.selectedAppointment?.Appointment?.AppointmentStatus
    );
  }

  getSlotHeader(status: string): string {
    switch (status) {
      case this.appointmentStatus?.ACCEPT:
        return 'Accepted Slot';
      case this.appointmentStatus?.ALTERNATESLOT:
        return 'Alternate Slot';
      case this.appointmentStatus?.REJECT:
        return 'Rejected Slot';
      default:
        return 'Preferred Slot';
    }
  }

  getSlotListByStatus(appt: any) {
    const status = appt?.Appointment?.AppointmentStatus;
    const slots = appt?.Patient?.PreferredSlotsList ?? [];
    switch (status) {
      case this.appointmentStatus?.ACCEPT:

        this.selectedPreferredSlotId =slots.find((s: any) => s.IsApproved)?.SlotId ?? null;
        return slots.filter((s: any) => s.IsApproved);

      case this.appointmentStatus?.ALTERNATESLOT:

        this.selectedPreferredSlotId =slots.find((s: any) => s.IsAlternateSlot)?.SlotId ?? null;
        return slots.filter((s: any) => s.IsAlternateSlot);

      case this.appointmentStatus?.PENDING:
      case this.appointmentStatus?.REJECT:

        this.selectedPreferredSlotId = null;
        return slots;

      default:
        this.selectedPreferredSlotId = null;
        return [];
    }
  }

  getAge(dob: string): number {
    const birth = new Date(dob);
    const diff = Date.now() - birth.getTime();
    return Math.floor(diff / (1000 * 60 * 60 * 24 * 365.25));
  }

   previewFileModal(folderId: number, fileName: string,fileType:string) {
    this.previewFileEvent.emit({ folderId, fileName });
  }

  cancel() {
    this.activeModal.dismiss('cancel');
  }
}

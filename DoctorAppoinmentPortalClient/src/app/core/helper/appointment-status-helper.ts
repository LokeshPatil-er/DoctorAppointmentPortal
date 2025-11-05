import { AppointmentStatus } from "../enums/appointment-status.enum";

export class AppointmentStatusHelper {
    static labels: { [key: string]: string } = {
        [AppointmentStatus.ACCEPT]: 'Accepted',
        [AppointmentStatus.REJECT]: 'Rejected',
        [AppointmentStatus.PENDING]: 'Pending',
        [AppointmentStatus.ALTERNATESLOT]: 'Alternate'
    };

  
  static badgeClasses: { [key: string]: string } = {
    [AppointmentStatus.ACCEPT]: 'bg-success',
    [AppointmentStatus.REJECT]: 'bg-danger',
    [AppointmentStatus.PENDING]: 'bg-warning',
    [AppointmentStatus.ALTERNATESLOT]: 'bg-secondary'
  };

 
  static getLabel(statusCode: string): string {
    return this.labels[statusCode] || 'Unknown';
  }

  
  static getBadgeClass(statusCode: string): string {
    return this.badgeClasses[statusCode] || 'bg-secondary';
  }
}

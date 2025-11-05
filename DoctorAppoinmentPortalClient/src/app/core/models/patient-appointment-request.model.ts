import { Appointment } from "./appointment.model";
import { Patient } from "./patient.model";

export class PatientAppointmentRequest {
    Patient:Patient=new Patient()
    Appointment:Appointment=new Appointment()
}

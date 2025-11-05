export class Appointment {
    AppointmentId:number
    PatientId:number
    DoctorId:number
    DoctorFirstName:string
    DoctorLastName:string
    DoctorSpecializations:string[]=[]
    ReasonOfAppointment:string
    MedicalHistory:string
    ApprovedDate:Date
    ApprovedStartTime:string
    ApprovedEndTime:string
    AppointmentStatus:string
    IsActive:boolean
    CreatedOn:Date
}

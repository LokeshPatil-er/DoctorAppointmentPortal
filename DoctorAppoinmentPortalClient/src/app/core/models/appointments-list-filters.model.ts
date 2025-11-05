import { AppointmentStatus } from "../enums/appointment-status.enum"

export class AppointmentsListFilters {

    DoctorId:number
    UserId:number
    PatientName:string
    FromDate:Date=new Date()
    ToDate:Date
    SpecializationId:number
    AppointmentStatus:string= AppointmentStatus.PENDING
    PageNumber:number=1
    PageSize:number=2
}

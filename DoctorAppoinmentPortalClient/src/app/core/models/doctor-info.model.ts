import { AvailableSlots } from "./availableSlots.model"

export class DoctorInfo {
    DoctorId:number
    FirstName:string
    LastName:string
    DateOfBirth:string
    ContactNo:string
    GenderId:number
    RoleShortCode:string
    BloodGroupId:number
    Email:string
    Password:string
    ConsultancyFee:number
    AddressId:number
    AddressLine1:string
    Pincode:string
    CountryId:number
    StateId:number
    DistrictId:number
    TalukaId:number
    ExperienceStartDate:Date
    CreatedBy:number;
    ModifiedBy:number
    DoctorQulificationsIdList:number[]
    DoctorSpecializationsIdList:number[]
    DoctorAvailableSlots:AvailableSlots[]=[]

    DoctorQualificationsList:any[];
    DoctorSpecializationsList:any[];
    DoctorspecializationsString:string=''


}

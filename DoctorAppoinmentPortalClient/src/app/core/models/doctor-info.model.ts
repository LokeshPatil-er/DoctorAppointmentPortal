import { AvailableSlots } from "./availableSlots.model"

export class DoctorInfo {
    DoctorId:number
    FirstName:string
    LastName:string
    DateOfBirth:number
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
    YearOfExperience:number
    QualificationsIdList:number[]
    SpecializationsIdList:number[]
    DoctorAvailableSlots:AvailableSlots[]=[]


}

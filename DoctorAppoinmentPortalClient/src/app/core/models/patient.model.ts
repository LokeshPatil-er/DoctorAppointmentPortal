import { PreferredSlots } from "./preferred-slots.model"
import { ReportFiles } from "./report-files.model"

export class Patient {
    PatientId:number
    FirstName:string
    LastName:string
    DateOfBirth:Date
    Email:string
    ContactNo:string

    GenderId:number
    Gender:string
    BloodGroupId:number
    AddressLine1:string
    Pincode:string
    TalukaId:number
    DistrictId:number
    StateId:number
    CountryId:number
   
    ProviderName:string
    PolicyNumber:string
    PolicyName:string
    ValidTill:Date | null
    PreferredSlotsList:PreferredSlots[]=[]
    UploadReports:ReportFiles[]=[]
    IsActive:boolean
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAPClassLibrary;

namespace DoctorAppointmentPortalServer.Controllers
{
    [JwtAuthorization]
    [Authorize(Roles="ADM")]
    public class AdminController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllDropDownValue()
        {

            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                CountriesOps objCountriesOps=new CountriesOps();
                StatesOps objStatesOps=new StatesOps();
                DistrictsOps objDistrictsOps=new DistrictsOps();
                TalukasOps objTalukasOps=new TalukasOps();

                BloodGroupsOps objBloodGroupsOps=new BloodGroupsOps();
                GendersOps objGendersOps=new GendersOps();
                QualificationsOps objQualificationsOps = new QualificationsOps();
                SpecializationsOps objSpecializationsOps=new SpecializationsOps();

                Doctors objDoctorsModel = new Doctors();

                objDoctorsModel.CountriesList = objCountriesOps.GetCountriesList();
                objDoctorsModel.StatesList = objStatesOps.GetStatesListAllOrByCountryId();
                objDoctorsModel.DistrictsList = objDistrictsOps.GetDistrictsListAllOrByStateId();
                objDoctorsModel.TalukasList = objTalukasOps.GetTalukasListAllOrByDistrictId();
                objDoctorsModel.BloodGroupsList = objBloodGroupsOps.GetBloodGroupsList();
                objDoctorsModel.GendersList = objGendersOps.GetGendersList();
                objDoctorsModel.QualificationsList = objQualificationsOps.GetQualificationsList();
                objDoctorsModel.SpecializationsList = objSpecializationsOps.GetSpecializationsList();
                response = Request.CreateResponse(HttpStatusCode.OK, objDoctorsModel);
            }
            catch(Exception ex)
            {

            }
            return response;
        }

        public HttpResponseMessage DoctorInsertOrUpdate(Doctors doctorModel)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DoctorsOps objDoctorsOps = new DoctorsOps();
                bool result=objDoctorsOps.InsertOrUpdateDoctors(doctorModel);
            }
            catch(Exception ex)
            {

            }

            return response;
        }
    }
}

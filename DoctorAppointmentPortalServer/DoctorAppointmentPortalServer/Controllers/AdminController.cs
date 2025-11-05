using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DAPClassLibrary;
using DAPClassLibrary.Helpers.Services;
using DAPServerLibrary;
using bcrypt = BCrypt.Net;

namespace DoctorAppointmentPortalServer.Controllers
{
    [JwtAuthorization]
    [Authorize(Roles = "ADM")]
    public class AdminController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage DoctorFormDropDowns()
        {
            try
            {
                CountriesOps objCountriesOps = new CountriesOps();
                StatesOps objStatesOps = new StatesOps();
                DistrictsOps objDistrictsOps = new DistrictsOps();
                TalukasOps objTalukasOps = new TalukasOps();
                BloodGroupsOps objBloodGroupsOps = new BloodGroupsOps();
                GendersOps objGendersOps = new GendersOps();
                QualificationsOps objQualificationsOps = new QualificationsOps();
                SpecializationsOps objSpecializationsOps = new SpecializationsOps();

                DoctorFormDropDownsList objDoctorDropDownList = new DoctorFormDropDownsList
                {
                    CountriesList = objCountriesOps.GetCountriesList(),
                    StatesList = objStatesOps.GetStatesListAllOrByCountryId(),
                    DistrictsList = objDistrictsOps.GetDistrictsListAllOrByStateId(),
                    TalukasList = objTalukasOps.GetTalukasListAllOrByDistrictId(),
                    BloodGroupsList = objBloodGroupsOps.GetBloodGroupsList(),
                    GendersList = objGendersOps.GetGendersList(),
                    QualificationsList = objQualificationsOps.GetQualificationsList(),
                    SpecializationsList = objSpecializationsOps.GetSpecializationsList()
                };

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = objDoctorDropDownList });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(DoctorFormDropDowns));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while loading doctor form dropdown data." });
            }
        }

        [HttpGet]
        public HttpResponseMessage AppointmentListFilterDropDown()
        {
            try
            {
                DoctorsOps objDoctorsOps = new DoctorsOps();
                SpecializationsOps objSpecializationsOps = new SpecializationsOps();
                AppointmentStatusOps objAppointmentStatusOps = new AppointmentStatusOps();

                var specializationsList = objSpecializationsOps.GetSpecializationsList();
                var doctorsList = objDoctorsOps.getDoctorsListAllOrBySpecialization();
                var statusList = objAppointmentStatusOps.GetAppointmentStatusesList();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    data = new { doctorsList, specializationsList, statusList }
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(AppointmentListFilterDropDown));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while loading appointment filter data." });
            }
        }

        [HttpPost]
        public HttpResponseMessage AppointmentListWithFiter(AppointmentsListFilters listFilter)
        {
            try
            {
                if (listFilter == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Invalid filter data." });

                AppointmentsOps objAppointmentsOps = new AppointmentsOps();
                var appointmentList = objAppointmentsOps.GetAppointmentsListWithFilter(listFilter);

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = appointmentList });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(AppointmentListWithFiter));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while loading appointment list." });
            }
        }

        [HttpGet]
        public HttpResponseMessage DoctorListDropDowns()
        {
            try
            {
                SpecializationsOps objSpecializationsOps = new SpecializationsOps();
                var specializationsList = objSpecializationsOps.GetSpecializationsList();

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = specializationsList });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(DoctorListDropDowns));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while loading doctor dropdown data." });
            }
        }

        [HttpPost]
        public HttpResponseMessage DoctorInsertOrUpdate(Doctors doctorModel)
        {
            try
            {
                if (doctorModel == null || !ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        success = false,
                        message = "Doctor data is required."
                    });
                }

                DoctorsOps objDoctorsOps = new DoctorsOps();

                UsersOps userOps = new UsersOps();
                userOps.Email = doctorModel.Email;
                userOps.UserId = doctorModel.UserId;

                bool isEmailExists = userOps.CheckEmailExists();

                if (isEmailExists)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, new
                    {
                        success = false,
                        message = "A user with this email already exists. Please use a different email address."
                    });
                }

                if (doctorModel.DoctorId == 0)
                {
                    doctorModel.CreatedBy = UserId;
                    doctorModel.Password = bcrypt.BCrypt.HashPassword(doctorModel.Password);
                }
                else
                {
                    doctorModel.ModifiedBy = UserId;
                }

               
                bool result = objDoctorsOps.insertOrUpdateDoctors(doctorModel);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = result,
                    message = result ? "Doctor saved successfully." : "Failed to save doctor details."
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(DoctorInsertOrUpdate));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "An error occurred while saving doctor data." });
            }
        }

        [HttpPost]
        public HttpResponseMessage GetDoctorsInfo(DoctorTableListFilter doctorListFilter)
        {
            try
            {
                if (doctorListFilter == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Invalid filter data." });

                DoctorsOps objDoctorsOps = new DoctorsOps();
                var objDoctorListResult = objDoctorsOps.getDoctorsDetailsWithFilters(doctorListFilter);

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = objDoctorListResult });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(GetDoctorsInfo));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while retrieving doctor information." });
            }
        }

        [HttpGet]
        public HttpResponseMessage LoadDoctorInfo(int doctorId)
        {
            try
            {
                if (doctorId <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Invalid doctor ID." });

                DoctorsOps objDoctorsOps = new DoctorsOps { DoctorId = doctorId };
                Doctors objDoctors = objDoctorsOps.loadDoctor();

                if (objDoctors == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { success = false, message = "Doctor not found." });

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = objDoctors });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(LoadDoctorInfo));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while loading doctor details." });
            }
        }

        [HttpGet]
        public HttpResponseMessage DeleteDoctor(int doctorId, int deletedBy)
        {
            try
            {
                if (doctorId <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Invalid doctor ID." });

                DoctorsOps objDoctorsOps = new DoctorsOps
                {
                    DoctorId = doctorId,
                    ModifiedBy = deletedBy
                };

                bool isDeleted = objDoctorsOps.deleteDoctorById();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = isDeleted,
                    message = isDeleted ? "Doctor deleted successfully." : "Failed to delete doctor."
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(DeleteDoctor));
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { success = false, message = "Error while deleting doctor." });
            }
        }


        [HttpGet]
        public HttpResponseMessage GetFileBlobAtAdmin(string fileName,int folderId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderId.ToString()))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "File name or folder name is missing.");

              
                string basePath = ConfigurationManager.AppSettings["PatientReportsPath"];
                string folderPath = Path.Combine(basePath, folderId.ToString());
                string filePath = Path.Combine(folderPath, fileName);

                if (!File.Exists(filePath))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

                var fileService = new FileService();
                var fileData = fileService.GetFilePreviewData(filePath);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileData.FileBytes)
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(fileData.MimeType);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = fileData.FileName
                };

                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(GetFileBlobAtAdmin));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
        }

    }
}

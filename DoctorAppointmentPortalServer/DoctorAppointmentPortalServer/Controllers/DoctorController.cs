using DAPClassLibrary;
using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoctorAppointmentPortalServer.Controllers
{
    [JwtAuthorization]
    [Authorize(Roles = "DOC")]
    public class DoctorController : BaseApiController
    {
        [HttpPost]
        public HttpResponseMessage GetAppointmentsByDoctor(AppointmentsListFilters appointmentFilters)
        {
            HttpResponseMessage response;
            try
            {
                appointmentFilters.DoctorId = DoctorId;

                AppointmentsOps objAppointmentsOps = new AppointmentsOps();
                AppointmentsInfoList patientsAppointmentsList = objAppointmentsOps.GetAppointmentsListWithFilter(appointmentFilters);

                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Data = patientsAppointmentsList
                });
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = "Error occurred while fetching appointments.",
                    Details = ex.Message
                });
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAppointmentStatusList()
        {
            HttpResponseMessage response;
            try
            {
                AppointmentStatusOps objAppointmentStatusOps = new AppointmentStatusOps();
                List<AppointmentStatus> appointmentStatusesList = objAppointmentStatusOps.GetAppointmentStatusesList();

                if (appointmentStatusesList == null || appointmentStatusesList.Count == 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.NoContent, new
                    {
                        Success = false,
                        Message = "No appointment statuses found."
                    });
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        Success = true,
                        Data = appointmentStatusesList
                    });
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = "Error occurred while retrieving appointment statuses.",
                    Details = ex.Message
                });
            }
            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAppointmentStatus(AppointmentStatusUpdate appointmentStatusUpdate)
        {
            HttpResponseMessage response;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        success = false,
                        message = "Provide all required data."
                    });
                }

                appointmentStatusUpdate.ModifiedBy = UserId;

                if (appointmentStatusUpdate.PreferredSlotId > 0)
                {
                    PreferredSlotsOps objPreferredSlotsOps = new PreferredSlotsOps
                    {
                        PreferredSlotId = appointmentStatusUpdate.PreferredSlotId
                    };

                    if (objPreferredSlotsOps.CheckPreferredSlotApproved() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.Conflict, new
                        {
                            success = false,
                            message = "The selected slot is already approved for another appointment."
                        });
                    }
                }

                AppointmentsOps objAppointmentsOps = new AppointmentsOps();
                int updated = objAppointmentsOps.UpdateAppointmentStatus(appointmentStatusUpdate);

                if (updated > 0)
                {
                    AppointmentEmailNotifier objAppointmentEmailNotifier = new AppointmentEmailNotifier();

                    if (appointmentStatusUpdate.ActionId == Convert.ToInt32(AppointmentActions.AlternateSlot))
                    {
                        PreferredSlots preferredSlots = new PreferredSlots
                        {
                            PreferredDate = appointmentStatusUpdate.AlternateDate,
                            PreferredStartTime = appointmentStatusUpdate.AlternateStartTime,
                            PreferredEndTime = appointmentStatusUpdate.AlternateEndTime
                        };

                        await objAppointmentEmailNotifier.NotifyAlternateSlotAsync(appointmentStatusUpdate.AppointmentId, preferredSlots);
                    }
                    else
                    {
                        await objAppointmentEmailNotifier.NotifyAppointmentAcceptedAndRejectAsync(appointmentStatusUpdate.AppointmentId, appointmentStatusUpdate.ActionId);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "Appointment status updated successfully."
                    });
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound, new
                    {
                        success = false,
                        message = "No appointment record was updated. Please check the provided data."
                    });
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "An unexpected error occurred while processing your request.",
                    Details = ex.Message
                });
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAvailableSlotsAtDoctor(string requestedDate)
        {
            HttpResponseMessage response;
            try
            {
                AppointmentService appointmentService = new AppointmentService();
                List<AvailableSlots> availableSlots = appointmentService.GetAvailableSlots(DoctorId, requestedDate);

                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    Data = availableSlots
                });
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "Error occurred while retrieving available slots.",
                    Details = ex.Message
                });
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetFileBlobAtDoctor(string fileName, int folderId)
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
                ExceptionLogService.LogExceptionInDB(ex, nameof(AdminController), nameof(GetFileBlobAtDoctor));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}

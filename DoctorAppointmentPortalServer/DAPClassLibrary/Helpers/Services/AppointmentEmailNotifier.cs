using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAPClassLibrary
{
   
    public class AppointmentEmailNotifier
    {
        private MailService objMailService;

        public async Task NotifyAppointmentSubmittedAsync(PatientAppointmentRequest model)
        {

           try
            {
                DoctorsOps objDoctorsOps = new DoctorsOps();


                objDoctorsOps.DoctorId = model.Appointment.DoctorId;
                var getDoctor = objDoctorsOps.loadDoctor();





                var emailData = new Dictionary<string, string>
                {
                    { "PatientName", model.Patient.FirstName +" "+model.Patient.LastName },
                    { "PatientPhone", model.Patient.ContactNo },
                    { "PatientEmail", model.Patient.Email },
                    { "DoctorName",  getDoctor.FirstName +" "+getDoctor.LastName},
                     { "DoctorSpecialization", getDoctor.DoctorSpecialization },
                    { "ReasonForVisit", model.Appointment.ReasonOfAppointment },
                    { "AppointmentRequestID", model.Appointment.AppointmentId.ToString() },
                    { "CurrentYear", DateTime.Now.Year.ToString() },
                    { "PreferredSlotsList", string.Join("", model.Patient.PreferredSlotsList
                                                        .Select(slot => $"<li class='slot-item'><span class='slot-icon'>📅</span>{slot.PreferredDate:dd MMM yyyy} - {slot.PreferredStartTime:hh\\:mm tt} to {slot.PreferredEndTime:hh\\:mm tt}</li>")) }
                };


                await NotifyAsync(
                    EmailScenario.AppointmentSubmitted,
                    model.Patient.Email,
                    getDoctor.Email,
                    emailData
                );
            }
            catch(Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentEmailNotifier), nameof(NotifyAppointmentSubmittedAsync));
               
            }
        }

        public async Task NotifyAppointmentAcceptedAndRejectAsync(int appointmentId,int actionId)
        {

            try
            {
                DoctorsOps objDoctorsOps = new DoctorsOps();
                AppointmentsOps objAppointmentsOps = new AppointmentsOps();

                objAppointmentsOps.AppointmentId = appointmentId;
                var appointmentDetail = objAppointmentsOps.LoadAppointment();

                objDoctorsOps.DoctorId = appointmentDetail.Appointment.DoctorId;
                var getDoctor = objDoctorsOps.loadDoctor();

                string appointmentDateTime = "";
                string preferredSlots = "";

                if (actionId == (int)AppointmentActions.Accept)
                {
                    var approvedSlot = appointmentDetail.Patient.PreferredSlotsList
                                    .FirstOrDefault(x => x.IsApproved);

                    appointmentDateTime = approvedSlot != null
                                 ? $"{approvedSlot.PreferredDate:dd MMM yyyy} {approvedSlot.PreferredStartTime:hh\\:mm tt} - {approvedSlot.PreferredEndTime:hh\\:mm tt}"
                                 : "Not yet approved";
                }
                else
                {
                    preferredSlots = string.Join("", appointmentDetail.Patient.PreferredSlotsList
                                    .Select(slot => $"<li class='slot-item'><span class='slot-icon'>📅</span>{slot.PreferredDate:dd MMM yyyy} {slot.PreferredStartTime:hh\\:mm tt} - {slot.PreferredEndTime:hh\\:mm tt}</li>"));
                }


                var emailData = new Dictionary<string, string>
            {
                { "PatientName", appointmentDetail.Patient.FirstName + " " + appointmentDetail.Patient.LastName },
                { "PatientPhone", appointmentDetail.Patient.ContactNo },
                { "PatientEmail", appointmentDetail.Patient.Email },
                { "DoctorName", getDoctor.FirstName + " " + getDoctor.LastName },
                { "DoctorSpecialization", getDoctor.DoctorSpecialization },
                { "ReasonForVisit", appointmentDetail.Appointment.ReasonOfAppointment },
                {"RejectionReason" ,"Due to doctor unavailble at that time"},
                { "AppointmentID", appointmentDetail.Appointment.AppointmentId.ToString() },
                {"AppointmentDateTime",appointmentDateTime},
                { "ConsultationFee", getDoctor.ConsultancyFee.ToString() },
                { "CurrentYear", DateTime.Now.Year.ToString() }
            };

                EmailScenario emailScenario = 0;

                if (actionId == (int)AppointmentActions.Accept)
                {
                    emailScenario = EmailScenario.AppointmentAccepted;
                }
                else if (actionId == (int)AppointmentActions.Reject)
                {
                    emailData.Add("PreferredSlotsList", preferredSlots);

                    emailScenario = EmailScenario.AppointmentRejected;
                }

                await NotifyAsync(
                    emailScenario,
                    appointmentDetail.Patient.Email,
                    getDoctor.Email,
                    emailData
                );
            }
            catch(Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentEmailNotifier), nameof(NotifyAppointmentAcceptedAndRejectAsync));
               
            }
        }

        public async Task NotifyAlternateSlotResponseAsync(int appointmentId,string action )
        {
            try
            {
                DoctorsOps objDoctorsOps = new DoctorsOps();
                AppointmentsOps objAppointmentsOps = new AppointmentsOps();

                objAppointmentsOps.AppointmentId = appointmentId;
                var appointmentDetail = objAppointmentsOps.LoadAppointment();

                objDoctorsOps.DoctorId = appointmentDetail.Appointment.DoctorId;
                var getDoctor = objDoctorsOps.loadDoctor();

                string appointmentDateTime = "";
                string previouslySuggestedSlots = "";

                EmailScenario emailScenario = 0;

                if (action == AppointmentActions.AlternateSlotAccept.ToString())
                {
                    emailScenario = EmailScenario.AlternateSlotAccept;

                    var approvedSlot = appointmentDetail.Patient.PreferredSlotsList
                                    .FirstOrDefault(x => x.IsApproved);

                    appointmentDateTime = approvedSlot != null
                                 ? $"{approvedSlot.PreferredDate:dd MMM yyyy} {approvedSlot.PreferredStartTime:hh\\:mm tt} - {approvedSlot.PreferredEndTime:hh\\:mm tt}"
                                 : "Not yet approved";
                }
                else
                {
                    emailScenario = EmailScenario.AlternateSlotReject;

                    previouslySuggestedSlots = string.Join("<br/>", appointmentDetail.Patient.PreferredSlotsList
                                    .Where(x => x.IsAlternateSlot)
                                    .Select(slot => $"{slot.PreferredDate:dd MMM yyyy} {slot.PreferredStartTime:hh\\:mm tt} - {slot.PreferredEndTime:hh\\:mm tt}"));
                }

                
                var emailData = new Dictionary<string, string>
                {
                    { "PatientName", appointmentDetail.Patient.FirstName + " " + appointmentDetail.Patient.LastName },
                    { "PatientPhone", appointmentDetail.Patient.ContactNo },
                    { "PatientEmail", appointmentDetail.Patient.Email },
                    { "DoctorName", getDoctor.FirstName + " " + getDoctor.LastName },
                    { "DoctorSpecialization", getDoctor.DoctorSpecialization },
                    { "ReasonForVisit", appointmentDetail.Appointment.ReasonOfAppointment },
                    { "AppointmentID", appointmentDetail.Appointment.AppointmentId.ToString() },
                    { "AppointmentDateTime", appointmentDateTime },
                    {"ConsultationFee" ,getDoctor.ConsultancyFee.ToString()},
                    { "PreviouslySuggestedSlots", previouslySuggestedSlots },
                    { "CurrentYear", DateTime.Now.Year.ToString() }
                };


             
                await NotifyAsync(
                    emailScenario,
                    appointmentDetail.Patient.Email,
                    getDoctor.Email,
                    emailData
                );
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentEmailNotifier), nameof(NotifyAlternateSlotResponseAsync));
            }
        }


        public async Task NotifyAlternateSlotAsync(int appointmentId, PreferredSlots suggestedSlot)
        {
            try
            {
                DoctorsOps objDoctorsOps = new DoctorsOps();
                AppointmentsOps objAppointmentsOps = new AppointmentsOps();


                objAppointmentsOps.AppointmentId = appointmentId;
                var appointmentDetail = objAppointmentsOps.LoadAppointment();


                objDoctorsOps.DoctorId = appointmentDetail.Appointment.DoctorId;
                var getDoctor = objDoctorsOps.loadDoctor();

                int suggestedSlotId= appointmentDetail.Patient.PreferredSlotsList
                                    .Where(x => x.IsAlternateSlot == true)
                                    .Select(x => x.PreferredSlotId)
                                    .FirstOrDefault();

                var token = Guid.NewGuid().ToString();

              
                AppointmentAlternateSlotActionTokenOps tokenOps = new AppointmentAlternateSlotActionTokenOps
                {
                    ActionTokenId=0,
                    Token = token,
                    AppointmentId = appointmentId,
                    PreferredSlotId =suggestedSlotId,
                    ExpiryDate = DateTime.Now.AddHours(24), 
                    IsUsed = false
                };

                tokenOps.SaveData(); 

                string baseUrl = ConfigurationManager.AppSettings["ProjectBaseUrl"];
                string urlWithControllerAction = baseUrl + "Patient/AlternateSlotStatusUpdate";
                string acceptLink = $"{urlWithControllerAction}?token={token}&action={AppointmentActions.AlternateSlotAccept}";
                string rejectLink = $"{urlWithControllerAction}?token={token}&action={AppointmentActions.AlternateSlotReject}";


                var emailData = new Dictionary<string, string>
                {
                    { "PatientName", appointmentDetail.Patient.FirstName + " " + appointmentDetail.Patient.LastName },
                    { "PatientPhone", appointmentDetail.Patient.ContactNo },
                    { "PatientEmail", appointmentDetail.Patient.Email },
                    { "DoctorName", getDoctor.FirstName + " " + getDoctor.LastName },
                    { "DoctorSpecialization", getDoctor.DoctorSpecialization },
                    { "SuggestedDate", suggestedSlot.PreferredDate.ToString("dd MMM yyyy") },
                    { "SuggestedDay", suggestedSlot.PreferredDate.ToString("dddd") },
                    { "SuggestedTime", $"{suggestedSlot.PreferredStartTime:hh\\:mm tt} - {suggestedSlot.PreferredEndTime:hh\\:mm tt}" },
                    { "AcceptSlotLink", acceptLink },
                    { "RejectSlotLink", rejectLink },
                    { "ReasonForVisit", appointmentDetail.Appointment.ReasonOfAppointment },
                    { "ProposedSlotsList", $"{suggestedSlot.PreferredDate:dd MMM yyyy} {suggestedSlot.PreferredStartTime:hh\\:mm tt} - {suggestedSlot.PreferredEndTime:hh\\:mm tt}" },
                    { "ResponseTime", "24 hours" },
                    { "CurrentYear", DateTime.Now.Year.ToString() }
                };





                await NotifyAsync(
                    EmailScenario.AlternateSlot,
                    appointmentDetail.Patient.Email,
                    getDoctor.Email,
                    emailData
                );
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentEmailNotifier), nameof(NotifyAlternateSlotAsync));
              
            }

          
           
        }

        private async Task NotifyAsync(EmailScenario scenario, string patientEmail, string doctorEmail, Dictionary<string, string> data)
        {
            try
            {
                objMailService = new MailService();

                string patientTemplatePath = GetTemplatePath(scenario, isDoctor: false);
                if (File.Exists(patientTemplatePath))
                {
                    string patientTemplateContent = File.ReadAllText(patientTemplatePath);
                    string patientBody = EmailTemplateHelper.PopulateTemplate(patientTemplateContent, data);
                    string patientSubject = GetSubject(scenario, isDoctor: false);
                    await objMailService.SendEmailResponseAsync(patientEmail, patientSubject, patientBody);
                }


                string doctorTemplatePath = GetTemplatePath(scenario, isDoctor: true);
                if (File.Exists(doctorTemplatePath))
                {
                    string doctorTemplateContent = File.ReadAllText(doctorTemplatePath);
                    string doctorBody = EmailTemplateHelper.PopulateTemplate(doctorTemplateContent, data);
                    string doctorSubject = GetSubject(scenario, isDoctor: true);
                    await objMailService.SendEmailResponseAsync(doctorEmail, doctorSubject, doctorBody);
                }
            }
            catch(Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentEmailNotifier), nameof(NotifyAsync));
              
            }
        }

       

        public static string GetTemplatePath(EmailScenario scenario, bool isDoctor)
        {
            try
            {
                string templatePath = string.Empty;

                var template = isDoctor
                    ? EmailTemplatePathConfig.DoctorTemplates
                    : EmailTemplatePathConfig.PatientTemplates;

                template.TryGetValue(scenario , out templatePath);

                if (string.IsNullOrEmpty(templatePath))
                    throw new Exception($"Template not found for scenario '{scenario}' and role '{(isDoctor ? "Doctor" : "Patient")}'.");

                return templatePath;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(EmailTemplatePathConfig), nameof(GetTemplatePath));
                return string.Empty;
            }
        }

        private string GetSubject(EmailScenario scenario, bool isDoctor)
        {
            try
            {
                switch (scenario)
                {
                    case EmailScenario.AppointmentSubmitted:
                        return isDoctor
                            ? "New Appointment Request Received"
                            : "Your Appointment Request Has Been Submitted Successfully";

                    case EmailScenario.AppointmentAccepted:
                        return isDoctor
                            ? "Appointment Accepted Confirmation Sent to Patient"
                            : "Your Appointment Request Has Been Accepted";

                    case EmailScenario.AppointmentRejected:
                        return isDoctor
                            ? "Appointment Request Rejected Confirmation Sent to Patient"
                            : "Your Appointment Request Has Been Rejected";

                    case EmailScenario.AlternateSlot:
                        return isDoctor
                            ? "Alternate Slot Suggested to Patient"
                            : "Doctor Has Suggested an Alternate Appointment Slot";

                    case EmailScenario.AlternateSlotAccept:
                        return isDoctor
                            ? "Patient Accepted the Alternate Slot"
                            : "You Accepted the Doctor’s Alternate Slot";

                    case EmailScenario.AlternateSlotReject:
                        return isDoctor
                            ? "Patient Rejected the Alternate Slot"
                            : "You Rejected the Doctor’s Alternate Slot";

                    default:
                        return "Appointment Notification";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentEmailNotifier), nameof(GetSubject));
              

               
                return "";
            }
        }




    }
}

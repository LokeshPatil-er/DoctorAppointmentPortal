using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class EmailTemplatePathConfig
    {

        private static readonly string EmailTemplateBasePath;

        public static readonly Dictionary<EmailScenario, string> DoctorTemplates;
        public static readonly Dictionary<EmailScenario, string> PatientTemplates;

        static EmailTemplatePathConfig()
        {
            try
            {
                EmailTemplateBasePath = ConfigurationManager.AppSettings["EmailTemplateBasePath"];

                if (string.IsNullOrEmpty(EmailTemplateBasePath))
                    throw new ConfigurationErrorsException("EmailTemplateBasePath key is missing or empty in AppSettings.");
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(EmailTemplatePathConfig), "static constructor");
                EmailTemplateBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
            }

          
            DoctorTemplates = new Dictionary<EmailScenario, string>
            {
                [EmailScenario.AppointmentSubmitted] = Path.Combine(EmailTemplateBasePath, "Doctor", "AppointmentRequestSubmitted.html"),
                [EmailScenario.AppointmentAccepted] = Path.Combine(EmailTemplateBasePath, "Doctor", "AppointmentAccepted.html"),
                [EmailScenario.AppointmentRejected] = Path.Combine(EmailTemplateBasePath, "Doctor", "AppointmentRejected.html"),
                [EmailScenario.AlternateSlot] = Path.Combine(EmailTemplateBasePath, "Doctor", "AlternateSlot.html"),
                [EmailScenario.AlternateSlotAccept] = Path.Combine(EmailTemplateBasePath, "Doctor", "AlternateSlotAccept.html"),
                [EmailScenario.AlternateSlotReject] = Path.Combine(EmailTemplateBasePath, "Doctor", "AlternateSlotReject.html")
            };

          
            PatientTemplates = new Dictionary<EmailScenario, string>
            {
                [EmailScenario.AppointmentSubmitted] = Path.Combine(EmailTemplateBasePath, "Patient", "AppointmentRequestSubmitted.html"),
                [EmailScenario.AppointmentAccepted] = Path.Combine(EmailTemplateBasePath, "Patient", "AppointmentAccepted.html"),
                [EmailScenario.AppointmentRejected] = Path.Combine(EmailTemplateBasePath, "Patient", "AppointmentRejected.html"),
                [EmailScenario.AlternateSlot] = Path.Combine(EmailTemplateBasePath, "Patient", "AlternateSlot.html"),
                [EmailScenario.AlternateSlotAccept] = Path.Combine(EmailTemplateBasePath, "Patient", "AppointmentAccepted.html"),
                [EmailScenario.AlternateSlotReject] = Path.Combine(EmailTemplateBasePath, "Patient", "AlternateSlotReject.html")
            };
        }
    }
}

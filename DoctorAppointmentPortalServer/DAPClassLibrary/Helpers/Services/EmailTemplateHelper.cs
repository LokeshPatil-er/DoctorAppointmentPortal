using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class EmailTemplateHelper
    {
        public static string PopulateTemplate(string templateContent, Dictionary<string, string> placeholders)
        {
            try
            {
                if (string.IsNullOrEmpty(templateContent))
                    throw new ArgumentException("Template content cannot be null or empty.");

                if (placeholders == null || placeholders.Count == 0)
                    throw new ArgumentException("Placeholders dictionary cannot be null or empty.");

                foreach (var placeholder in placeholders)
                {
                    templateContent = templateContent.Replace("{{" + placeholder.Key + "}}", placeholder.Value);
                }

                return templateContent;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(EmailTemplateHelper), nameof(PopulateTemplate));
               

              
                return string.Empty;
            }
        }
    }

}

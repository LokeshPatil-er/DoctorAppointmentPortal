using System;
using System.IO;

namespace DAPClassLibrary.Helpers.Services
{
    public class ExceptionLogService
    {

       
        
       public static void LogExceptionInDB(Exception ex, string className, string methodName)
        {
            try
            {
                if (ex == null)
                    return;

                ExceptionsLog exceptionsLog = new ExceptionsLog()
                {
                    ExceptionMessage = ex.Message,
                    ExceptionStackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Source = ex.Source,
                    TargetSite = $"Class: {className}, Method: {methodName}"
                };

                ExceptionsLogOps objExceptionsLogOps = new ExceptionsLogOps();
                objExceptionsLogOps.exceptionsLogInsert(exceptionsLog);
            }
            catch
            {
               
            }
        }
    }
}

using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace DAPClassLibrary
{
    public class FileService
    {
        public List<ReportFiles> SaveFiles(HttpRequest httpRequest)
        {
            List<ReportFiles> uploadedFilesInfo = new List<ReportFiles>();
            try
            {
                if (httpRequest.Files.Count == 0)
                    return uploadedFilesInfo;

                string folderPath = ConfigurationManager.AppSettings["PatientReportsPath"];
                string tempFolderPath = Path.Combine(folderPath, "TempFolder");

                if (!Directory.Exists(tempFolderPath))
                    Directory.CreateDirectory(tempFolderPath);

                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    var postedFile = httpRequest.Files[i];
                    if (postedFile == null || postedFile.ContentLength == 0)
                        continue;

                    string extension = Path.GetExtension(postedFile.FileName);
                    string guidFileName = Guid.NewGuid() + extension;

                    string filePath = Path.Combine(tempFolderPath, guidFileName);
                    postedFile.SaveAs(filePath);

                    uploadedFilesInfo.Add(new ReportFiles
                    {
                        ReportName = postedFile.FileName,
                        ReportFileName = guidFileName,
                        FileType = postedFile.ContentType
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(FileService), nameof(SaveFiles));
               

              
                throw new Exception("Error while storing files", ex);
            }

            return uploadedFilesInfo;
        }

        public void MoveTempFilesToFinalFolder(int appointmentId)
        {
            try
            {
                string folderPath = ConfigurationManager.AppSettings["PatientReportsPath"];
                string tempFolderPath = Path.Combine(folderPath, "TempFolder");
                string destinationFolderPath = Path.Combine(folderPath, appointmentId.ToString());

                if (!Directory.Exists(destinationFolderPath))
                    Directory.CreateDirectory(destinationFolderPath);

                string[] files = Directory.GetFiles(tempFolderPath);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(destinationFolderPath, fileName);
                    File.Move(file, destPath);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(FileService), nameof(MoveTempFilesToFinalFolder));
              

                throw new Exception("Error while moving files from temp folder to destination folder", ex);
            }
        }

        public void ClearTempFolder()
        {
            try
            {
                string folderPath = ConfigurationManager.AppSettings["PatientReportsPath"];
                string tempFolderPath = Path.Combine(folderPath, "TempFolder");

                if (!Directory.Exists(tempFolderPath))
                    return;

                string[] files = Directory.GetFiles(tempFolderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(FileService), nameof(ClearTempFolder));
             

                throw new Exception("Error while clearing temp folder", ex);
            }
        }

        public FilePreviewData GetFilePreviewData(string filePath)
        {
            FilePreviewData fileData = null;
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string ext = Path.GetExtension(filePath)?.ToLower() ?? string.Empty;
                string mimeType = GetMimeType(ext);

                fileData= new FilePreviewData
                {
                    FileBytes = fileBytes,
                    MimeType = mimeType,
                    FileName = Path.GetFileName(filePath)
                };

                return fileData;
            }
            catch (IOException ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(FileService), nameof(GetFilePreviewData));
               
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(FileService), nameof(GetFilePreviewData));
              
            }
            return fileData;
        }

        private string GetMimeType(string ext)
        {
            switch (ext)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                default:
                    return "application/octet-stream";
            }
        }
    }
}

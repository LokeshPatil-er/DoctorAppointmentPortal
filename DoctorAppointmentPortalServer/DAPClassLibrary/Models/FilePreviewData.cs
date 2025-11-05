using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class FilePreviewData
    {
        public byte[] FileBytes { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }
}

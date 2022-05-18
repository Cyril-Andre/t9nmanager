using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace t9n.api
{
    public class AppSettings
    {
        public string ConfirmationEmailUrl { get; set; }
        public string TemplatesPath { get; set; }
        public string CertFullPath { get; set; }
        public string CertSerialNumber { get; set; }

    }
}

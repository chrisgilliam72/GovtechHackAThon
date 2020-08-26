using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class ToastMessage
    {
        public String Header { get; set; }
        public String Message { get; set; }
        public bool IsError { get; set; }
        public ToastMessage(String header,String message, bool IsError=false)
        {
            this.IsError = IsError;
            Header = header;
            Message = message;
        }
    }
}

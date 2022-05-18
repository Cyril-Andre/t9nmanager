using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace t9n.api.model
{
    public class ApiMessage
    {
        public ApiMessage(int httpStatus, string message=null, string moreInfo=null)
        {
            HttpStatus = httpStatus;
            Message = message;
            MoreInfo = moreInfo;

        }
        public int HttpStatus { get; set; }
        public string Message { get; set; }
        public string MoreInfo { get; set; }
        public object Value { get; set; }

    }
}

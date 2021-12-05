using System;
using System.Collections.Generic;

namespace AcceptsCoin.ApiGateway.Core.Views
{
    public class WebApiMessageResponse
    {
        public bool Success { get; set; }
    }
    public class WebApiErrorMessageResponse
    {
        public List<string> Errors { get; set; }

        public bool Success { get; set; }
    }
}

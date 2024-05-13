using Microsoft.AspNetCore.Mvc;

namespace TaskHub.Business.Models.Custum
{
    public class CustumHttpResponse : ObjectResult 
    {
        public CustumHttpResponse(object content, int statusCode) : base(content)
        {
            StatusCode = statusCode;
        }
    }
}

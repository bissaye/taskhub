using Microsoft.AspNetCore.Mvc;

namespace TaskHub.Business.Models.Custum
{
    public class CustumHttpResponse<T> : ObjectResult where T : class
    {

        public CustumHttpResponse(GenericResponse<T> content, int statusCode) : base(content)
        {
            StatusCode = statusCode;
        }
    }
}

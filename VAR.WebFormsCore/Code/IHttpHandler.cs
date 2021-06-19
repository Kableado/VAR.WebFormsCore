using Microsoft.AspNetCore.Http;

namespace VAR.WebForms.Common.Code
{
    public interface IHttpHandler
    {
        void ProcessRequest(HttpContext context);
    }
}
using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.Code
{
    public interface IHttpHandler
    {
        void ProcessRequest(HttpContext context);
    }
}
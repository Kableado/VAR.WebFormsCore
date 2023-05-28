namespace VAR.WebFormsCore.Code;

public interface IHttpHandler
{
    void ProcessRequest(IWebContext context);
}
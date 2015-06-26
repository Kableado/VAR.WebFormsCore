using System;

namespace VAR.Focus.Web.Code.Entities
{
    public class Session
    {
        public string UserName { get; set; }
        public string SessionToken { get; set; }
        public DateTime StartDate { get; set; }
    }
}

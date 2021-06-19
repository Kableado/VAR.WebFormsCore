using System;
using System.Collections.Generic;
using VAR.WebForms.Common.Pages;

namespace VAR.WebForms.Common.Controls
{
    // TODO: Implememnt control
    public class Control
    {
        public event EventHandler PreInit;
        public event EventHandler Init;
        public event EventHandler Load;
        public event EventHandler PreRender;

        public string CssClass { get; set; }
        public List<Control> Controls { get; set; }

        public bool Visible { get; set; }

        public string ID { get; set; }

        public string ClientID { get; set; }

        public Dictionary<string, string> Style { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public Page Page { get; set; }
    }
}
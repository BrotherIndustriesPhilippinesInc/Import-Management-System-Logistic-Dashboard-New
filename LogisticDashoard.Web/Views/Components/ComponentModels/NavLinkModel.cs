using System.ComponentModel.DataAnnotations;

namespace LogisticDashboard.Web.Views.Components.ComponentModels
{
    public class NavLinkModel
    {
        public string Text { get; set; }

        public string ASPController { get; set; }

        public string Icon { get; set; }

        public string? ASPAction { get; set; }

        public string? BootstrapClass { get; set; }
    }
}
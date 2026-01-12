using Microsoft.AspNetCore.Html;

namespace LogisticDashboard.Web.Models
{
    public class GenericModalViewModel
    {
        public string ModalId { get; set; }
        public string Title { get; set; }
        public string SaveButtonId { get; set; }
        public string SaveText { get; set; } = "Save";
        public string SizeClass { get; set; } = "";
        public IHtmlContent Body { get; set; }   // 👈 important
    }


}

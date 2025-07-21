using LogisticDashboard.Web.Views.Components.ComponentModels;

namespace LogisticDashboard.Web.Views.Components
{
    public class CustomComponent
    {
        public static string Render(string modelName)
        {
            return "~/Views/Components/" + modelName + ".cshtml";
        }
    }
}

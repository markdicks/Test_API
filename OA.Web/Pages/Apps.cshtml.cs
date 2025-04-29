using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OA.Web.Pages
{
    public class AppsModel : PageModel
    {
        public List<AppInfo> AppList { get; set; } = new List<AppInfo>();

        public void OnGet()
        {
            AppList = new List<AppInfo>
            {
                new AppInfo
                {
                    Name = "Remote Desktop Connection",
                    Description = "Able to connect to another computer remotely. Validation done through the API.",
                    Url = "https://github.com/markdicks/RemoteDesktopWPF"
                },
                new AppInfo
                {
                    Name = "App Two",
                    Description = "Visualize analytics using our charting APIs with interactive dashboards and filters.",
                    Url = "/apps/app-two"
                },
                new AppInfo
                {
                    Name = "App Three",
                    Description = "Explore geolocation APIs with live maps and place search functionality.",
                    Url = "/apps/app-three"
                }
                // Add more apps here
            };
        }

        public class AppInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
        }
    }
}

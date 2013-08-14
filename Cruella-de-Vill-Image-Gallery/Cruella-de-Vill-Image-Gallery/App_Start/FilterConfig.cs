using System.Web;
using System.Web.Mvc;

namespace Cruella_de_Vill_Image_Gallery
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace EventMangerBLL.Utility
{
   static class SaveImage
    {
        public static string Save(HttpPostedFileBase img)
        {
            if (img!=null)
            {
                var fileName = Path.GetFileName(img.FileName);
                img.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Data/Pictures/" + fileName));

                return "~/Data/Pictures/" + fileName;
            }
            else
            {
                return "no image";
            }
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Accounting.Data
{
    public class Tools
    {
        public string getProgectPath()
        {
            string path = HttpContext.Current.Server.MapPath("~/Reports/").ToString();
            //path = path.Replace("\\Reports\\", ".Data\\Reports\\");
            return path;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Storage
{
    public class DataStorage
    {
        public void Save(object data)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Hackathon.NaN.MLBox/model.xml");

        }
    }
}

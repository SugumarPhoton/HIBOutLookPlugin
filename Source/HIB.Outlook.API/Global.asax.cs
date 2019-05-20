using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace HIB.Outlook.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string file = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["LogConfigPath"]);
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(file));
            GlobalContext.Properties["source"] = "Outlook API";

            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents(GlobalConfiguration.Configuration);
        }
    }
}

using System.Web.Http;
using Sitecore.Pipelines;

namespace Hackathon.MLBox.Project.Demo.Routes
{
    public class InitRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(config =>
            {
                config.Routes.MapHttpRoute("WebApiRoute", "api/{controller}/{action}/", new
                {
                    controller = "Contact"
                });
            });
        }
    }
}
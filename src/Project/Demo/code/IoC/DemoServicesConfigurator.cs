using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Hackathon.NaN.MLBox.Project.Demo.IoC
{
    public class DemoServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("Hackathon*");
            serviceCollection.AddApiControllers("Hackathon*");
        }
    }
}
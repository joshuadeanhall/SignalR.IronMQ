using System.Configuration;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalR.IronMQ.Example.Startup))]
namespace SignalR.IronMQ.Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var projectId = ConfigurationManager.AppSettings["IronMQProjectId"];
            var token = ConfigurationManager.AppSettings["IronMQToken"];
            GlobalHost.DependencyResolver.UseIronMQ(projectId, token);
        }
    }
}

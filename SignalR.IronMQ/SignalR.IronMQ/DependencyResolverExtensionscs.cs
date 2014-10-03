using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.IronMQ
{
    public static class DependencyResolverExtensionscs
    {
        public static IDependencyResolver UseIronMQ(this IDependencyResolver resolver, string projectId, string server, int port, string token)
        {
            var configuration = new IronMqScaleoutConfiguration(projectId, server, port, token);

            return UseIronMQ(resolver, configuration);
        }

        public static IDependencyResolver UseIronMQ(this IDependencyResolver resolver, string projectId, string token)
        {
            var configuration = new IronMqScaleoutConfiguration(projectId, token);

            return UseIronMQ(resolver, configuration);
        }

        public static IDependencyResolver UseIronMQ(this IDependencyResolver resolver, IronMqScaleoutConfiguration configuration)
        {
            var bus = new Lazy<IronMqMessageBus>(() => new IronMqMessageBus(resolver, configuration));
            resolver.Register(typeof(IMessageBus), () => bus.Value);
            return resolver;
        }
    }
}

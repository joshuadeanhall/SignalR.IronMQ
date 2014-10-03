using System;
using Blacksmith.Core;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.IronMQ
{
    public class IronMqScaleoutConfiguration : ScaleoutConfiguration
    {
        public IronMqScaleoutConfiguration(string projectId, string host, int port, string token)
            : this(MakeConnectionFactory(projectId, host, port, token))
        {
        }

        public IronMqScaleoutConfiguration(string projectId, string token)
            : this(MakeConnectionFactory(projectId, token))
        {
        }

        public IronMqScaleoutConfiguration(Func<Client> connectionFactory)
        {
            ClientFactory = connectionFactory;
        }

        internal Func<Client> ClientFactory { get; private set; } 
        private static Func<Client> MakeConnectionFactory(string projectId, string host, int port, string token)
        {
            return () => new Client(projectId, token, host, port);
        }

        private static Func<Client> MakeConnectionFactory(string projectId,  string token)
        {
            return () => new Client(projectId, token);
        }
    }
}

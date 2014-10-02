using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blacksmith.Core;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.IronMQ
{
    public class IronMQScaleoutConfiguration : ScaleoutConfiguration
    {
        public IronMQScaleoutConfiguration(string projectId, string host, int port, string token)
            : this(MakeConnectionFactory(projectId, host, port, token))
        {
        }

        public IronMQScaleoutConfiguration(string projectId, string token)
            : this(MakeConnectionFactory(projectId, token))
        {
        }

        public IronMQScaleoutConfiguration(Func<Client> connectionFactory)
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

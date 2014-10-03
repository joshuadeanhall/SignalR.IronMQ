using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Blacksmith.Core;
using Blacksmith.Core.Responses;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNet.SignalR.Tracing;
using Message = Microsoft.AspNet.SignalR.Messaging.Message;

namespace SignalR.IronMQ
{
    public class IronMqMessageBus :ScaleoutMessageBus
    {
        private readonly Client _client;
        private readonly TraceSource _trace;


        public IronMqMessageBus(IDependencyResolver resolver, IronMqScaleoutConfiguration configuration) : base(resolver, configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            // Retrieve the trace manager
            var traceManager = resolver.Resolve<ITraceManager>();
            _trace = traceManager["SignalR." + typeof(IronMqMessageBus).Name];
            var ironMqFactory = configuration.ClientFactory;
            _client = ironMqFactory();
            var receivingWorkerTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _client.Queue<IronMqMessageWrapper>().Next().OnError(ErrorOccured).Consume((m, ctx) => OnMessage(m));
                }
            });

            Open(0);

        }

        private void ErrorOccured(Exception obj)
        {
            _trace.TraceError(obj.Message);
        }

        private void OnMessage(Message<IronMqMessageWrapper> message)
        {

            OnReceived(0, ulong.Parse(message.Id), message.Target.ScaleoutMessage);
        }

        protected override Task Send(int streamIndex, IList<Message> messages)
        {
            TraceMessages(messages, "Sending");
            _client.Queue<IronMqMessageWrapper>().Push(new IronMqMessageWrapper(messages));
            return Task.FromResult<object>(null);
        }



        private void TraceMessages(IEnumerable<Message> messages, string messageType)
        {
            if (!_trace.Switch.ShouldTrace(TraceEventType.Verbose))
            {
                return;
            }

            foreach (var message in messages)
            {
                _trace.TraceVerbose("{0} {1} bytes over IronMQ: {2}", messageType, message.Value.Array.Length, message.GetString());
            }
        }
    }
}

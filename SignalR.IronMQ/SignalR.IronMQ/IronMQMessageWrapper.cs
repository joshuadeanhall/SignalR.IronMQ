﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;

namespace SignalR.IronMQ
{
    [Serializable]
    public class IronMQMessageWrapper
    {
        
            [NonSerialized]
            [JsonIgnore]
            private ScaleoutMessage _scaleoutMessage;

            public IronMQMessageWrapper()
            {
            }

            public IronMQMessageWrapper(IList<Message> messages)
            {
                if (messages == null)
                {
                    throw new ArgumentNullException("messages");
                }
                ScaleoutMessage = new ScaleoutMessage(messages);
            }

            public byte[] Bytes { get; set; }

            [JsonIgnore]
            public ScaleoutMessage ScaleoutMessage
            {
                get
                {
                    if (_scaleoutMessage == null)
                    {
                        using (var stream = new MemoryStream(Bytes))
                        {
                            var binaryReader = new BinaryReader(stream);
                            byte[] buffer = binaryReader.ReadBytes(Bytes.Length);
                            _scaleoutMessage = ScaleoutMessage.FromBytes(buffer);
                        }
                    }
                    return _scaleoutMessage;
                }
                set
                {
                    Bytes = value.ToBytes();
                    _scaleoutMessage = value;
                }
            }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TopicHandlerAttribute : Attribute
    {
        public string Topic { get; private set; }
        public TopicHandlerAttribute(string topic)
        {
            this.Topic = topic;
        }
    }
}
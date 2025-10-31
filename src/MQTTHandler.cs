using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MQTTnet;
using src.MqttMessageHandlers;

namespace src
{
    public interface IMQTTHandler
    {
        public Task Handle(MqttApplicationMessageReceivedEventArgs args);
    }

    public class MQTTHandler : IMQTTHandler
    {
        Dictionary<string, ConstructorInfo> _handlers = new Dictionary<string, ConstructorInfo>();
        private readonly IHomeStateProvider _homeStateProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        public MQTTHandler(IHomeStateProvider homeStateProvider, IServiceProvider serviceProvider, ILogger<MQTTHandler> logger)
        {
            _homeStateProvider = homeStateProvider;
            _serviceProvider = serviceProvider;
            _logger = logger;
            var iface_type = typeof(IMqttMessageHandler);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => iface_type.IsAssignableFrom(p));
            foreach (var type in types)
            {
                var topicHandler = type.GetCustomAttribute<TopicHandlerAttribute>();
                if (topicHandler == null)
                    continue;

                var constructors = type.GetConstructors();
                var handlerCtor = constructors.FirstOrDefault(c => c.GetCustomAttribute<HandlerCtorAttribute>() != null);
                if (handlerCtor == null)
                    handlerCtor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);
                if (handlerCtor == null && constructors.Length == 1)
                    handlerCtor = constructors[0];
                if (handlerCtor == null)
                {
                    _logger.LogWarning("No suitable constructor found for {handler}", type.Name);
                    continue;
                }
                
                _handlers.Add(topicHandler.Topic, handlerCtor);
            }
        }

        public async Task Handle(MqttApplicationMessageReceivedEventArgs args)
        {

            var msg = args.ApplicationMessage.ConvertPayloadToString();
            var topic = args.ApplicationMessage.Topic;

            using var scope = _serviceProvider.CreateScope();
            foreach (var t in _handlers.Keys)
            {
                if (MqttTopicFilterComparer.Compare(topic, t) == MqttTopicFilterCompareResult.IsMatch)
                {
                    var handler = Construct(_handlers[t], scope);
                    await handler.Handle(topic, msg);
                }
            }
        }
        
        public IMqttMessageHandler Construct(ConstructorInfo constructor, IServiceScope scope)
        {
            List<object> paramList = [];
            var ps = constructor.GetParameters();
            foreach (var param in ps)
            {
                paramList.Add(scope.ServiceProvider.GetRequiredService(param.ParameterType));
            }

            return (IMqttMessageHandler)constructor.Invoke([.. paramList]);
        }
    }
}
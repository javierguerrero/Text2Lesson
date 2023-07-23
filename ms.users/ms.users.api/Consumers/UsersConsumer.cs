using AutoMapper;
using MediatR;
using ms.rabbitmq.Consumers;
using ms.rabbitmq.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ms.users.api.Consumers
{
    public class UsersConsumer : IConsumer
    {
        private readonly ILogger<UsersConsumer> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private IConnection _connection;

        public UsersConsumer(ILogger<UsersConsumer> logger, IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Unsubscribe() => _connection?.Dispose();

        public void Subscribe()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetValue<string>("Communication:EventBus:HostName"),
            };

            /*
             * Antes surgía el error: RabbitMQ.Client.Exceptions.BrokerUnreachableException: 'None of the specified endpoints were reachable
             * Para solucionarlo, reinicio el contenedor 'ms.users.api'
             */
            _connection = factory.CreateConnection();
            var channel = _connection.CreateModel();

            var queue = typeof(StudentCreatedEvent).Name;

            channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == typeof(StudentCreatedEvent).Name)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var studentCreatedEvent = JsonSerializer.Deserialize<StudentCreatedEvent>(message);

                //TODO: en este punto tengo que usar ASP.NET Identity para crear un nuevo usario.
                //TODO: Refactorizar código para seguir una arquitectura limpia.

                //var createUserAccountCommand = _mapper.Map<CreateUserAccountCommand>(studentCreatedEvent);
                //var result = await _mediator.Send(new CreateUserAccountCommand(createUserAccountCommand.UserName, createUserAccountCommand.Password, createUserAccountCommand.Role));


            }
        }
    }
}
using AutoMapper;
using MediatR;
using ms.rabbitmq.Events;
using ms.rabbitmq.Producers;
using ms.students.domain.Entities;
using ms.students.domain.Interfaces;

namespace ms.students.application.Commands.Handler
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, string>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IProducer _producer;
        private readonly IMapper _mapper;

        public CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper, IProducer producer)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task<string> Handle(CreateStudentCommand createStudentCommand, CancellationToken cancellationToken)
        {
            var result = await _studentRepository.CreateStudent(new Student
            {
                UserName = createStudentCommand.userName,
                FirstName = createStudentCommand.firstName,
                LastName = createStudentCommand.lastName
            });

            var studentCreatedEvent = _mapper.Map<StudentCreatedEvent>(createStudentCommand);
            _producer.Produce(studentCreatedEvent);

            return result;
        }
    }
}
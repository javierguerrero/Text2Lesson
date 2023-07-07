using AutoMapper;
using MediatR;
using ms.students.domain.Entities;
using ms.students.domain.Interfaces;

namespace ms.students.application.Commands.Handler
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, string>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var result = await _studentRepository.CreateStudent(new Student
            {
                UserName = request.userName,
                FirstName = request.firstName,
                LastName = request.lastName
            });

            return result;
        }
    }
}
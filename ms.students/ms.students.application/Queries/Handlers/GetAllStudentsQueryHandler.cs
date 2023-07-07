using AutoMapper;
using MediatR;
using ms.students.application.Responses;
using ms.students.domain.Interfaces;

namespace ms.students.application.Queries.Handlers
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentResponse>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentResponse>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllStudents();
            return _mapper.Map<IEnumerable<StudentResponse>>(students);
        }
    }
}
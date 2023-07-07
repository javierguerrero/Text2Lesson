using MediatR;
using ms.students.application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms.students.application.Queries
{
    public record GetAllStudentsQuery() : IRequest<IEnumerable<StudentResponse>>;
}

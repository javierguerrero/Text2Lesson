using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms.students.application.Commands
{
    public record CreateStudentCommand(string userName, string firstName, string lastName) : IRequest<string>;
}

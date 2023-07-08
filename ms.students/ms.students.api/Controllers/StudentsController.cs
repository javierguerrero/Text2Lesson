using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.students.application.Commands;
using ms.students.application.Queries;
using ms.students.application.Requests;
using System.Data;

namespace ms.students.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllStudents()
            => Ok(await _mediator.Send(new GetAllStudentsQuery()));

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest employee) =>
            Ok(await _mediator.Send(new CreateStudentCommand(employee.UserName, employee.FirstName, employee.LastName)));
    }
}

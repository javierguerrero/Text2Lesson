using ms.students.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms.students.domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudents();
        Task<string> CreateStudent(Student student);
    }
}

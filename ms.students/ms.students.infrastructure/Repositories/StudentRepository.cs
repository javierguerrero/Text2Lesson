using Microsoft.EntityFrameworkCore;
using ms.students.domain.Entities;
using ms.students.domain.Interfaces;
using ms.students.infrastructure.Data;

namespace ms.students.infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<string> CreateStudent(Student student)
        {
            await _context.AddAsync(student);
            await _context.SaveChangesAsync();
            return student.UserName;
        }
    }
}
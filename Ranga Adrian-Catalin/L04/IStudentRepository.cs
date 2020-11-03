using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

public interface IStudentRepository {
    Task<List<StudentEntity>> GetAllStudents();

    Task CreateStudent(StudentEntity student);

    Task Update(string partitionKey, string rowKey, StudentEntity student);

    Task Delete(string partitionKey, string rowKey);

    
}
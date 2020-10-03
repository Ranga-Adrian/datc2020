using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentRepo.Students;
        }

        
        [HttpGet("{Id}")]
        public Student GetStudent([FromRoute]int Id)
        {
            return StudentRepo.Students.FirstOrDefault(s => s.Id == Id);
        }

        [HttpDelete("{Id}")]
        public void DeleteStudent([FromRoute] int Id) => StudentRepo.Students.RemoveAt(Id);

        [HttpPost]
        public string Post([FromBody]Student student)
        {
            try{
                StudentRepo.Students.Add(student);
                return "Adaugarea a fost efectuata cu succes !";
            }
            catch(System.Exception e)
            {
                return "S-a produs o eroare !" + e.Message;
                throw;
            }
        }

        [HttpPut]
        public string Put(int Id, [FromBody]Student student)
        {
            try{
                StudentRepo.Students[Id].Id = student.Id;
                StudentRepo.Students[Id].Name = student.Name;
                StudentRepo.Students[Id].Surname = student.Surname;
                StudentRepo.Students[Id].Faculty = student.Faculty;
                StudentRepo.Students[Id].StudyYear = student.StudyYear;
                return "Modificarea a fost facuta cu succes !";
            }
            catch(System.Exception e)
            {
                return "S-a produs o eroare !" + e.Message;
                throw;
            }
        }
    }
}

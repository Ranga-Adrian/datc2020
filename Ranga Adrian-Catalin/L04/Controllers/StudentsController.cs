using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;

namespace L04.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {

        private IStudentRepository _studentRepository;
        public StudentsController(IStudentRepository studentRepository)
        {

            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task <IEnumerable<StudentEntity>> Get()
        {
            return await _studentRepository.GetAllStudents();
        }

        [HttpPost]
        public async Task<string> Post(StudentEntity student) 
        {
            try
            {
                await _studentRepository.CreateStudent(student);
                return "S-a adaugat cu succes !";
            }
            catch (System.Exception e)
            {
                return "Eroare " + e.Message;
                throw;
            }

        }

        [HttpPut("{partitionKey}/{rowKey}")]
        public async Task <string> Update([FromRoute]string partitionKey, [FromRoute]string rowKey, [FromRoute]StudentEntity student)
        {
            try{
                await _studentRepository.Update(partitionKey,rowKey,student);
                return "Modificarea a fost facuta cu succes !";
            }
            catch(System.Exception e)
            {
                return "S-a produs o eroare !" + e.Message;
                throw;
            }
        }

        [HttpDelete("{partitionKey}/{rowKey}")]
        public async Task<string> Delete([FromRoute] string partitionKey, [FromRoute] string rowKey)
        {
            try
            {
                await _studentRepository.Delete(partitionKey, rowKey);
                return "Stergerea a fost efectuata cu succes !";
            }
            catch(System.Exception e)
            {
                return "S-a produs o eroare " + e.Message;
                throw;
            }

                
        }
    }
}

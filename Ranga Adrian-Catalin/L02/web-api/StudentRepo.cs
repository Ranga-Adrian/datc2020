using System.Collections.Generic;
namespace web_api
{

    public static class StudentRepo{

        public static List<Student> Students = new List<Student>() {

            new Student() { Id = 1, Name = "Ranga", Surname = "Catalin-Adrian", Faculty = "AC", StudyYear = 4},
            new Student() { Id = 2, Name = "Dretcanu", Surname = "Denisa", Faculty = "AMGD", StudyYear = 4},
            new Student() { Id = 3, Name = "Marin", Surname = "Andreea", Faculty = "ETC", StudyYear = 4},
            new Student() { Id = 4, Name = "Balean", Surname = "Vlad", Faculty = "AC", StudyYear = 2},
            new Student() { Id = 5, Name = "Bosca", Surname = "Alexandru-Nicolae", Faculty = "CTI", StudyYear = 4}
        };
    }   
}
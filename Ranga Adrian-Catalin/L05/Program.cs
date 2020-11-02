using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Model;

namespace L05
{
    class Program
    {
        private static CloudTableClient _tableClient;

        private static CloudTable _studentsTable;

        private static TableOperation tableOperation;
	    private static TableResult tableResult;

        private static List<StudentEntity> students  = new List<StudentEntity>();
        static void Main(string[] args)
        {
            Task.Run(async() => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult();
        }
        public async Task CreateStudent(StudentEntity student)
        {
            var insertOperation = TableOperation.Insert(student);

            await _studentsTable.ExecuteAsync(insertOperation);
        }

        static async Task InitializeTable() 
        {
            string _connectionString = "DefaultEndpointsProtocol=https;"
                                + "AccountName=datc2020ranga;"
                                + "AccountKey=2xD2xz0LnT7wsBwjXChd+WuV/gIpNWKLx/9wkoup7RpVxYLkDnFeWjiY1nMoxg7L0HGh270tmvKYtEXTM0FdaQ==;" 
                                + "EndpointSuffix=core.windows.net";
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();

            int option = -1;
	            do
	            {
	                System.Console.WriteLine("1.Adauga un student.");
	                System.Console.WriteLine("2.Sterge un student.");
	                System.Console.WriteLine("3.Afisare studenti.");
	                System.Console.WriteLine("4.Iesire");
	                System.Console.WriteLine("Alegeti optiunea: ");
	                string opt = System.Console.ReadLine();
	                option =Int32.Parse(opt);
	                switch(option)
	                {
	                    case 1:
	                        await AddNewStudent();
	                        break;
	                    case 2:
	                        await DeleteStudent();
	                        break;
	                    case 3:
	                        await DisplayStudents();
	                        break;
	                    case 4:
	                        System.Console.WriteLine("La revedere :)");
	                        break;
	                }
	            }while(option != 4);
        }

        private static async Task<StudentEntity> RetrieveRecordAsync(CloudTable table,string partitionKey,string rowKey)
	        {
	            tableOperation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
	            tableResult = await table.ExecuteAsync(tableOperation);
	            return tableResult.Result as StudentEntity;
	        }

        private static async Task AddNewStudent(){
            
             System.Console.WriteLine("Add the Universitatea:");
	            string university = Console.ReadLine();
	            System.Console.WriteLine("Add the CNP:");
	            string cnp = Console.ReadLine();
	            System.Console.WriteLine("Add the first name:");
	            string firstName = Console.ReadLine();
	            System.Console.WriteLine("Add the last name:");
	            string lastName = Console.ReadLine();
	            System.Console.WriteLine("Add the faculty:");
	            string faculty = Console.ReadLine();
	            System.Console.WriteLine("Add the year of study:");
	            string year = Console.ReadLine();
	
	            StudentEntity stud = await RetrieveRecordAsync(_studentsTable, university, cnp);
	            if(stud == null)
	            {
	                var student = new StudentEntity(university,cnp);
	                student.FirstName = firstName;
	                student.LastName = lastName;
	                student.Faculty = faculty;
	                student.Year = Convert.ToInt32(year);
	                var insertOperation = TableOperation.Insert(student);
	                await _studentsTable.ExecuteAsync(insertOperation);
	                System.Console.WriteLine("This student has been inserted !");
	            }
	            else
	            {
	                System.Console.WriteLine("This student exists already !");
	            }
        }

        private static async Task EditStudent(){
            
            var student = new StudentEntity("UPT", "1970213137244");
            student.FirstName = "Marian";
            student.LastName = "Cojocaru";
            student.Email = "mail@student.uvt.ro";
            student.Year = 2;
            student.ETag = "*";

            var editOperation = TableOperation.Merge(student);

            await _studentsTable.ExecuteAsync(editOperation);
        }

        private static async Task DeleteStudent()
	        {
	            System.Console.WriteLine("Introduce the University:");
	            string university = Console.ReadLine();
	            System.Console.WriteLine("Introduce CNP:");
	            string cnp = Console.ReadLine();
	
	            StudentEntity stud = await RetrieveRecordAsync(_studentsTable, university, cnp);
	            if(stud != null)
	            {
	                var student = new StudentEntity(university,cnp);
	                student.ETag = "*";
	                var deleteOperation = TableOperation.Delete(student);
	                await _studentsTable.ExecuteAsync(deleteOperation);
	                System.Console.WriteLine("The students has been deleted!");
	            }
	            else
	            {
	                System.Console.WriteLine("The student does not exist !");
	            }
	        }

        private async Task<List<StudentEntity>> GetAllStudents()
        {
            var students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results);
            } while(token != null);

            return students;
        }

	        private static async Task DisplayStudents()
	        {
	            await GetAllStudents();
	
	            foreach(StudentEntity std in students)
	            {
					Console.WriteLine("First name : {0}", std.FirstName);
					Console.WriteLine("Last name : {0}", std.LastName);
	                Console.WriteLine("Faculty : {0}", std.Faculty);
	                Console.WriteLine("Year : {0}", std.Year);
	                Console.WriteLine("\n");
	            }
	            students.Clear();
	            
	        }
	    }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Model;
using Models;

namespace L05
{
    class Program
    {
        private static CloudTableClient _tableClient;

        private static CloudTable _studentsTable;

        private static CloudTable universityMetrics;
        private static CloudTableClient tableMetrics;

        private static List<StudentEntity> students  = new List<StudentEntity>();
		private static List<Statistics> metrics  = new List<Statistics>();
		public static int previousTotal = 0;
        static void Main(string[] args)
        {
            Task.Run(async() => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult();
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

           	await DisplayStudents(_connectionString);
        }

		private static async Task<List<StudentEntity>> GetAllStudents()
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

		private static async Task<List<Statistics>> GetAllMetrics()
        {
            TableQuery<Statistics> tableQuery = new TableQuery<Statistics>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Statistics> result = await universityMetrics.ExecuteQuerySegmentedAsync(tableQuery,token);
                token = result.ContinuationToken;
                metrics.AddRange(result.Results);
            }while(token != null);

            return metrics;
        }


	        private static async Task DisplayStudents(string _connectionString)
	        {
	            await GetAllStudents();
	
	        	var accountMetrics = CloudStorageAccount.Parse(_connectionString);
            	tableMetrics = accountMetrics.CreateCloudTableClient();

            	universityMetrics = tableMetrics.GetTableReference("TableMetrics");
            	await universityMetrics.CreateIfNotExistsAsync();
            	await GetAllMetrics();
            	List<int> totalStudents  = new List<int>();
            	int UptCounter = 0;
            	int UvtCounter = 0;
                int EtcCounter = 0;
                int CtiCounter = 0;
            	foreach(StudentEntity std in students)
            	{
                	if(std.PartitionKey == "UPT")
                    	UptCounter++;
                	else if(std.PartitionKey == "ETC")
                    	    EtcCounter++;
                    else if(std.PartitionKey == "CTI")
                    	    CtiCounter++;
                    else
                        UvtCounter++;
            	}
				foreach(Statistics stat in metrics)
				{
					totalStudents.Add(stat.TotalNrOfStudents);
				}

				int total = UptCounter + UvtCounter + EtcCounter + CtiCounter;
            	previousTotal = Convert.ToInt32(totalStudents.Max());
            
				if(total != previousTotal)
				{
					var timeSpan1 = DateTime.Now.ToString("o");
					Statistics stat1 = new Statistics("UPT",timeSpan1);
					stat1.TotalNrOfStudents = UptCounter;
					var insertOperation1 = TableOperation.Insert(stat1);
					await universityMetrics.ExecuteAsync(insertOperation1);

					var timeSpan2 = DateTime.Now.ToString("o");
					Statistics stat2 = new Statistics("UVT",timeSpan2);
					stat2.TotalNrOfStudents = UvtCounter;
					var insertOperation2 = TableOperation.Insert(stat2);
					await universityMetrics.ExecuteAsync(insertOperation2);

                    var timeSpan3 = DateTime.Now.ToString("o");
					Statistics stat3 = new Statistics("ETC",timeSpan3);
					stat3.TotalNrOfStudents = EtcCounter;
					var insertOperation3 = TableOperation.Insert(stat3);
					await universityMetrics.ExecuteAsync(insertOperation3);

                    var timeSpan4 = DateTime.Now.ToString("o");
					Statistics stat4 = new Statistics("CTI",timeSpan4);
					stat4.TotalNrOfStudents = CtiCounter;
					var insertOperation4 = TableOperation.Insert(stat4);
					await universityMetrics.ExecuteAsync(insertOperation4);
					
					var timeSpan5 = DateTime.Now.ToString("o");
					Statistics stat5 = new Statistics("Total",timeSpan5);
					stat5.TotalNrOfStudents = total;
					var insertOperation5 = TableOperation.Insert(stat5);
					await universityMetrics.ExecuteAsync(insertOperation5);
					
				students.Clear();
	            
	       		}
	    }
    }
}

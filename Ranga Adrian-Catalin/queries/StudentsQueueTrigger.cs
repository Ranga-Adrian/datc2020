using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;

namespace DATCStudents
{
    public static class StudentsQueueTrigger
    {
        [FunctionName("StudentsQueueTrigger")]
        [return: Table("studenti")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "datc2020storageranga_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}

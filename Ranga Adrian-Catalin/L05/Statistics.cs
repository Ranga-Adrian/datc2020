using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class Statistics : TableEntity
    {
        public Statistics(string university,string timestamp)
        {
            this.PartitionKey = university;
            this.RowKey = timestamp;
        }
        public Statistics() {}
        public int TotalNrOfStudents{get; set;} 
    }
}
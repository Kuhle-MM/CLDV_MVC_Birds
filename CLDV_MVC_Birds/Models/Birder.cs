using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace CLDV_MVC_Birds.Models
{
    public class Birder : ITableEntity
    {
        [Key]
        public int Birder_Id { get; set; }
        public string? Birder_Name { get; set; }
        public string? Email{ get; set; }
        public string? Password { get; set; }
        


        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}

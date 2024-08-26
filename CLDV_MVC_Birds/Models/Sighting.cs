using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace CLDV_MVC_Birds.Models
{
    public class Sighting : ITableEntity
    {
        [Key]
        public int Signting_Id { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
       
        //introduce validation sample
        [Required(ErrorMessage = "Please select a birder")]
        public int Birder_ID { get; set; }
        [Required(ErrorMessage = "Please select a bird")]
        public int Bird_ID {  get; set; }
        [Required(ErrorMessage = "Please select a date.")]
        public DateTime Sighting_Date { get; set; }
        [Required(ErrorMessage = "Please enter a location")]
        public string? Sighting_Location {  get; set; }
    }
}

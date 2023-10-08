#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice.Models
{
    [Table("Logs")]
    public class LogDetail
    {
        public int Id { get; set; }

        public long Timestamp { get; set; }

        public string Level { get; set; }

        public string Exception { get; set; }

        public string RenderedMessage { get; set; }

        public string Properties { get; set; }
    }
}

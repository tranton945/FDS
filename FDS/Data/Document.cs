using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public float Version { get; set; }
        public string? Creator { get; set; }
        public byte[]? Signature { get; set; }
        //[NotMapped]
        public byte[]? DocFile { get; set; }
        //public IFormFile DocFile { get; set; }

        public int? DocTypeId { get; set; }
        [ForeignKey(nameof(DocTypeId))]
        [JsonIgnore]
        public DocType? DocType { get; set; }

        public int? FlightId { get; set; } 
        [ForeignKey(nameof(FlightId))]
        [InverseProperty("Documents")]
        [JsonIgnore]
        public Flight? Flight { get; set; }

        [JsonIgnore]
        public ICollection<OldDocVer>? OldDocVers { get; set; }
    }
}

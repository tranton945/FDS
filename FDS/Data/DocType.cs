using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("DocType")]
    public class DocType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DocumentType { get; set; }
        public string? Note { get; set; }
        public string? Createtor { get; set; }
        public DateTime CreateDate { get; set; }

        //[NotMapped]
        //[JsonIgnore]
        public ICollection<GroupType>? GroupTypes { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("OldDocVer")]
    public class OldDocVer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public float Version { get; set; }
        public string Creator { get; set; }
        public byte[]? Signature { get; set; }
        public byte[]? DocFile { get; set; }
        public int DocId { get; set; }
        [ForeignKey(nameof(DocId))]
        //[JsonIgnore]
        public Document? Doc { get; set; }
    }
}

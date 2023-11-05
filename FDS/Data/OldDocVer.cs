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
        public string Name { get; set; } = null!;
        public string Note { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public float Version { get; set; }
        public string Creator { get; set; } = null!;
        public byte[]? Signature { get; set; } = null!;

        public int DocId { get; set; }
        [ForeignKey(nameof(DocId))]
        //[JsonIgnore]
        public Document? Doc { get; set; }
    }
}

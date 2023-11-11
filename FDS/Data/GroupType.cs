using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("GroupType")]
    public class GroupType
    {
        [Key]
        public int Id { get; set; }

        public int DocTypeId { get; set; }
        [ForeignKey(nameof(DocTypeId))]
        [JsonIgnore]
        public DocType? DocType { get; set; }

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [JsonIgnore]
        public Group? Group { get; set; }

        [Required]
        public string Permission { get; set; }
    }
}

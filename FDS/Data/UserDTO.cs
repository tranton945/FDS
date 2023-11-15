using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("UserDTO")]
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Permission { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [JsonIgnore]
        public Group? Group { get; set; }
    }
}
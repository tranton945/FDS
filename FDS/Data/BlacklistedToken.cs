using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDS.Data
{
    [Table("BlacklistedToken")]
    public class BlacklistedToken
    {
        [Key]
        public int Id { get; set; }
        public string? Token { get; set; }
    }
}

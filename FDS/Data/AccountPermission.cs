using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDS.Data
{
    [Table("AccountPermission")]
    public class AccountPermission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UsreName { get; set; }

        [Required]
        public bool AllowPermission { get; set;}
    }
}

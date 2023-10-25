using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDS.Data
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }
        //public string DateOfBirt { get; set; }
        public byte age { get; set; }
        public string gender { get; set; }

        public User()
        {
            var a = System.Guid.NewGuid();
            // Tạo một GUID duy nhất khi khởi tạo User mới
            UserID = a;
        }
    }
}

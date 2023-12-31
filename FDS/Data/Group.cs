﻿using FDS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? CreateDate { get; set; }
        public string? Note { get; set; }
        public string? Creator { get; set; }
        public ICollection<UserDTO>? Members { get; set; }
        public ICollection<GroupType>? GroupTypes { get; set; }
    }
}

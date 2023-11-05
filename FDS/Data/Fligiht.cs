using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace FDS.Data
{
    [Table("Flight")]
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FlightNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string PointOfLoading { get; set; }
        [Required]
        public string PointOfUnLoading { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        //[JsonIgnore]
        public ICollection<Document>? Documents { get; set; }

    }

}

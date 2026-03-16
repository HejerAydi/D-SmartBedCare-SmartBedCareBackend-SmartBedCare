using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("LocationLit")]
    public class LocationLit
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("LocationId")]
        public int LocationId { get; set; }

        [Column("LitId")]
        public int LitId { get; set; }

        [Column("PrixLocation")]
        public decimal PrixLocation { get; set; } = 0;

        [Column("FraisTransport")]
        public decimal FraisTransport { get; set; } = 0;

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        [ForeignKey("LitId")]
        public LitMedical? LitMedical { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("LocationRubrique")]
    public class LocationRubrique
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("LocationId")]
        public int LocationId { get; set; }

        [Column("RubriqueId")]
        public int RubriqueId { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }

        [ForeignKey("RubriqueId")]
        public Rubrique? Rubrique { get; set; }
    }
}

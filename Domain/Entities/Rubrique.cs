using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Rubrique")]
    public class Rubrique
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nom")]
        [Required]
        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Column("Description")]
        public string? Description { get; set; }
    }
}

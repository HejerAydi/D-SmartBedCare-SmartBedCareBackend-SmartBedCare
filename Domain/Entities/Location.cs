using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Location")]
    public class Location
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ClientId")]
        public int ClientId { get; set; }

        [Column("DateLocation")]
        public DateTime DateLocation { get; set; } = DateTime.Now;

        [Column("DateLivraison")]
        public DateTime DateLivraison { get; set; }

        [Column("DateRecuperation")]
        public DateTime DateRecuperation { get; set; }

        [Column("Statut")]
        public string Statut { get; set; } = "EnCours"; // "EnCours" | "Terminé" | "Annulé"

        [Column("MontantTotal")]
        public decimal MontantTotal { get; set; } = 0;

        [Column("CreatedBy")]
        public int? CreatedBy { get; set; }

        // Navigation
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        [ForeignKey("CreatedBy")]
        public Utilisateur? Utilisateur { get; set; }

        public ICollection<LocationLit> LocationLits { get; set; } = new List<LocationLit>();
        public ICollection<LocationRubrique> LocationRubriques { get; set; } = new List<LocationRubrique>();
        public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
    }
}

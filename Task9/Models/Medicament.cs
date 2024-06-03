using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task9.Models
{
    [Table("Medicament")]
    public class Medicament
    {
        [Key]
        public int IdMedicament { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        [Required]
        public string Description { get; set; } = null!;

        [MaxLength(100)]
        [Required]
        public string Type { get; set; } = null!;

        public ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; } = new List<Prescription_Medicament>();
    }
}
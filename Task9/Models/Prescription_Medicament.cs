using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Task9.Models
{
    [Table("Prescription_Medicament")]
    [PrimaryKey("IdMedicament", "IdPrescription")]
    public class Prescription_Medicament
    {
        [Key]
        public int IdMedicament { get; set; }

        [Key]
        public int IdPrescription { get; set; }

        public int? Dose { get; set; }

        [MaxLength(100)]
        [Required]
        public string Details { get; set; } = null!;

        public Medicament Medicament { get; set; } = null!;

        public Prescription Prescription { get; set; } = null!;
    }
}
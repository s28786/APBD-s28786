using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task9.Models
{
    [Table("Patient")]
    public class Patient
    {
        [Key]
        public int IdPatient { get; set; }

        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; } = null!;

        [Column(TypeName = "datetime")]
        [Required]
        public DateTime Birthdate { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
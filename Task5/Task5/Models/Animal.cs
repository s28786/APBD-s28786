using System.ComponentModel.DataAnnotations;

namespace Task5.Models
{
    public class Animal

    {
        [Key]
        public int IdAnimal { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string Area { get; set; }
    }
}
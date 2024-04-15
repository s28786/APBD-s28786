using System.ComponentModel.DataAnnotations.Schema;

namespace Task4.models
{
    public class Visit
    {
        // id of visists [Guid], datesofvisits [DateTime], AnimalId, Description, Price
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DateOfVisit { get; set; }

        public Guid AnimalId { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }
    }
}
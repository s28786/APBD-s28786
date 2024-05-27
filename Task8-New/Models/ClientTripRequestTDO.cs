using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task8_New.Models
{
    public class ClientTripRequestTDO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telephone { get; set; } = null!;

        public string Pesel { get; set; } = null!;
        public string TripName { get; set; } = null!;
        public DateTime? PaymentDate { get; set; }
    }
}
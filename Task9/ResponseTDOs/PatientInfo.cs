using Microsoft.AspNetCore.Routing.Constraints;
using Task9.Models;

namespace Task9.ResponseTDOs
{
    public class PatientInfo
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }

        public List<PrescriptionInfo> prescriptions { get; set; } = new List<PrescriptionInfo>();
    }

    public class PrescriptionInfo
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<Medicament> Medicaments { get; set; } = new List<Medicament>();
        public DoctorInfo Doctor { get; set; }
    }

    public class DoctorInfo
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
    }
}
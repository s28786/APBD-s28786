using Task9.Models;

namespace Task9.RequestTDOs
{
    public class AddPrescriptionCommand
    {
        public PatientCommand Patient { get; set; }
        public List<MedicamentCommand> Medicaments { get; set; }

        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorCommand Doctor { get; set; }
    }

    public class MedicamentCommand
    {
        public int IdMedicament { get; set; }
        public int Dose { get; set; }
        public string Details { get; set; }
    }

    public class PatientCommand
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }
    }

    public class DoctorCommand
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
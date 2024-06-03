using Task9.Context;
using Task9.Models;
using Task9.ResponseTDOs;

namespace Task9.Services
{
    public class PatientService : IPatientService
    {
        /*Prepare an endpoint that allows displaying all data about a specific
        patient, along with a list of prescriptions and medications they have
        taken. We would like the response to include all information about the
        medications and doctors. The data about prescriptions should be
        sorted by the DueDate field.
        */
        private readonly s28786DbContext _context;

        public PatientService(s28786DbContext context)
        {
            _context = context;
        }

        public async Task<object> GetPatientInfo(int patientId)
        {
            PatientInfo patientInfo = new PatientInfo();
            Patient patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                throw new Exception("Patient does not exist");
            }
            patientInfo.IdPatient = patient.IdPatient;
            patientInfo.FirstName = patient.FirstName;
            patientInfo.LastName = patient.LastName;
            patientInfo.Birthdate = patient.Birthdate;
            List<PrescriptionInfo> prescriptionInfos = new List<PrescriptionInfo>();
            foreach (var prescription in _context.Prescriptions.Where(p => p.IdPatient == patientId).OrderBy(p => p.DueDate))
            {
                PrescriptionInfo prescriptionInfo = new PrescriptionInfo();
                prescriptionInfo.IdPrescription = prescription.IdPrescription;
                prescriptionInfo.Date = prescription.Date;
                prescriptionInfo.DueDate = prescription.DueDate;
                String firstName = _context.Doctors.Find(prescription.IdDoctor).FirstName;

                prescriptionInfo.Doctor = new DoctorInfo
                {
                    IdDoctor = prescription.IdDoctor,
                    FirstName = firstName
                };
                foreach (var perscription_medicament in _context.Prescription_Medicaments.Where(pm => pm.IdPrescription == prescription.IdPrescription))
                {
                    Medicament medicamentRes = _context.Medicaments.Find(perscription_medicament.IdMedicament);
                    prescriptionInfo.Medicaments.Add(new Medicament
                    {
                        IdMedicament = medicamentRes.IdMedicament,
                        Name = medicamentRes.Name,
                        Description = medicamentRes.Description,
                        Type = medicamentRes.Type
                    });
                }
                prescriptionInfos.Add(prescriptionInfo);
            }
            patientInfo.prescriptions = prescriptionInfos;
            return patientInfo;
        }
    }
}
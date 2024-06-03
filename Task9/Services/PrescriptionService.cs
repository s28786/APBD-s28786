using System.Transactions;
using Task9.Context;
using Task9.Models;
using Task9.RequestTDOs;

namespace Task9.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly s28786DbContext _context;

        public PrescriptionService(s28786DbContext context)
        {
            _context = context;
        }

        /* The endpoint
should accept patient information, prescription information, and
information about the doctor listed on the prescription as part of
the request.
If the patient provided in the request does not exist, insert a new
patient into the Patient table.
If the medication listed on the prescription does not exist, return
an error.
A prescription can include a maximum of 10 medications.
Otherwise, return an error.
We must check if DueDate >= Date*/

        public async Task<object> AddPrescription(AddPrescriptionCommand addPrescriptionCommand)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                /*If the patient provided in the request does not exist, insert a new
                 patient into the Patient table.*/
                Patient patient = new Patient
                {
                    FirstName = addPrescriptionCommand.Patient.FirstName,
                    LastName = addPrescriptionCommand.Patient.LastName,
                    Birthdate = addPrescriptionCommand.Patient.Birthdate
                };
                if (_context.Patients.Find(addPrescriptionCommand.Patient.IdPatient) == null)
                {
                    _context.Patients.Add(patient);
                }
                else
                {
                    patient = _context.Patients.Find(addPrescriptionCommand.Patient.IdPatient);
                }
                /*If the medication listed on the prescription does not exist, return
                an error.*/
                foreach (var medicamentCom in addPrescriptionCommand.Medicaments)
                {
                    if (_context.Medicaments.Find(medicamentCom.IdMedicament) == null)
                    {
                        throw new Exception("Medicament does not exist");
                    }
                }
                /*A prescription can include a maximum of 10 medications. Otherwise, return an error.*/
                if (addPrescriptionCommand.Medicaments.Count > 10)
                {
                    throw new Exception("A prescription can include a maximum of 10 medications");
                }
                /*We must check if DueDate >= Date*/
                if (addPrescriptionCommand.DueDate < addPrescriptionCommand.Date)
                {
                    throw new Exception("DueDate must be greater or equal to Date");
                }

                /*check if doctor exist*/
                Doctor doctor = new Doctor
                {
                    FirstName = addPrescriptionCommand.Doctor.FirstName,
                    LastName = addPrescriptionCommand.Doctor.LastName,
                    Email = addPrescriptionCommand.Doctor.Email
                };
                if (_context.Doctors.Find(addPrescriptionCommand.Doctor.IdDoctor) == null)
                {
                    throw new Exception("Doctor does not exist");
                }
                else
                {
                    doctor = _context.Doctors.Find(addPrescriptionCommand.Doctor.IdDoctor);
                }
                Prescription prescription = new Prescription
                {
                    Date = addPrescriptionCommand.Date,
                    DueDate = addPrescriptionCommand.DueDate,
                    Doctor = doctor,
                    Patient = patient
                };
                //add prescription then return its id

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();

                foreach (var medicamentCom in addPrescriptionCommand.Medicaments)
                {
                    _context.Prescription_Medicaments.Add(new Prescription_Medicament
                    {
                        IdMedicament = medicamentCom.IdMedicament,
                        IdPrescription = prescription.IdPrescription,
                        Details = medicamentCom.Details,
                        Dose = medicamentCom.Dose,
                    });
                }

                await _context.SaveChangesAsync();
                transaction.Complete();
                return prescription;
            }
        }
    }
}
using Task9.RequestTDOs;

namespace Task9.Services
{
    public interface IPrescriptionService
    {
        public Task<object> AddPrescription(AddPrescriptionCommand addPrescriptionCommand);
    }
}
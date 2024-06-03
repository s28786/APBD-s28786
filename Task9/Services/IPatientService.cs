namespace Task9.Services
{
    public interface IPatientService
    {
        public Task<object> GetPatientInfo(int patientId);
    }
}
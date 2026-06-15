namespace StudentAPI.Services
{
    public interface IStudentServices
    {
        Task<byte[]> ExportStudentAsync();
    }
}

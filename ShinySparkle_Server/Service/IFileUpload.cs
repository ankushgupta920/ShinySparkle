using Microsoft.AspNetCore.Components.Forms;

namespace ShinySparkle_Server.Service
{
    public interface IFileUpload
    {
        Task<string> UploadFile(IBrowserFile file);
        bool DeleteFile(string FileName);
    }
}

using Microsoft.AspNetCore.Components.Forms;

namespace ShinySparkle_Server.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public bool DeleteFile(string FileName)
        {
            try
            {
                var pathh = $"{webHostEnvironment.WebRootPath}\\ProductImages\\{FileName}";
                if (File.Exists(pathh))
                {
                    File.Delete(pathh);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> UploadFile(IBrowserFile file)
        {
            try
            {
                FileInfo fileinfo = new FileInfo(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileinfo.Extension;
                var folderDirectory = $"{webHostEnvironment.WebRootPath}\\ProductImages";
                var path = Path.Combine(folderDirectory, fileName);

                var memoryStream = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(memoryStream);

                if (!Directory.Exists(folderDirectory))
                {
                    Directory.CreateDirectory(folderDirectory);
                }
                await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }
                var fullpath = $"ProductImages/{fileName}";
                return fullpath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using Microsoft.AspNetCore.Components.Forms;

namespace ShinySparkle_Server.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration _configuration;

        public FileUpload(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
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
        //public async Task<string> UploadFile(IBrowserFile file)
        //{
        //    try
        //    {
        //        FileInfo fileinfo = new FileInfo(file.Name);
        //        var fileName = Guid.NewGuid().ToString() + fileinfo.Extension;
        //        var folderDirectory = $"{webHostEnvironment.WebRootPath}\\ProductImages";
        //        var path = Path.Combine(folderDirectory, fileName);

        //        var memoryStream = new MemoryStream();
        //        await file.OpenReadStream().CopyToAsync(memoryStream);

        //        if (!Directory.Exists(folderDirectory))
        //        {
        //            Directory.CreateDirectory(folderDirectory);
        //        }
        //        await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        //        {
        //            memoryStream.WriteTo(fs);
        //        }
        //        var url = $"{_configuration.GetValue<string>("ServerUrl")}";
        //        var fullpath = $"{url}ProductImages/{fileName}";
        //        return fullpath;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<string> UploadFile(IBrowserFile file)
        {
            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                long maxFileSize = 5 * 1024 * 1024; // 5 MB

                var extension = Path.GetExtension(file.Name).ToLower();

                if (!allowedExtensions.Contains(extension) || !allowedMimeTypes.Contains(file.ContentType))
                {
                    throw new InvalidOperationException("File type is not supported.");
                }

                if (file.Size > maxFileSize)
                {
                    throw new InvalidOperationException("File size exceeds the 5 MB limit.");
                }

                FileInfo fileinfo = new FileInfo(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileinfo.Extension;
                var folderDirectory = $"{webHostEnvironment.WebRootPath}\\ProductImages";

                if (!Directory.Exists(folderDirectory))
                {
                    Directory.CreateDirectory(folderDirectory);
                }

                var path = Path.Combine(folderDirectory, fileName);

                using var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);

                await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(fs);
                }

                var url = $"{_configuration.GetValue<string>("ServerUrl")}";
                var fullpath = $"{url}ProductImages/{fileName}";
                return fullpath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace MasterData.Domain.Services
{
    public class FileUploadService
    {
        private readonly string _basePath;
        private readonly string _baseUrl;

        public FileUploadService(IConfiguration config, IWebHostEnvironment env)
        {
            _basePath = Path.Combine(env.ContentRootPath, "..", "MasterData.Web", "wwwroot", "files", "Pedidos");
            _baseUrl = config["FileUpload:BaseUrl"] ?? "https://localhost:6140/files";
        }
        public string EnsureMimePrefix(string base64)
        {
            if (base64.StartsWith("data:")) return base64;

            if (base64.StartsWith("JVBER")) 
                return $"data:application/pdf;base64,{base64}";
            if (base64.StartsWith("iVBOR")) 
                return $"data:image/png;base64,{base64}";
            if (base64.StartsWith("/9j/"))
                return $"data:image/jpeg;base64,{base64}";
            if (base64.StartsWith("UEsDB"))
                return $"data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,{base64}";

            return $"data:application/octet-stream;base64,{base64}";
        }
        public string GetExtension(string base64)
        {
            var match = Regex.Match(base64, @"^data:(?<mime>[\w\-\+\.\/]+);base64,");
            if (!match.Success) return "bin";

            var mime = match.Groups["mime"].Value.ToLower();
            return mime switch
            {
                "application/pdf" => "pdf",
                "image/png" => "png",
                "image/jpeg" => "jpg",
                "image/jpg" => "jpg",
                "text/plain" => "txt",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "docx",
                _ => "bin"
            };
        }
        public async Task SaveAsync(string base64, int orderId, string fileName)
        {
            var match = Regex.Match(base64, @"^data:(?<mime>[\w\-\+\.\/]+);base64,");
            var data = match.Success ? base64[(match.Length)..] : base64;
            var bytes = Convert.FromBase64String(data);
            var folder = Path.Combine(_basePath, orderId.ToString());

            Directory.CreateDirectory(folder);
            await File.WriteAllBytesAsync(Path.Combine(folder, fileName), bytes);
        }
    }
}

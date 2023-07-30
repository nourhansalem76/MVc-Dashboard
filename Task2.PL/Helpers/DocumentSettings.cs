using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Task2.PL.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile File, string FolderName)
        {
            //1. Get located folder path
            string FolderPath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);

            // 2.Get file name and make it unique
            string FileName = $"{Guid.NewGuid()}{File.FileName}";

            //3. Get file path
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. save file as stream [data per time]
           using var FS= new FileStream(FilePath,FileMode.Create);

            await File.CopyToAsync(FS);

            return FileName;

        }

        public static void DeleteFile(string FileName,string FolderName) 
        { 
            string FolderPath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName, FileName);
            if(File.Exists(FolderPath))
               File.Delete(FolderPath);

        }
    }
}

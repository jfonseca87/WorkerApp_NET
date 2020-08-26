using System.IO;
using System.Web;
using System.Web.Configuration;

namespace WorkerApp.Service.Utilities
{
    public static class File
    {
        private static readonly string relativePath = HttpContext.Current.Server.MapPath("~/WorkerFiles");

        public static string SaveFile(string id, HttpPostedFileBase file)
        {
            if (!Directory.Exists(relativePath)) 
            {
                Directory.CreateDirectory(relativePath);
            }

            string completePath = Path.Combine(relativePath, id);

            if (!Directory.Exists(completePath))
            {
                Directory.CreateDirectory(completePath);
            }

            string filePath = Path.Combine(completePath, file.FileName);

            if (!Directory.Exists(filePath))
            {
                file.SaveAs(filePath);
            }

            return $"{WebConfigurationManager.AppSettings["ServerUrl"]}WorkerFiles/{id}/{file.FileName}";
        }

        public static void DeleteFile(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }
    }
}

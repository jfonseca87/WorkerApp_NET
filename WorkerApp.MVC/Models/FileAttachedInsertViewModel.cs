using System.Web;

namespace WorkerApp.MVC.Models
{
    public class FileAttachedInsertViewModel
    {
        public string FileType { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string PersonId { get; set; }
    }
}
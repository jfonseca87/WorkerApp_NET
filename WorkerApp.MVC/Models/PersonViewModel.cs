using System.Collections.Generic;

namespace WorkerApp.MVC.Models
{
    public class PersonViewModel
    {
        public string PersonId { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Profession { get; set; }
        public string Photo { get; set; }
        public string UserId { get; set; }
        public ICollection<FileAttachedViewModel> FilesAttached { get; set; }
    }
}
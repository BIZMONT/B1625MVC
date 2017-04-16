using System.ComponentModel.DataAnnotations;

namespace B1625MVC.Web.Areas.Admin.Models
{
    public class SmtpSettingsViewModel
    {
        [Required]
        public bool Enabled { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public int? Port { get; set; }

        [Required]
        public bool Ssl { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
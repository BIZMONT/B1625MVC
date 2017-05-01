using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B1625MVC.Web.Areas.Admin.Models
{
    public class EditPublicationViewModel
    {
        public long Id { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
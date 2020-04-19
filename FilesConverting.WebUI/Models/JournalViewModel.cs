using FilesConverting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FilesConverting.WebUI.Models
{
    public class JournalViewModel
    {
        [ScaffoldColumn(false)]
        public long id { get; set; }

        [DisplayName("Дата и время")]
        [Required]
        public DateTime upload { get; set; }

        [DisplayName("Наименование файла")]
        [StringLength(150)]
        [Required]
        public string filename { get; set; }
        
        
    }
}
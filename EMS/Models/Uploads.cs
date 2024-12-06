using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Uploads
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UPID { get; set; }

        public int EmpID { get; set; }

        public string? ImagePath { get; set; }

        public string? AadharPath { get; set; }

        public string? PanPath { get; set; }

        public string? PassbookPath { get; set; }
    }
}

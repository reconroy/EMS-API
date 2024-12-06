using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class LnABalance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LnABID { get; set; }

        public int EmpID { get; set; }

        [MaxLength(7)]
        public string MonthYear { get; set; }

        public double OpeningBalance { get; set; }

        public double TotalDeduction { get; set; }

        public double ClosingBalance { get; set; } 
    }
}

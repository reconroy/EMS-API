using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class LoansandAdvance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LnAID { get; set; }

        public int EmpID { get; set; }

        public DateOnly LnADate { get; set; }

        public double AmountGiven { get; set; }

        public double DeductionAmount { get; set; }

        public DateOnly DeductionStartDate { get; set; }

        public int DeductionFrequencyID { get; set; }

        public bool isActive { get; set; } = true;

        public string MonthYear { get; set; }

        public bool IsDeductionComplete { get; set; } = false;
    }
}

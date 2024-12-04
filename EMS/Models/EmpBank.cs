using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace EMS.Models
{
    public class EmpBank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpBankID { get; set; }

        public int EmpID { get; set; }

        public string BankName { get; set; }

        public string IFSCCode { get; set; }

        public string AccountNumber { get; set; }
    }
}

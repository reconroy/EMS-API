using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    public class Payroll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrID { get; set; }

        public int EmpID { get; set; }

        public string PayrollType { get; set; }

        public double BasicSalary { get; set; }

        public double Increament { get; set; } = 0;

        public DateOnly? IncreamentDate { get; set; }

        public double EPF {  get; set; }

        public double ESI { get; set; }

        public double RD { get; set; }

        public double HI { get; set; }

        public string MonthYear { get; set; }
    }
}

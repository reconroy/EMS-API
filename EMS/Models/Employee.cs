using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpID { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string FatherName { get; set; }

        public string MotherName { get; set; }

        public string MaritalStatus { get; set; }

        public string Qualification { get; set; }

        public string? Email { get; set; }

        public string Mobile1 { get; set; }

        public string? Mobile2 { get; set; }

        public string PAddress { get; set; }

        public string PPinCode { get; set; }

        public string PDistrict { get; set; }

        public string CAddress { get; set; }

        public string CPinCode { get; set; }

        public string CDistrict { get; set; }

        public DateOnly DOB { get; set; }

        public DateOnly DOJ { get; set; }

        public string Gender { get; set; }

        public int DepartmentID { get; set; }

        public int RoleID { get; set; }

        public int Designation { get; set; }

        public string AadhaarNumber { get; set; }

        public string PanNumber { get; set; }

        public bool isActive { get; set; } = true;

        public int WorkingLocation { get; set; }
    }
}

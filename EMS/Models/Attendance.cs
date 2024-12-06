using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceID { get; set; }

        public int EmpID { get; set; }
        
        public DateOnly Date {  get; set; }
        
        public string Status { get; set; }
        
        public TimeOnly CheckInTime { get; set; }
        
        public TimeOnly CheckOutTime { get; set; }

        public TimeSpan TotalDuration { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    public class Banks
    {
        [Key]
        public int BankId { get; set; }

        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

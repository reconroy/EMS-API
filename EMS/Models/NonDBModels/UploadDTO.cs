namespace EMS.Models.NonDBModels
{
    public class UploadDTO
    {
        public int EmpID { get; set; }

        public IFormFile? Image {  get; set; }

        public IFormFile? Aadhar { get; set; }

        public IFormFile? Pan {  get; set; }

        public IFormFile? Passbook { get; set; }
    }
}

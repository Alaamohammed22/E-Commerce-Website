namespace WebApplication1.ViewModel
{
    public class SupplierVM
    {
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string SSN { get; set; }
        public string? SSNImageName { get; set; }
        public IFormFile SSNImageFile { get; set; }

        public string VerifecationState { get; set; }
        public double TotalSales { get; set; }
        public string AccountNumber { get; set; }
    }
}

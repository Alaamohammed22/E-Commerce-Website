using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel
{
    public class LoginVM
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

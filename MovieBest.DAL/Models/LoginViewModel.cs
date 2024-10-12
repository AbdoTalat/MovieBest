using System.ComponentModel.DataAnnotations;

namespace MovieBest.DAL.Models
{
    public class LoginViewModel
    {
		[Required(ErrorMessage = "User Name Is Required.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password Is Required.")]
		[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

namespace MovieBest.DAL.Models
{
    public class UserViewModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
		public string Email { get; set; }
        public List<string> UserRoles { get; set; }
    }
}

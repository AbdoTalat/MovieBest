namespace MovieBest.DAL.Models
{
    public class ProfileViewModel
    {
        public string userName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> userRoles { get; set; }
    }
}

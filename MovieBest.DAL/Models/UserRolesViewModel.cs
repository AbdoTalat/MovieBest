namespace MovieBest.DAL.Models
{
    public class UserRolesViewModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public List<string?> AvailableRoles { get; set; } = new List<string>();
        public IList<string> SelectedRoles { get; set; }
    }
}

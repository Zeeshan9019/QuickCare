
using Common_QuickCare.ViewModel;

namespace QuickCare.ViewModel
{
    public class UserViewModel
    {
        public UserVM? User { get; set; }
        public List<UserVM>? Users { get; set; }
    }
    public class UserVM : BaseOrgViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password{ get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public int RoleId { get; set; }
    }
}

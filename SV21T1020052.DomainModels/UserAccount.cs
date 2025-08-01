namespace SV21T1020052.DomainModels
{
    public class UserAccount
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public string RoleNames { get; set; } = "";

        // Các thuộc tính thêm cho đăng ký
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string Password { get; set; } = "";  // Chứa mật khẩu của người dùng
    }
}

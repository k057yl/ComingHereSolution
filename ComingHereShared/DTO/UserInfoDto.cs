namespace ComingHereShared.DTO
{
    public class UserInfoDto
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsOrganizer { get; set; }
    }
}

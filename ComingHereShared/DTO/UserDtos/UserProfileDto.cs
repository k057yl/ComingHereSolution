using ComingHereShared.DTO.CommentDtos;

namespace ComingHereShared.DTO.UserDtos
{
    public class UserProfileDto
    {
        public string UserName { get; set; }
        public int Reputation { get; set; }
        public string Rank { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}

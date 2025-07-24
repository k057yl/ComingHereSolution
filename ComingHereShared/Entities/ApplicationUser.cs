using Microsoft.AspNetCore.Identity;

namespace ComingHereShared.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int ReputationPoints { get; set; } = 0;
    }
}

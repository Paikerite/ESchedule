using Microsoft.AspNetCore.Identity;

namespace ESchedule.Data
{
    public class ApplicationRole : IdentityRole<int>
    {
        // Дополнительные свойства для роли
        public string? Description { get; set; }
        public string? RoleName { get; set; }
    }
}

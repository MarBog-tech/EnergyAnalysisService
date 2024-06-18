using Microsoft.AspNetCore.Identity;

namespace EnergyAnalysisService.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

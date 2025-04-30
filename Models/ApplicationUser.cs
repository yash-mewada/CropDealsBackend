using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CropDeals.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public UserStatus Status { get; set; }

        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }

        public Guid? BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; }

        public float? AverageRating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum UserRole
    {
        Farmer,
        Dealer,
        Admin
    }

    public enum UserStatus
    {
        Active,
        Inactive
    }
}

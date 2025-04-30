using System.ComponentModel.DataAnnotations;

namespace CropDeals.Models
{
    public class BankAccount
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string IFSCCode { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        public string BranchName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

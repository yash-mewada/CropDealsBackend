using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CropDeals.Models
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Content { get; set; }

        [Required]
        public Guid GeneratedBy { get; set; }

        [ForeignKey(nameof(GeneratedBy))]
        public ApplicationUser ByUser { get; set; }

        [Required]
        public Guid GeneratedFor { get; set; }

        [ForeignKey(nameof(GeneratedFor))]
        public ApplicationUser ForUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

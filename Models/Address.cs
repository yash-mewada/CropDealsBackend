using System.ComponentModel.DataAnnotations;

namespace CropDeals.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }
    }
}

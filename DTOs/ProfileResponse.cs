namespace CropDeals.Models.DTOs
{
    public class ProfileResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }

        // Address
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        // Bank Account
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }

        // Only for Farmer
        public float? AverageRating { get; set; }
    }
}

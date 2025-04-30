namespace CropDeals.DTOs
{
    public class UpdateProfileRequest
    {
        public string PhoneNumber { get; set; }

        // Address
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // BankAccount
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
    }
}

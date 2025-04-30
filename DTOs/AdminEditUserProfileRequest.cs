using CropDeals.Models;

public class AdminEditUserProfileRequest
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public UserStatus Status { get; set; }

    // Address
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    // Bank Account
    public string AccountNumber { get; set; }
    public string IFSCCode { get; set; }
    public string BankName { get; set; }
    public string BranchName { get; set; }
}

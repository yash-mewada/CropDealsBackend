using Microsoft.AspNetCore.Identity;
using CropDeals.Models;
using CropDeals.Models.DTOs;
using CropDeals.Repositories.Interfaces;
using AutoMapper;
using CropDeals.DTOs;
using CropDeals.Data;
using Microsoft.EntityFrameworkCore;

namespace CropDeals.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AccountRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<string> RegisterAsync(SignUpRequest request)
        {
            if (request.Role.ToLower() == "admin")
                return "You are not allowed to register as an Admin.";

            var allowedRoles = new[] { "Farmer", "Dealer" };
            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
                return "Invalid role. Only 'Farmer' or 'Dealer' roles are allowed.";

            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                return "User already exists";

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));

            if (!await _roleManager.RoleExistsAsync(request.Role))
                return $"Role '{request.Role}' does not exist in the system.";

            await _userManager.AddToRoleAsync(user, request.Role);
            return "User created successfully!";
        }

        public async Task<string> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.BankAccount)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
                return "User not found.";

            // Update phone number
            user.PhoneNumber = request.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            // Update or create Address
            if (user.Address == null)
            {
                var newAddress = new Address
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    Street = request.Street,
                    City = request.City,
                    State = request.State,
                    ZipCode = request.ZipCode
                };
                _context.Addresses.Add(newAddress);
                user.Address = newAddress;
                user.AddressId = newAddress.Id;
            }
            else
            {
                user.Address.Street = request.Street;
                user.Address.City = request.City;
                user.Address.State = request.State;
                user.Address.ZipCode = request.ZipCode;
                _context.Addresses.Update(user.Address);
            }

            // Update or create BankAccount
            if (user.BankAccount == null)
            {
                var newBank = new BankAccount
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    AccountNumber = request.AccountNumber,
                    IFSCCode = request.IFSCCode,
                    BankName = request.BankName,
                    BranchName = request.BranchName,
                    UserId = user.Id
                };
                _context.BankAccounts.Add(newBank);
                user.BankAccount = newBank;
                user.BankAccountId = newBank.Id;
            }
            else
            {
                user.BankAccount.AccountNumber = request.AccountNumber;
                user.BankAccount.IFSCCode = request.IFSCCode;
                user.BankAccount.BankName = request.BankName;
                user.BankAccount.BranchName = request.BranchName;
                _context.BankAccounts.Update(user.BankAccount);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return "Profile updated successfully.";
        }

        public async Task<string> AdminEditUserAsync(string targetUserId, AdminEditUserProfileRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.BankAccount)
                .FirstOrDefaultAsync(u => u.Id.ToString() == targetUserId);

            if (user == null)
                return "User not found.";

            if (user.Role == UserRole.Admin)
                return "You cannot edit an Admin profile.";

            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            user.Status = request.Status;
            user.UpdatedAt = DateTime.UtcNow;

            // Address
            if (user.Address == null)
            {
                var address = new Address
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    Street = request.Street,
                    City = request.City,
                    State = request.State,
                    ZipCode = request.ZipCode
                };
                _context.Addresses.Add(address);
                user.Address = address;
                user.AddressId = address.Id;
            }
            else
            {
                user.Address.Street = request.Street;
                user.Address.City = request.City;
                user.Address.State = request.State;
                user.Address.ZipCode = request.ZipCode;
                _context.Addresses.Update(user.Address);
            }

            // BankAccount
            if (user.BankAccount == null)
            {
                var bank = new BankAccount
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    AccountNumber = request.AccountNumber,
                    IFSCCode = request.IFSCCode,
                    BankName = request.BankName,
                    BranchName = request.BranchName,
                    UserId = user.Id
                };
                _context.BankAccounts.Add(bank);
                user.BankAccount = bank;
                user.BankAccountId = bank.Id;
            }
            else
            {
                user.BankAccount.AccountNumber = request.AccountNumber;
                user.BankAccount.IFSCCode = request.IFSCCode;
                user.BankAccount.BankName = request.BankName;
                user.BankAccount.BranchName = request.BranchName;
                _context.BankAccounts.Update(user.BankAccount);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return "User profile updated by Admin successfully.";
        }

        public async Task<string> AdminDeleteUserAsync(string targetUserId)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.BankAccount)
                .FirstOrDefaultAsync(u => u.Id.ToString() == targetUserId);

            if (user == null)
                return "User not found.";

            if (user.Role == UserRole.Admin)
                return "Cannot delete an Admin user.";

            // Delete related Address and BankAccount if they exist
            if (user.Address != null)
                _context.Addresses.Remove(user.Address);

            if (user.BankAccount != null)
                _context.BankAccounts.Remove(user.BankAccount);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return "User deleted successfully.";
        }

    }
}

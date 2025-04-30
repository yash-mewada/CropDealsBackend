using Microsoft.AspNetCore.Mvc;
using CropDeals.Models.DTOs;
using CropDeals.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CropDeals.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using CropDeals.Data;
using CropDeals.Models;
using Microsoft.EntityFrameworkCore;

namespace CropDeals.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(IAccountRepository accountRepo, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUpRequest request)
        {
            if (request.Role != "Farmer" && request.Role != "Dealer")
            {
                return BadRequest("Only Farmers and Dealers can register.");
            }

            var response = await _accountRepo.RegisterAsync(request);

            if (response != "User created successfully!")
                return BadRequest(response);

            return Ok(response);
        }


        [Authorize(Roles = "Farmer,Dealer")]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token or user ID not found in token.");

            var result = await _accountRepo.UpdateProfileAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("whoami")]
        [Authorize]
        public IActionResult WhoAmI()
        {
            var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
            return Ok(claims);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.BankAccount)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
                return NotFound("User not found");

            var response = new ProfileResponse
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                Status = user.Status.ToString(),

                Street = user.Address?.Street,
                City = user.Address?.City,
                State = user.Address?.State,
                ZipCode = user.Address?.ZipCode,

                AccountNumber = user.BankAccount?.AccountNumber,
                IFSCCode = user.BankAccount?.IFSCCode,
                BankName = user.BankAccount?.BankName,
                BranchName = user.BankAccount?.BranchName,
            };

            if (user.Role == UserRole.Farmer)
            {
                response.AverageRating = user.AverageRating;
            }

            return Ok(response);
        }


    }
}

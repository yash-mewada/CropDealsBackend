using Microsoft.AspNetCore.Mvc;
using CropDeals.Models.DTOs;
using CropDeals.Repositories.Interfaces;

namespace CropDeals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var token = await _authRepo.SignInAsync(model);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }
    }
}

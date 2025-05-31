using AutoMapper;
using CropDeals.Models;
using CropDeals.Models.DTOs;
using CropDeals.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CropDeals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Dealer")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _repository;
        private readonly IMapper _mapper;

        public SubscriptionController(ISubscriptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionCreateDto dto)
        {
            var dealerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var subscription = _mapper.Map<Subscription>(dto);

            var result = await _repository.AddSubscriptionAsync(dealerId, subscription);
            return Ok(_mapper.Map<SubscriptionReadDto>(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetMySubscriptions()
        {
            var dealerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _repository.GetDealerSubscriptionsAsync(dealerId);
            return Ok(_mapper.Map<IEnumerable<SubscriptionReadDto>>(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unsubscribe(Guid id)
        {
            var dealerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _repository.UnsubscribeAsync(id, dealerId);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var dealerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var notifications = await _repository.GetNotificationsAsync(dealerId);
            return Ok(notifications);
        }
    }
}

using ApplicationCore.Services;
using AutoMapper;
using Domain.DTOs;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServices _notifServices;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public NotificationController(INotificationServices notifServices, IUnitOfWork uof, IMapper mapper)
        {
            _notifServices = notifServices; _uof = uof; _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var notifs = await _notifServices.GetAllAsync();
                return Ok(new ApiResponse<IReadOnlyList<NotificationDTO>>("Récupération réussie", _mapper.Map<IReadOnlyList<NotificationDTO>>(notifs)));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("GetUnread")]
        public async Task<IActionResult> GetUnread()
        {
            try
            {
                var notifs = await _notifServices.GetUnreadAsync();
                return Ok(new ApiResponse<IReadOnlyList<NotificationDTO>>("Récupération réussie", _mapper.Map<IReadOnlyList<NotificationDTO>>(notifs)));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPatch("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                _uof.BeginTransaction();
                await _notifServices.MarkAsReadAsync(id);
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("Notification marquée comme lue", true));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }

        [HttpPatch("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                _uof.BeginTransaction();
                await _notifServices.MarkAllAsReadAsync();
                _uof.CommitTransaction();
                return Ok(new ApiResponse<bool>("Toutes les notifications marquées comme lues", true));
            }
            catch (Exception ex) { _uof.RollbackTransaction(); return BadRequest(ex.Message); }
        }
    }
}

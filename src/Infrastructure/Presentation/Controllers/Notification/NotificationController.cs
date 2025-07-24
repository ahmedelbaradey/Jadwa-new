using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Funds.Dtos;
using Application.Features.Notifications.Dtos;
using Application.Features.Notifications.Queries.List;
using Application.Features.Notifications.Commands.MarkAsRead;
using Application.Features.Notifications.Commands.MarkAllAsRead;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : AppControllerBase
    {
        [HttpGet("NotitficationList")]
        [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new ListQuery { });
            return NewResult(response);
        }

        [HttpGet("UnReadedNotificationList")]
        [ProducesResponseType(typeof(PaginatedResult<NotificationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadedList([FromQuery] ListUnreadedQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Mark a single notification as read
        /// </summary>
        /// <param name="notificationId">ID of the notification to mark as read</param>
        /// <returns>Success response</returns>
        [HttpPut("MarkAsRead/{notificationId}")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]    
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var command = new MarkNotificationAsReadCommand { NotificationId = notificationId };
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Mark all notifications as read for the current user
        /// </summary>
        /// <returns>Success response</returns>
        [HttpPut("MarkAllAsRead")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            var command = new MarkAllNotificationsAsReadCommand();
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}

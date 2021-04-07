using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelegramBot.Services;

namespace TelegramBot.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsNotificationsController : ControllerBase
    {
        private readonly ILogger<NewsNotificationsController> _logger;

        private readonly IReceivedNewsNotificationsService _receivedNewsNotificationsService;

        public NewsNotificationsController( ILogger<NewsNotificationsController> logger,
                                            IReceivedNewsNotificationsService receivedNewsNotificationsService)
        {
            _logger = logger;
            _receivedNewsNotificationsService = receivedNewsNotificationsService;
        }


        [HttpPost]
        public ActionResult<string> Post([FromBody] NewsNotification newsNotification)
        {
            _logger.LogInformation($"News Notifications received: {{id: {newsNotification.Id}, title: {newsNotification.Title} }}");

            _receivedNewsNotificationsService.AddNewsNotification(newsNotification);

            return Ok("The message was received.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace KE_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailRetentionController : ControllerBase
    {
        private readonly ILogger<EmailRetentionController> _logger;

        public EmailRetentionController(ILogger<EmailRetentionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<EmailRetention> Get()
        {
           
            return Enumerable.Range(1, 5).Select(index => new EmailRetention
            {
                Date = DateTime.Now.AddDays(index),
              
            })
            .ToArray();

        }

        [HttpGet("Test")]
        public string Test()
        {
            return "Hello";
        }
    }
}
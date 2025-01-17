using CrossCuttingLayer;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Microservice.TodoApp.Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IBus _bus;

        public TodoController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Todo todoModel)
        {
            if (todoModel is not null)
            {
                Uri uri = new Uri(RabbitMqConsts.RabbitMqUri);
                var endPoint = await _bus.GetSendEndpoint(uri);
                /* GetSendEndpoint is a method that helps you obtain a reference to 
                   the send endpoint for sending messages to a specific queue or exchange.

                   var sendEndpoint = await bus.GetSendEndpoint(new Uri("queue:your-queue-name"));
                   await sendEndpoint.Send(new YourMessage { Property = "Value" });
                   
                   bus.GetSendEndpoint() - gets the send endpoint for a given URI 
                   Send() - sends a message to that endpoint.
                */
                await endPoint.Send(todoModel);
                return Ok();
            }
            return BadRequest();
        }
    }
}
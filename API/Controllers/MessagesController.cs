using System;
using System.Threading.Tasks;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user/{userName}/[controller]")]
    public class MessagesController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> Create(String userName, Create.Command command)
        {
            command.SenderUserName = userName;
            return await Mediator.Send(command);
        }
    }
}
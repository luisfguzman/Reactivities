using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Messages;
using MediatR;
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

        [HttpGet]
        public async Task<ActionResult<List.MessagesEnvelope>> List(string userName, int? limit, int? offset, string predicate)
        {
            return await Mediator.Send(new List.Query(userName, limit, offset, predicate));
        }

        [HttpGet("convo/{recipientUserName}")]
        public async Task<ActionResult<List<MessageDto>>> ListThread(string userName, string recipientUserName)
        {
            return await Mediator.Send(new ListThread.Query {UserName = userName, RecipientUserName = recipientUserName});
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> Create(String userName, Create.Command command)
        {
            command.SenderUserName = userName;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}/delete")]
        public async Task<ActionResult<Unit>> Delete(string userName, Guid id)
        {
            return await Mediator.Send(new Delete.Command { UserName = userName, Id = id });
        }

        [HttpPost("{id}/read")]
        public async Task<ActionResult<Unit>> Read(string userName, Guid id)
        {
            return await Mediator.Send(new Read.Command { UserName = userName, Id = id });
        }
    }
}
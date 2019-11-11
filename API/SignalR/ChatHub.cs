using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Comments;
using Application.Likes;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command)
        {
            var userName = Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            command.UserName = userName;

            var comment = await _mediator.Send(command);

            await Clients.All.SendAsync("ReceiveComment", comment);
        }

        public async Task SendLike(Process.Command command)
        {
            var userName = Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            command.UserName = userName;

            var like = await _mediator.Send(command);

            await Clients.All.SendAsync("ReceiveLike", like);
        }
    }
}
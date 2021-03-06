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
            var userName = GetUserName();

            command.UserName = userName;

            var comment = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }

        public async Task SendLike(Process.Command command)
        {
            var userName = GetUserName();

            command.UserName = userName;

            var like = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString()).SendAsync("ReceiveLike", like);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            var userName = GetUserName();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{userName} has left the group");
        }

        public async Task AddToGroup(string groupName)
        {
            var userName = GetUserName();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{userName} has joined the group");
        }
        private string GetUserName()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
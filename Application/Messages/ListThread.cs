using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Messages
{
    public class ListThread
    {
        public class Query : IRequest<List<MessageDto>>
        {
            public string UserName { get; set; }
            public string RecipientUserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<MessageDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<MessageDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.UserName != _userAccessor.GetCurrentUsername())
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorized to retrieve these messages" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Sender User not found" });

                var recipient = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.RecipientUserName);

                if (recipient == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Recipient User not found" });

                var messages = await _context.Messages
                    .Where(m => m.RecipientId == user.Id && m.RecipientDeleted == false
                        && m.SenderId == recipient.Id
                        || m.RecipientId == recipient.Id && m.SenderId == user.Id
                        && m.SenderDeleted == false)
                    .OrderByDescending(m => m.MessageSent)
                    .ToListAsync();

                return _mapper.Map<List<Message>, List<MessageDto>>(messages);
            }
        }
    }
}
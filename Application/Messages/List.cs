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
    public class List
    {
        public class MessagesEnvelope
        {
            public List<MessageDto> Messages { get; set; }
            public int MessageCount { get; set; }
        }

        public class Query : IRequest<MessagesEnvelope>
        {
            public Query(string userName, int? limit, int? offset, string predicate)
            {
                UserName = userName;
                Limit = limit;
                Offset = offset;
                Predicate = predicate;
            }
            public string UserName { get; set; }
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public string Predicate { get; set; }
        }
        public class Handler : IRequestHandler<Query, MessagesEnvelope>
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

            public async Task<MessagesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.UserName != _userAccessor.GetCurrentUsername())
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorized to retrieve these messages" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

                var queryable = _context.Messages.AsQueryable();

                switch (request.Predicate)
                {
                    case "Inbox":
                        queryable = queryable.Where(u => u.RecipientId == user.Id
                            && u.RecipientDeleted == false);
                        break;
                    case "Outbox":
                        queryable = queryable.Where(u => u.SenderId == user.Id
                            && u.SenderDeleted == false);
                        break;
                    default: //Unread Messages
                        queryable = queryable.Where(u => u.RecipientId == user.Id
                            && u.RecipientDeleted == false && u.IsRead == false);
                        break;
                }

                queryable = queryable.OrderByDescending(d => d.MessageSent);

                var messages = await queryable
                    .Skip(request.Offset ?? 0)
                    .Take(request.Limit ?? 10)
                    .ToListAsync();

                return new MessagesEnvelope
                {
                    Messages = _mapper.Map<List<Message>, List<MessageDto>>(messages),
                    MessageCount = queryable.Count()
                };
            }
        }
    }
}
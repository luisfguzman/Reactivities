using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Messages
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

                var message = await _context.Messages.FindAsync(request.Id);

                if (message == null)
                    throw new RestException(HttpStatusCode.NotFound, new { message = "Not found." });

                if (message.RecipientId != user.Id)
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = "Not authorized to read this message" });

                if (message.SenderId == user.Id)
                    message.SenderDeleted = true;

                if (message.RecipientId == user.Id)
                    message.RecipientDeleted = true;

                if (message.SenderDeleted && message.RecipientDeleted)
                    _context.Messages.Remove(message);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Error deleting the message");
            }
        }
    }
}
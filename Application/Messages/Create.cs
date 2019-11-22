using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Messages
{
    public class Create
    {
        public class Command : IRequest<MessageDto>
        {
            public string SenderUserName { get; set; }
            public string RecipientUserName { get; set; }
            public string Content { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.SenderUserName).NotEmpty();
                RuleFor(x => x.RecipientUserName).NotEmpty();
                RuleFor(x => x.Content).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, MessageDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<MessageDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var sender = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.SenderUserName);

                if (sender == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Sender User not found" });

                var recipient = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.RecipientUserName);

                if (recipient == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Recipient User not found" });

                var message = new Message
                {
                    Sender = sender,
                    Recipient = recipient,
                    Content = request.Content,
                    IsRead = false,
                    MessageSent = DateTime.Now,
                    SenderDeleted = false,
                    RecipientDeleted = false
                };

                _context.Messages.Add(message);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<MessageDto>(message);

                throw new Exception("Creating the message failed on save");
            }
        }
    }
}
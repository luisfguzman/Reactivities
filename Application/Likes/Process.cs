using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Likes
{
    public class Process
    {
        public class Command : IRequest<LikeDto>
        {
            public Guid ActivityId { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, LikeDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<LikeDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.ActivityId);

                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var like = await _context.Likes.FirstOrDefaultAsync(x => x.Activity.Id == activity.Id && x.Author.Id == user.Id);

                if (like == null)
                {
                    like = new Like
                    {
                        Author = user,
                        Activity = activity,
                        Status = "Liked"
                    };
                    _context.Likes.Add(like);
                } else {
                    like.Status = "Unliked";
                    _context.Likes.Remove(like);
                }

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<LikeDto>(like);

                throw new Exception("Problem saving changes");
            }
        }
    }
}
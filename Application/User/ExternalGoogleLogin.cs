using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class ExternalGoogleLogin
    {
        public class Query : IRequest<User>
        {
            public string IdToken { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly ISocialAccessor _socialAccessor;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(UserManager<AppUser> userManager, ISocialAccessor socialAccessor, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _socialAccessor = socialAccessor;
                _userManager = userManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var userInfo = await _socialAccessor.GoogleLogin(request.IdToken);

                if (userInfo == null)
                    throw new RestException(HttpStatusCode.BadRequest, new {User = "Problem validating token"});

                var user = await _userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    user = new AppUser
                    {
                        DisplayName = userInfo.Name,
                        Id = userInfo.Sub,
                        Email = userInfo.Email,
                        UserName = "google_" + userInfo.Sub
                    };

                    var photo = new Photo
                    {
                        Id = "google_" + userInfo.Sub,
                        Url = userInfo.Picture,
                        IsMain = true
                    };

                    user.Photos.Add(photo);

                    var result = await _userManager.CreateAsync(user);

                    if (!result.Succeeded)
                        throw new RestException(HttpStatusCode.BadRequest, new {User = "Problem creating user"});
                }

                return new User
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    Username = user.UserName,
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }
        }
    }
}
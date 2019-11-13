using System;

namespace Application.Likes
{
    public class LikeDto
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
    }
}
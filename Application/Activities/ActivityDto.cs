using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.Comments;
using Application.Likes;

namespace Application.Activities
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Price { get; set; }
        [JsonPropertyName("attendees")]
        public ICollection<AttendeeDto> UserActivities { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<LikeDto> Likes { get; set; }
    }
}
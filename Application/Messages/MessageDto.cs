using System;

namespace Application.Messages
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public string SenderUserName { get; set; }
        public string SenderDisplayName { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        public string RecipientDisplayName { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
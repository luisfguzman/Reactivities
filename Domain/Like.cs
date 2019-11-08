using System;

namespace Domain
{
    public class Like
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public virtual AppUser Author { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
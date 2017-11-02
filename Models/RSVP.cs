using System;

namespace wedding.Models{
    public class RSVP : BaseEntity
    {
        public int id { get; set; }
        public int weddingid { get; set; }
        public Wedding wedding { get; set; }
        public int userID { get; set; }
        public User attending { get; set; }
    }
}
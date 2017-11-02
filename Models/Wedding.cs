using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class Wedding : BaseEntity
    {
        public int ID { get; set; }
        public string bride { get; set; }
        public string groom { get; set; }
        public DateTime date { get; set; }
        public string address { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User planner { get; set; }
        public int userID { get; set; }
        public List<RSVP> attendees { get; set; }
        public Wedding()
        {
            attendees = new List<RSVP>();
        }
    }
}
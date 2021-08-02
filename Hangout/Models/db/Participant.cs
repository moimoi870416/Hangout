using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Participant
    {
        public Participant(int eventId, int participanter, string motivation)
        {
            EventId = eventId;
            Participanter = participanter;
            Motivation = motivation;
        }

        public int ParticipantId { get; set; }
        public int EventId { get; set; }
        public int Participanter { get; set; }
        public string Motivation { get; set; }
        public byte Status { get; set; }
    }
}

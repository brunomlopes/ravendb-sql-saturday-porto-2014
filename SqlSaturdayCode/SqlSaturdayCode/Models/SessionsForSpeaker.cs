using System;
using System.Linq;
using Raven.Client.Indexes;

namespace SqlSaturdayCode.Models
{
    public class SessionsForSpeaker : AbstractIndexCreationTask<Event, SessionsForSpeaker.Result>
    {
        public class Result
        {
            public string SpeakerId { get; set; }
            public DateTime EventDate { get; set; }
        }
        public SessionsForSpeaker()
        {
            Map = events => from evt in events
                from session in evt.Sessions
                select new
                {
                    session.SpeakerId,
                    EventDate = evt.Date
                };
        }
    }
}
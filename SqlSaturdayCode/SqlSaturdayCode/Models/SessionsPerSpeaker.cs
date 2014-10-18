using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace SqlSaturdayCode.Models
{
    public class SessionsPerSpeaker :
        AbstractMultiMapIndexCreationTask<SessionsPerSpeaker.Result>
    {
        public SessionsPerSpeaker()
        {
            AddMap<Speaker>(speakers => from speaker in speakers
                select new
                {
                    SpeakerId = speaker.Id,
                    SpeakerName = speaker.Name,
                    NumberOfSessions = 0
                });
            AddMap<Event>(events => from evt in events
                from session in evt.Sessions
                select new
                {
                    SpeakerId = session.SpeakerId,
                    SpeakerName = (string) null,
                    NumberOfSessions = 1
                });

            Reduce = results => from result in results
                group result by result.SpeakerId
                into g
                select new
                {
                    SpeakerId = g.Key,
                    SpeakerName = g.Where(l => l.SpeakerName != null)
                        .Select(l => l.SpeakerName)
                        .FirstOrDefault(),
                    NumberOfSessions = g.Sum(l => l.NumberOfSessions)
                };
        }

        public class Result
        {
            public string SpeakerId { get; set; }
            public string SpeakerName { get; set; }
            public int NumberOfSessions { get; set; }
        }
    }
}
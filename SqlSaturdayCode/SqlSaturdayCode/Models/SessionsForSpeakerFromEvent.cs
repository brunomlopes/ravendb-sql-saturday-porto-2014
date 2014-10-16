using System.Linq;
using Raven.Client.Indexes;

namespace SqlSaturdayCode.Models
{
    public class SessionsForSpeakerFromEvent : AbstractTransformerCreationTask<Event>
    {
        public SessionsForSpeakerFromEvent()
        {
            TransformResults = events => from evt in events
                from session in evt.Sessions
                where session.SpeakerId == Parameter("speakerId").Value<string>()
                select session;
        }
    }
}
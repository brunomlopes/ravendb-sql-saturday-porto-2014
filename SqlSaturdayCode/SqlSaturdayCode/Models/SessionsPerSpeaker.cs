using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace SqlSaturdayCode.Models
{
    public class SessionsPerSpeaker : AbstractIndexCreationTask
    {
        public class Result
        {
            public string SpeakerId { get; set; }
            public string SpeakerName { get; set; }
            public int NumberOfSessions { get; set; }
        }
        public override string IndexName
        {
            get
            {
                return "SessionsPerSpeaker";
            }
        }
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition
            {
                Maps =  {
                    @"from speaker in docs.Speakers
select new {
	SpeakerId = speaker.Id,
	SpeakerName = speaker.Name,
	NumberOfSessions = 0
}",
                    @"from speaker in docs.Speakers
select new {
	SpeakerId = speaker.Id,
	SpeakerName = speaker.Name,
	NumberOfSessions = 0
}"
                },
                Reduce = @"from result in results
group result by result.SpeakerId into g
select new {
	SpeakerId = g.Key,
	SpeakerName = g.Where(l => l.SpeakerName != null).Select(l => l.SpeakerName).FirstOrDefault(),
	NumberOfSessions = g.Sum(l => l.NumberOfSessions)
}"
            };
        }
    }
}

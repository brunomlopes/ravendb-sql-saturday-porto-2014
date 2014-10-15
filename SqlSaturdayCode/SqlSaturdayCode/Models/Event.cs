using System;

namespace SqlSaturdayCode.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public string[] tags { get; set; }
        public EventState State { get; set; }
        public EventSession[] Sessions { get; set; }
    }

    public class EventSession
    {
        public string SpeakerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
    }

    public class Speaker
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }  

    public enum EventState { Draft, Published }
}
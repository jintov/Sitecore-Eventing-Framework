namespace SitecoreZone.Eventing.Samples
{
    using SitecoreZone.Eventing;
    using System.Runtime.Serialization;

    [DataContract]
    public class PublishCompleteEvent : SitecoreEvent
    {
        [DataMember]
        public string Message { get; set; }
    }
}

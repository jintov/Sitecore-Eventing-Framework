namespace SitecoreZone.Eventing.Samples
{
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Publishing.Pipelines.Publish;

    public class PublishCompleteEventGenerator : PublishProcessor
    {
        public override void Process(PublishContext context)
        {
            //Example of event being generated only on the local Sitecore instance
            PublishCompleteEvent event1 = new PublishCompleteEvent() { Message = "Local event" };
            event1.PublishLocally();

            //Example of event being generated on all Sitecore instances in the cluster, including local instance
            PublishCompleteEvent event2 = new PublishCompleteEvent() { Message = "Global event" };
            event2.PublishGlobally();

            //Example of event being generated on all remote Sitecore instances that uses the target database
            Database db = Factory.GetDatabase("web");
            PublishCompleteEvent event3 = new PublishCompleteEvent() { Message = "Remote event" };
            event3.PublishToRemoteTargetsOnly(db);
        }
    }
}

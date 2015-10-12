namespace SitecoreZone.Eventing.Samples
{
    using Sitecore.Diagnostics;
    using Sitecore.Events;
    using System;

    public class CacheClearEventHandler
    {
        public void OnPublishComplete(object sender, EventArgs args)
        {
            PublishCompleteEvent evt = (args as SitecoreEventArgs).Parameters[0] as PublishCompleteEvent;
            Log.Info(evt.Message + " - " + evt.ToString(), this);
        }
    }
}

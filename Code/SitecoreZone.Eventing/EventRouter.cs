namespace SitecoreZone.Eventing
{
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Eventing;
    using Sitecore.Events;

    /// <summary>
    /// Responsible for routing custom events to different instances
    /// </summary>
    public static class EventRouter
    {
        /// <summary>
        /// Raises the custom event on the local Sitecore instance, i.e., on the instance where the event is generated
        /// This does not use the Sitecore EventQueue
        /// </summary>
        /// <param name="evt">Custom event that is routed</param>
        public static void PublishLocally(this SitecoreEvent evt)
        {
            Assert.ArgumentNotNull((object)evt, "evt");

            if (!EventRegistry.IsEventRegistered(evt))
            {
                Log.Error(string.Format("Event '{0}' is not registered on this instance", evt.ToString()), typeof(SitecoreZone.Eventing.EventRouter));
            }

            evt.SetDefaultValues();
            Event.RaiseEvent(evt.EventName, new object[] { evt });
        }

        /// <summary>
        /// Raises the custom event on all Sitecore instances in the cluster (CM, CD, Publishing instance, etc.)
        /// The event is also raised on the local Sitecore instance, i.e., on the instance where the event is generated
        /// </summary>
        /// <param name="evt">Custom event that is routed</param>
        public static void PublishGlobally(this SitecoreEvent evt)
        {
            Assert.ArgumentNotNull((object)evt, "evt");

            if (!EventRegistry.IsEventRegistered(evt))
            {
                Log.Error(string.Format("Event '{0}' is not registered on this instance", evt.ToString()), typeof(SitecoreZone.Eventing.EventRouter));
            }

            evt.SetDefaultValues();
            //Queue events to global and local queues
            WrappedEvent wrappedEvent = new WrappedEvent() { Event = evt };
            EventManager.QueueEvent<WrappedEvent>(wrappedEvent, true, true);
        }

        /// <summary>
        /// Raises the custom event on all remote Sitecore instances in the cluster that uses the target database
        /// The event will not be raised on the local Sitecore instance, i.e., the instance where the event is generated
        /// </summary>
        /// <param name="evt">Custom event that is routed</param>
        /// <param name="remoteTargetDB">Target database where event has to be raised</param>
        public static void PublishToRemoteTargetsOnly(this SitecoreEvent evt, Database remoteTargetDB)
        {
            Assert.ArgumentNotNull((object)evt, "evt");
            Assert.ArgumentNotNull((object)remoteTargetDB, "remoteTargetDB");

            if (!EventRegistry.IsEventRegistered(evt))
            {
                Log.Error(string.Format("Event '{0}' is not registered on this instance", evt.ToString()), typeof(SitecoreZone.Eventing.EventRouter));
            }

            evt.SetDefaultValues();
            //Queue events for remote
            WrappedEvent wrappedEvent = new WrappedEvent() { Event = evt };
            remoteTargetDB.RemoteEvents.Queue.QueueEvent<WrappedEvent>(wrappedEvent);
        }
    }
}

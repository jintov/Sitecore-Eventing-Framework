namespace SitecoreZone.Eventing
{
    using Sitecore.Diagnostics;
    using Sitecore.Eventing;
    using Sitecore.Pipelines;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Stores information about all custom events registered in the application
    /// </summary>
    public class EventRegistry
    {
        #region Private Memebers

        private static Dictionary<Type, string> registeredEvents = new Dictionary<Type, string>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns event name configured for the eventType passed
        /// </summary>
        /// <param name="eventType">Type of the event</param>
        /// <returns>Name of the event for the given eventType</returns>
        public static string GetEventName(Type eventType)
        {
            return registeredEvents[eventType];
        }

        /// <summary>
        /// Returns the list of types that have been registsred
        /// </summary>
        /// <returns>List of types that are registered</returns>
        public static Type[] GetKnownTypes()
        {
            return registeredEvents.Keys.ToArray();
        }

        /// <summary>
        /// Determines if the type of the event is registered or not
        /// </summary>
        /// <param name="evt">Custom event</param>
        /// <returns>true if event type is registered, else false</returns>
        public static bool IsEventRegistered(SitecoreEvent evt)
        {
            return registeredEvents.Keys.Contains(evt.GetType());
        }

        /// <summary>
        /// Called by initialize pipeline to register the remote event handler
        /// </summary>
        /// <param name="args">Pipeline arguments</param>
        public void Process(PipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");

            Log.Debug("EventRegistry - BEGIN", this);
            EventManager.Subscribe<WrappedEvent>(new Action<WrappedEvent>(this.HandleRemoteEvent));
            Log.Debug("EventRegistry - END", this);
        }

        /// <summary>
        /// Event handler for remote events
        /// This method in turn generates local events on individual servers
        /// </summary>
        /// <param name="wrappedEvent">wrapped event</param>
        public void HandleRemoteEvent(WrappedEvent @wrappedEvent)
        {
            Log.Debug(string.Format("Handling remote event '{0}'", @wrappedEvent.Event.ToString()), this);
            @wrappedEvent.Event.PublishLocally();
        }

        /// <summary>
        /// Called by Sitecore to register events from the config file upon startup
        /// </summary>
        /// <param name="configNode">XML node containing the custom events information</param>
        public void RegisterEvent(XmlNode configNode)
        {
            Assert.ArgumentNotNull((object)configNode, "configNode");
            if (configNode.Attributes == null || configNode.Attributes["type"] == null)
            {
                Log.Error("Event not defined correctly; check your configuration", this);
                return;
            }

            Type eventType = this.GetEventType(configNode);
            if (eventType == null)
            {
                Log.Error("Event not defined correctly; check your configuration", this);
                return;
            }

            registeredEvents[eventType] = this.GetEventName(configNode, eventType);
        }

        #endregion

        #region Private / Internal Methods

        private Type GetEventType(XmlNode configNode)
        {
            if (configNode.Attributes["type"] == null ||
                string.IsNullOrEmpty(configNode.Attributes["type"].Value.Trim()))
            {
                return null;
            }

            try
            {
                return Type.GetType(configNode.Attributes["type"].Value.Trim());
            }
            catch
            {
                //if type name specified is not a valid type, return null
                return null;
            }
        }

        private string GetEventName(XmlNode configNode, Type eventType)
        {
            //if event name is explcitly specified in configuration, then return it
            if (configNode.Attributes["eventName"] != null &&
                !string.IsNullOrEmpty(configNode.Attributes["eventName"].Value.Trim()))
            {
                return configNode.Attributes["eventName"].Value.Trim();
            }

            //if event name is not specified, determine event name from the type
            return eventType.FullName.Replace(".", ":").ToLower();
        }

        #endregion
    }
}

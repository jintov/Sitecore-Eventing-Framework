namespace SitecoreZone.Eventing
{
    using Sitecore.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Base class for all events to derive from, if they need to be propagated using the Event Framework
    /// </summary>
    [DataContract]
    public class SitecoreEvent
    {
        #region Private Members

        private const string ToStringFormat = @"[Name: {0}, Time: {1}, Origin Instance {2}]";
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the custom event
        /// </summary>
        public SitecoreEvent()
        {
            this.EventData = new Dictionary<string, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the event; set automatically from the event type registration information in config file
        /// </summary>
        [DataMember]
        public string EventName { get; internal set; }

        /// <summary>
        /// Time when the event was generated
        /// </summary>
        [DataMember]
        public DateTime EventTime { get; internal set; }

        /// <summary>
        /// Instance name where the event was generated
        /// </summary>
        [DataMember]
        public string EventOriginInstance { get; internal set; }

        /// <summary>
        /// Dictionary to hold custom event data
        /// Any custom objects added to the dictionary should also be serializable,
        /// by using the [DataContract] and [DataMember] attributes
        /// </summary>
        [DataMember]
        public Dictionary<string, object> EventData { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Overrides the default .NET implementation to return a summary of event information (name, time and instance)
        /// </summary>
        /// <returns>Summary of event information (name, time and instance)</returns>
        public override string ToString()
        {
            return string.Format(ToStringFormat, this.EventName, this.EventTime.ToString(), this.EventOriginInstance ?? string.Empty);
        }

        #endregion

        #region Private / Internal Methods

        /// <summary>
        /// Sets the default event information, if not set already - such as event name, time and instance
        /// </summary>
        internal void SetDefaultValues()
        {
            if (string.IsNullOrEmpty(this.EventName))
            {
                this.EventName = EventRegistry.GetEventName(this.GetType());
                this.EventTime = DateTime.Now;
                this.EventOriginInstance = Settings.InstanceName;
            }
        }

        #endregion
    }
}

namespace SitecoreZone.Eventing
{
    using System;
    using System.Runtime.Serialization;

    ///<summary>
    ///Class used to wrap events before sending over the wire to other servers.
    ///A wrapper class is required because Sitecore's EventManager can subscribe to a concrete class, and not any derived types
    ///</summary>
    [DataContract]
    [KnownType("GetKnownTypes")]
    public class WrappedEvent
    {
        #region Properties

        /// <summary>
        /// The custom event that is being wrapped
        /// </summary>
        [DataMember]
        public SitecoreEvent Event { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the list of types that have been registsred
        /// </summary>
        /// <returns>List of types that are registered</returns>
        public static Type[] GetKnownTypes()
        {
            return EventRegistry.GetKnownTypes();
        }

        #endregion
    }
}

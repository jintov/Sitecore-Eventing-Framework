Sitecore-Eventing-Framework
=========================

This framework allows you to publish custom events in one Sitecore instance, and subscribe to those events in other Sitecore instances in your cluster / farm. It also allows you to pass any serializable data between Sitecore instances through these custom events. The framework internally leverages the Sitecore Event Queue, and provides a developer-friendly abstraction over it.

I will soon make the framework available as a Sitecore module in the Marketplace.

More details about the framework, including usage instructions, is available on my [blog post](https://thesitecorezone.wordpress.com/2015/10/12/custom-events-in-sitecore/).

**Usage Instructions Summary**

In a nutshell, below is what you will need to do:

 1. Define custom event by deriving from ***SitecoreZone.Eventing.SitecoreEvent*** class, and marking them as serializable using *[DataContract]* and *[DataMember]* attributes.
 2. Register the custom event through a patch config file
 3. Implement the event handler that handles the custom event
 4. Subscribe to the event by defining the event handler through a patch config file
 5. Implement the code that generates / raises the event and publishes to one of the following


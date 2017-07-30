

namespace Exrin.Abstraction
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// For tracking events through the frameworks handling of actions
    /// </summary>
    public interface IApplicationInsights
    {

        /// <summary>
        /// Get all insight reports ready for sending
        /// </summary>
        /// <returns></returns>
        Task<IBlockingQueue<IInsightData>> GetQueue();

        /// <summary>
        /// Optionally set the identity these events should be tracked to
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fullName"></param>
        void SetIdentity(string userId, string fullName);

        /// <summary>
        /// Track an event that occurred in the handling of an action
        /// </summary>
        /// <param name="objectName">Name of the object that caused the event</param>
        /// <param name="message">Additional details of the event</param>
        Task TrackEvent(string eventName, string message, string callerName = "");

        /// <summary>
        /// Timeouts, Load Times
        /// </summary>
        Task TrackMetric(string metricIdentifier, object value, string callerName = "");

        /// <summary>
        /// Tracking exceptions that occur within your application
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        Task TrackException(Exception ex, string callerName = "");

        /// <summary>
        /// Track a completely custom insight data object you created
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task TrackRaw(IInsightData data);
    }
}

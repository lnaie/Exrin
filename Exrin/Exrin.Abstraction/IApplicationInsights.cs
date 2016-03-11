using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exrin.Abstraction
{
    /// <summary>
    /// For tracking events through the frameworks handling of actions
    /// </summary>
    public interface IApplicationInsights
    {
        /// <summary>
        /// Get all insight reports ready for sending
        /// </summary>
        /// <returns></returns>
        Task<List<IInsightData>> GetQueue();

        /// <summary>
        /// Delete insight reports that have been sent
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task Clear(IList<IInsightData> list);

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
        void TrackEvent(string objectName, string message);

        /// <summary>
        /// Timeouts, Load Times
        /// </summary>
        void TrackMetric();

        void TrackException();

        void TrackCrash();
    }
}

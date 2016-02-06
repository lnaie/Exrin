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
        /// Track an event that occurred in the handling of an action
        /// </summary>
        /// <param name="objectName">Name of the object that caused the event</param>
        /// <param name="message">Additional details of the event</param>
        void TrackEvent(string objectName, string message);
    }
}

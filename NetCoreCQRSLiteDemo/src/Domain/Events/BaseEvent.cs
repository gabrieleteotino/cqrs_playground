using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Events
{
    // TODO i believe this class is an oversimplification and not useful at all in the long term
    public class BaseEvent : IEvent
    {
        #region IEvent
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public int Version { get; set; }
        #endregion
    }
}

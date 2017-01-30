using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class BaseEvent : IEvent
    {
        #region IEvent
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public int Version { get; set; }
        #endregion
    }
}

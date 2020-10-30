using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brightcast.Enums
{
    public enum ChatMessageStatusEnum
    { 
        Queued = 0,
        Received = 1,
        Sent = 2,
        Delivered = 3,
        Read = 4,
        Failed = 5
    }
}

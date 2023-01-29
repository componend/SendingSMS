using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNN.Enums
{
    public enum MessageStatus
    {
        NotDirty = 0,
        Success= 1,
        Pending = 3,
        Failed = 2
    }
}

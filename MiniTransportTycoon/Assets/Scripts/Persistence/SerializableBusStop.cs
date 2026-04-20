using System;

namespace Persistence
{
    [Serializable]
    public class SerializableBusStop : SerializableCell
    {
        public SerializableBusStop(BusStop busStop)
        {
            
        }
        public SerializableBusStop(){}
    }
}
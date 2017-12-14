using System;

namespace DDDify.Bus
{
    public class Event : IMessage
    {
        protected DateTime When;
    }
}

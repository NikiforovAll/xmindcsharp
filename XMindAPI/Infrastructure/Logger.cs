using System.Diagnostics.Tracing;

namespace XMindAPI.Infrastructure.Logging
{
    [EventSource(Name = "XMind-XMindCsharpEventSource")]
    internal sealed class Logger : EventSource
    {
        [Event(1, Keywords = Keywords.Requests, Message = "Start processing request\n\t*** {0}", Task = Tasks.Request, Opcode = EventOpcode.Start)]
        public void RequestStart(string RequestID) { WriteEvent(1, RequestID); }

        [Event(2, Keywords = Keywords.Requests, Message = "Entering Phase {1} for request {0}", Task = Tasks.Request, Opcode = EventOpcode.Info, Level = EventLevel.Verbose)]
        public void RequestPhase(string RequestID, string PhaseName) { WriteEvent(2, RequestID, PhaseName); }

        [Event(3, Keywords = Keywords.Requests, Message = "Stop processing request\n\t*** {0} ***", Task = Tasks.Request, Opcode = EventOpcode.Stop)]
        public void RequestStop(string RequestID) { WriteEvent(3, RequestID); }

        [Event(4, Keywords = Keywords.Debug, Message = "DebugMessage: {0}", Channel = EventChannel.Debug)]
        public void DebugTrace(string Message) { WriteEvent(4, Message); }

        [Event(5, Keywords = Keywords.Error, Message = "ErrorMessage: {0}", Channel = EventChannel.Debug, Level = EventLevel.Error)]
        public void Error(string Message) { WriteEvent(5, Message); }

        [Event(6, Keywords = Keywords.Error, Message = "ErrorMessage: {0}", Channel = EventChannel.Debug, Level = EventLevel.Warning)]
        public void Warning(string Message) { WriteEvent(6, Message); }

        public class Keywords   // This is a bitvector
        {
            public const EventKeywords Requests = (EventKeywords)0x0001;
            public const EventKeywords Debug = (EventKeywords)0x0002;
            public const EventKeywords Warning = (EventKeywords)0x0008;
            public const EventKeywords Error = (EventKeywords)0x0004;
        }

        public class Tasks
        {
            public const EventTask Request = (EventTask)0x1;
        }
        public static Logger Log = new Logger();
    }
}

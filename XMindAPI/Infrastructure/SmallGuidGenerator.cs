using System;

namespace XMindAPI.Infrastructure
{
    internal class SmallGuidGenerator
    {

        public static string NewGuid() =>
            Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
    }
}

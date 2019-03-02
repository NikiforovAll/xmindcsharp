using System;

namespace XMindAPI.Core
{
    internal class IdFactory : IIdFactory
    {
        public string CreateId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
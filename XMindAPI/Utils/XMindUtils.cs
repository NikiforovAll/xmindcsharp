using System;
using System.Linq;
using System.Xml.Linq;


namespace XMindAPI.Utils
{
    // USE DOM Utils instead
    internal static class XMindUtils
    {
        public static string NewId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GetTimeStamp()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }
        public static string GetAttribValue(XElement el, string attributeName)
        {
            return el.Attribute(attributeName).Value;
        }

    }
}

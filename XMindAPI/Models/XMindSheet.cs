using System;
using System.Collections.Generic;

namespace XMindAPI
{
    public class XMindSheet
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        public List<XMindTopic> TopicFlatList { get; private set; }
        public List<XMindTopic> Topics { get; private set; }

        private XMindSheet()
        {
            TopicFlatList = new List<XMindTopic>();
            Topics = new List<XMindTopic>();
        }

        internal XMindSheet(string sheetId, string sheetName) : this()
        {
            ID = sheetId;
            Name = sheetName;
        }

        public new string ToString()
        {
            return Name;
        }
    }

}
using System;
using System.Collections.Generic;

namespace XMindAPI
{

    /// <summary>
    ///  Base element of build XMind maps, topics are added to <see cref="XMindWorkBook"/>
    /// </summary>
    public class XMindTopic
    {
        /// <summary>
        ///   Topic ID
        /// </summary>
        /// <value></value>
        public string ID { get; private set; }
        /// <summary>
        /// Topic Name
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        /// <summary>
        /// Parent Topic
        /// </summary>
        /// <value></value>
        public XMindTopic Parent { get; private set; }
        /// <summary>
        /// List of descendant topics
        /// </summary>
        /// <value></value>
        public List<XMindTopic> Topics { get; private set; }

        private XMindTopic()
        {
            Topics = new List<XMindTopic>();
        }

        internal XMindTopic(XMindTopic parent, string id, string name) : this()
        {
            Parent = parent;
            ID = id;
            Name = name;
        }
    }

}
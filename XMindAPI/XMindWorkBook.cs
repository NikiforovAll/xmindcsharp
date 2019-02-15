using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO.Compression;
using System.Collections.Generic;

using XMindAPI.Configuration;
using XMindAPI.Writers;
using XMindAPI.Logging;

namespace XMindAPI
{
    /// <summary>
    /// XMindWorkBook encapsulates an XMind workbook and methods for performing actions on workbook content. 
    /// </summary>
    public class XMindWorkBook
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly XMindConfiguration _globalConfiguration;
        private readonly IXMindDocumentBuilder _documentBuilder;
        internal readonly XMindConfigurationCache _xMindSettings;
        

        // private string _fileName = null;
        

        /// <summary>
        /// Creates a new XMind workbook if loadContent is false, otherwise the file content will be loaded.
        /// </summary>
        // /// <param name="loadContent">If true, the current data from the file will be loaded, otherwise an empty workbook will be created.</param>
        internal XMindWorkBook(XMindConfiguration globalConfiguration, XMindConfigurationCache config)
        {
            _xMindSettings = config;
            _globalConfiguration = globalConfiguration;
            _documentBuilder = new XMindDocumentBuilder();
            _documentBuilder.CreateMetaFile();
            _documentBuilder.CreateManifestFile();
            _documentBuilder.CreateContentFile();
            //TODO: use builder in order to work with XDocuments, ideally get rid of this namespace in workbook
        }

        public List<XMindSheet> GetSheetInfo()
        {
            List<XMindSheet> lst = new List<XMindSheet>();

            foreach (XElement el in GetSheets())
            {
                string sheetId = GetAttribValue(el, "id");

                string centralTopicId = el.Descendants().Where(w1 => w1.Name.ToString().EndsWith("topic") && w1.Parent.Name.ToString().EndsWith("sheet"))
                    .Where(w3 => GetAttribValue(w3.Parent, "id") == sheetId)
                    .Select(s => GetAttribValue(s, "id")).First();

                string centralTopicTitle = GetTopic(centralTopicId).Descendants()
                    .Where(w2 => w2.Name.ToString().EndsWith("title")).Select(s => s.Value).First();

                XMindSheet xmSheet = new XMindSheet(sheetId,
                    el.Descendants()
                    .Where(w2 => w2.Name.ToString().EndsWith("title") && w2.Parent.Name.ToString().EndsWith("sheet"))
                    .Where(w3 => GetAttribValue(w3.Parent, "id") == sheetId)
                    .Select(s => s.Value).First());

                XMindTopic xmTopic = new XMindTopic(null, centralTopicId, centralTopicTitle);
                xmSheet.Topics.Add(xmTopic);

                xmSheet.TopicFlatList.Add(xmTopic);
                GetTopicsRecursively(GetTopic(centralTopicId), xmSheet, xmTopic);

                lst.Add(xmSheet);
            }

            return lst;
        }

        /// <summary>
        /// Add a new sheet to the workbook. 
        /// </summary>
        /// <param name="sheetName">Name of the new sheet (sheet title)</param>
        /// <returns>New sheet id</returns>
        public string AddSheet(string sheetName)
        {
            string sheetId = NewId();
            //TODO: 
            // _contentData.Root.Add(
            //     new XElement(_defaultContentNS + "sheet",
            //         new XAttribute("id", sheetId),
            //         new XAttribute("timestamp", GetTimeStamp()),
            //         new XElement(_defaultContentNS + "title", sheetName)
            //         ));

            return sheetId;
        }

        /// <summary>
        /// Get a list of sheet id's of all sheets matching the specified sheet title. Note: It is possible to have more than 
        /// one sheet in the same workbook with the same title.
        /// </summary>
        /// <param name="title">Sheet title to search for</param>
        /// <returns>List of sheet id's</returns>
        public List<string> GetSheetIdsByTitle(string title)
        {
            List<string> sheetsFound = new List<string>();

            foreach (XElement sheet in GetSheets())
            {
                sheetsFound.AddRange(sheet.Descendants()
                    .Where(w2 => w2.Name.ToString().EndsWith("title") && w2.Value == title && w2.Parent.Name.ToString().EndsWith("sheet"))
                    .Select(s => GetAttribValue(s.Parent, "id")).ToList());
            }

            return sheetsFound;
        }
        /// <summary>
        /// Add a new central topic to the specified sheet. A sheet must have one (and only one) central topic.
        /// </summary>
        /// <param name="sheetId">Sheet to add central topic to</param>
        /// <param name="topicName">Name of the central topic</param>
        /// <param name="structure">Type of diagram structure. Refer XMindStructure enum.</param>
        /// <returns>Id of newly created central topic</returns>
        public string AddCentralTopic(string sheetId, string topicName, XMindStructure structure)
        {
            XElement sheet = GetSheet(sheetId);

            if (sheet == null)
            {
                throw new InvalidOperationException("Sheet not found!");
            }

            if (GetTopics(sheet).Count() > 0)
            {
                throw new InvalidOperationException("Sheet can have only one central topic!");
            }

            string topicId = NewId();
            //TODO: 
            // var enumField = structure.GetType().GetFields().Where(field => field.Name == structure.ToString()).FirstOrDefault();
            // DescriptionAttribute[] a = (DescriptionAttribute[])enumField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // sheet.Add(
            //     new XElement(_defaultContentNS + "topic",
            //         new XAttribute("id", topicId),
            //         new XAttribute("structure-class", a[0].Description),
            //         new XAttribute("timestamp", GetTimeStamp()),
            //         new XElement(_defaultContentNS + "title", topicName)
            //         ));

            return topicId;
        }

        /// <summary>
        /// Add a topic to either a central topic or another topic. Diagram structure can be specified, if set to null 
        /// parent structure will be inherited.
        /// </summary>
        /// <param name="parentId">Id of parent topic</param>
        /// <param name="topicName">New topic title</param>
        /// <param name="structure">Type of diagram structure. Refer XMindStructure enum.</param>
        /// <returns>Newly created topic id</returns>
        public string AddTopic(string parentId, string topicName, XMindStructure? structure)
        {
            XElement parent = GetTopic(parentId);

            if (parent == null)
            {
                throw new InvalidOperationException("Topic not found!");
            }

            // Get topic children tag, if not exist create:
            XElement children = parent.Descendants().Where(w => w.Name.ToString().EndsWith("children")).FirstOrDefault();
            //TODO:
            // if (children == null)
            // {
            //     children = new XElement(_defaultContentNS + "children");
            //     parent.Add(children);
            // }

            // // Get topics tag, if not exists create:
            // XElement topics = children.Descendants().Where(w => w.Name.ToString().EndsWith("topics")).FirstOrDefault();

            // if (topics == null)
            // {
            //     topics = new XElement(_defaultContentNS + "topics",
            //         new XAttribute("type", "attached"));
            //     children.Add(topics);
            // }

            // // Add new topic to topics element:
            // string topicId = NewId();
            // XElement topicElement = new XElement(_defaultContentNS + "topic",
            //         new XAttribute("id", topicId),
            //         new XAttribute("timestamp", GetTimeStamp()),
            //         new XElement(_defaultContentNS + "title", topicName)
            //         );

            // if (structure != null)
            // {
            //     var enumField = structure.GetType().GetFields().Where(field => field.Name == structure.ToString()).FirstOrDefault();
            //     DescriptionAttribute[] a = (DescriptionAttribute[])enumField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            //     topicElement.Add(new XAttribute("structure-class", a[0].Description));
            // }

            // topics.Add(topicElement);

            return null;//topicId;
        }

        /// <summary>
        /// Add a topic to either a central topic or another topic. Parent diagram structure will be inherited.
        /// </summary>
        /// <param name="parentId">Id of parent topic</param>
        /// <param name="topicName">New topic title</param>
        /// <returns>Newly created topic id</returns>
        public string AddTopic(string parentId, string topicName)
        {
            return AddTopic(parentId, topicName, null);
        }

        /// <summary>
        /// Add a label to the specified topic.
        /// </summary>
        /// <param name="topicId">Id of topic to add label to</param>
        /// <param name="labelText">Label text</param>
        public void AddLabel(string topicId, string labelText)
        {
            XElement topic = GetTopic(topicId);

            if (topic == null)
            {
                throw new InvalidOperationException("Topic not found!");
            }
            //TODO:
            // Get topic labels tag, if not exist create:
            // XElement labels = topic.Descendants().Where(w => w.Name.ToString().EndsWith("labels")).FirstOrDefault();

            // if (labels == null)
            // {
            //     labels = new XElement(_defaultContentNS + "labels");
            //     topic.Add(labels);
            // }

            // // Get topic label tag, if not exist create:
            // XElement label = labels.Descendants().Where(w => w.Name.ToString().EndsWith("label")).FirstOrDefault();

            // if (label == null)
            // {
            //     label = new XElement(_defaultContentNS + "label");
            //     labels.Add(label);
            // }

            // label.Value = labelText;
        }

        /// <summary>
        /// Add a marker to an existing topic. Refer XMindMarkers enum for available markers.
        /// </summary>
        /// <param name="topicId">Id of topic to add marker to</param>
        /// <param name="marker">Marker type. Refer XMindMarkers enum</param>
        public void AddMarker(string topicId, XMindMarkers marker)
        {
            // TODO:
            // XElement topic = GetTopic(topicId);

            // if (topic == null)
            // {
            //     throw new InvalidOperationException("Topic not found!");
            // }

            // // Get topic marker-refs tag, if not exist create:
            // XElement marker_refs = topic.Descendants().Where(w => w.Name.ToString().EndsWith("marker-refs")).FirstOrDefault();

            // if (marker_refs == null)
            // {
            //     marker_refs = new XElement(_defaultContentNS + "marker-refs");
            //     topic.Add(marker_refs);
            // }

            // // Get topic marker_ref tag, if not exist create:
            // XElement marker_ref = marker_refs.Descendants().Where(w => w.Name.ToString().EndsWith("marker-ref")).FirstOrDefault();

            // if (marker_ref == null)
            // {
            //     marker_ref = new XElement(_defaultContentNS + "marker-ref");
            //     marker_refs.Add(marker_ref);
            // }

            // XAttribute att = marker_ref.Attributes().Where(w => w.Name == "marker-id").FirstOrDefault();

            // if (att != null)
            // {
            //     marker_ref.Attributes("marker-id").Remove();
            // }

            // var enumField = marker.GetType().GetFields().Where(field => field.Name == marker.ToString()).FirstOrDefault();
            // DescriptionAttribute[] a = (DescriptionAttribute[])enumField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // marker_ref.Add(new XAttribute("marker-id", a[0].Description));
        }

        /// <summary>
        /// Add a link from one topic to another.
        /// </summary>
        /// <param name="topicId">Id of topic to contain the link</param>
        /// <param name="linkToTopicId">Id of topic to link to</param>
        public void AddTopicLink(string topicId, string linkToTopicId)
        {
            XElement topic = GetTopic(topicId);
            //TODO:
            // if (topic == null)
            // {
            //     throw new InvalidOperationException("Topic not found!");
            // }

            // if (GetTopic(linkToTopicId) == null)
            // {
            //     throw new InvalidOperationException("Link to topic not found!");
            // }

            // XAttribute att = topic.Attributes().Where(w => w.Name == _xlinkNS + "href").FirstOrDefault();

            // if (att != null)
            // {
            //     topic.Attributes(_xlinkNS + "href").Remove();
            // }

            // topic.Add(new XAttribute(_xlinkNS + "href", "xmind:#" + linkToTopicId));
        }

        /// <summary>
        /// Collapse the child structure for the specified topic.
        /// </summary>
        /// <param name="topicId">Topic to collapse child structure</param>
        public void CollapseChildren(string topicId)
        {
            //TODO:
            XElement topic = GetTopic(topicId);

            if (topic == null)
            {
                throw new InvalidOperationException("Topic not found!");
            }

            XAttribute att = topic.Attributes().Where(w => w.Name == "branch").FirstOrDefault();

            if (att != null)
            {
                topic.Attributes("branch").Remove();
            }

            topic.Add(new XAttribute("branch", "folded"));
        }

        /// <summary>
        /// Change the title of a topic.
        /// </summary>
        /// <param name="topicId">Id of topic</param>
        /// <param name="newTitle">New title</param>
        public void EditTopicTitle(string topicId, string newTitle)
        {
            //TODO:
            // XElement topic = GetTopic(topicId);

            // if (topic != null)
            // {
            //     XElement titleElement = topic.Descendants().Where(w => w.Name.ToString().EndsWith("title")).First();
            //     titleElement.Value = newTitle;
            // }
        }

        /// <summary>
        /// Get the title text for the specified topic id.
        /// </summary>
        /// <param name="topicId">Topic id</param>
        /// <returns>Topic title</returns>
        public string GetTopicTitle(string topicId)
        {
            //TODO:
            string title = null;
            XElement topic = GetTopic(topicId);

            if (topic != null)
            {
                title = topic.Descendants().Where(w => w.Name.ToString().EndsWith("title")).Select(s => s.Value).First();
            }

            return title;
        }

        /// <summary>
        /// Get a list of topic id's where the title matches the suppled title. All sheets will be searched.
        /// </summary>
        /// <param name="title">Topic title to search for</param>
        /// <returns>List of topic titles found</returns>
        public List<string> GetTopicIdsByTitle(string title)
        {
            //TODO:
            List<string> topicsFound = new List<string>();

            foreach (XElement sheet in GetSheets())
            {
                topicsFound.AddRange(GetTopicIdsByTitle(GetAttribValue(sheet, "id"), title));
            }

            return topicsFound;
        }

        /// <summary>
        /// Get a list of topic id's where the title matches the suppled title. Only the specified sheet will be searched.
        /// </summary>
        /// <param name="sheetId">Sheet to search in</param>
        /// <param name="title">Topic title to search for</param>
        /// <returns>List of topic titles found</returns>
        public List<string> GetTopicIdsByTitle(string sheetId, string title)
        {
            //TODO:
            List<string> topicsFound = new List<string>();

            XElement sheet = GetSheet(sheetId);

            if (sheet != null)
            {
                topicsFound.AddRange(sheet.Descendants().Where(w1 => w1.Name.ToString().EndsWith("topic"))
                    .Descendants().Where(w2 => w2.Name.ToString().EndsWith("title") && w2.Value == title).Select(s => GetAttribValue(s.Parent, "id")).ToList());
            }

            return topicsFound;
        }

        /// <summary>
        /// Get a list of topic id's that contains the specified user tag where the user tag value mathes the specified value. 
        /// Also see method AddUserTag().
        /// </summary>
        /// <param name="tagName">User tag to search</param>
        /// <param name="searchValue">User tag value to match</param>
        /// <returns>List of topic id's where the user tag/value matches</returns>
        public List<string> GetTopicIdsByUserTagValue(string tagName, string searchValue)
        {
            //TODO:
            List<string> topicsFound = new List<string>();

            foreach (XElement sheet in GetSheets())
            {
                foreach (XElement topic in GetTopics(sheet))
                {
                    string topicId = GetAttribValue(topic, "id");
                    if (GetUserTagValues(topicId, tagName).Contains(searchValue))
                    {
                        topicsFound.Add(topicId);
                    }
                }
            }

            return topicsFound;
        }

        /// <summary>
        /// Add a user tag to the specified sheet or topic.
        /// </summary>
        /// <param name="itemId">Sheet or topic id to add user tag to</param>
        /// <param name="tagName">User tag name</param>
        /// <param name="tagValue">User tag value</param>
        public void AddUserTag(string itemId, string tagName, string tagValue)
        {
            // Check if itemId is a sheet:
            XElement item = GetSheet(itemId);

            // If not a sheet, check if itemid is a topic:
            if (item == null)
            {
                item = GetTopic(itemId);
            }
            //TODO:
            // if (item == null)
            // {
            //     throw new InvalidOperationException("Topic/Sheet not found!");
            // }

            // // Get user tags, if not exist create:
            // XElement userTags = item.Descendants().Where(w => w.Name.ToString().EndsWith("UserTags")).FirstOrDefault();

            // if (userTags == null)
            // {
            //     userTags = new XElement(_defaultContentNS + "UserTags");
            //     item.Add(userTags);
            // }

            // // Get the named user tag, if not exist create:
            // XElement userTag = userTags.Descendants()
            //     .Where(w => w.Name.ToString().EndsWith("UserTag") && GetAttribValue(w, "TagName") == tagName).FirstOrDefault();

            // if (userTag == null)
            // {
            //     userTag = new XElement(_defaultContentNS + "UserTag",
            //         new XAttribute("TagName", tagName),
            //         new XAttribute("TagValue", ""));
            //     userTags.Add(userTag);
            // }

            // userTag.SetAttributeValue(XName.Get("TagValue"), tagValue);
        }

        /// <summary>
        /// Get the values of a specified user tag from the specified sheet/topic.
        /// </summary>
        /// <param name="itemId">Id of sheet or topic to search</param>
        /// <param name="tagName">User tag name to search for</param>
        /// <returns>List of user tag values that was found</returns>
        public List<string> GetUserTagValues(string itemId, string tagName)
        {
            //TODO:
            List<string> tagValues = new List<string>();

            // Check if itemId is a sheet:
            XElement item = GetSheet(itemId);

            // If not a sheet, check if itemid is a topic:
            if (item == null)
            {
                item = GetTopic(itemId);
            }

            if (item == null)
            {
                return tagValues;
            }

            // Get user tags:
            XElement userTags = item.Descendants().Where(w => w.Name.ToString().EndsWith("UserTags")).FirstOrDefault();

            if (userTags == null)
            {
                return tagValues;
            }

            // Get the named user tag:
            foreach (XElement userTag in userTags.Descendants()
                .Where(w => w.Name.ToString().EndsWith("UserTag") && GetAttribValue(w, "TagName") == tagName))
            {
                tagValues.Add(GetAttribValue(userTag, "TagValue"));
            }

            return tagValues;
        }

        /// <summary>
        /// Save the current XMind workbook file to disk.
        /// </summary>
        public void Save()
        {
            var manifestFileName = _xMindSettings.XMindConfigCollection["output:definition:manifest"];
            var metaFileName = _xMindSettings.XMindConfigCollection["output:definition:meta"];
            var contentFileName = _xMindSettings.XMindConfigCollection["output:definition:content"];

            var files = new Dictionary<string, XDocument>(3)
            {
                [metaFileName] = _documentBuilder.MetaFile,
                [manifestFileName] = _documentBuilder.ManifestFile,
                [contentFileName] = _documentBuilder.ContentFile
            };

            var writerContexts = new List<XMindWriterContext>();
            foreach (var kvp in files)
            {
                var currentWriterContext = new XMindWriterContext()
                {
                    FileName = kvp.Key,
                    FileEntries = new XDocument[1] { kvp.Value }
                };
                var selectedWriters = _globalConfiguration
                    .WriteTo
                    .ResolveWriters(currentWriterContext);
                if (selectedWriters == null)
                {
                    throw new InvalidOperationException("XMindBook.Save: Writer is not selected");
                }
                selectedWriters.ForEach(w => w.WriteToStorage(kvp.Value, kvp.Key));
                writerContexts.Add(currentWriterContext);
            }
            _globalConfiguration.WriteTo.FinalizeAction?.Invoke(writerContexts);
        }


        /// <summary>
        /// Helper method to build nested topic structure used by public method GetSheetInfo().
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xmSheet"></param>
        /// <param name="xmTopic"></param>
        private void GetTopicsRecursively(XElement parent, XMindSheet xmSheet, XMindTopic xmTopic)
        {
            //TODO:
            foreach (XElement nextLevelTopic in parent.Descendants().Where(w1 => w1.Name.ToString().EndsWith("topic")
                && GetAttribValue(w1.Parent.Parent.Parent, "id") == GetAttribValue(parent, "id")))
            {
                string topicId = GetAttribValue(nextLevelTopic, "id");
                XMindTopic nextXmTopic = new XMindTopic(xmTopic, topicId, GetTopicTitle(topicId));

                xmSheet.TopicFlatList.Add(nextXmTopic);
                xmTopic.Topics.Add(nextXmTopic);
                GetTopicsRecursively(nextLevelTopic, xmSheet, nextXmTopic);
            }
        }

        private XElement GetSheet(string sheetId)
        {
            //TODO:
            return GetSheets()
                .Where(w => GetAttribValue(w, "id") == sheetId)
                .FirstOrDefault();
        }

        private List<XElement> GetSheets()
        {
            //TODO:
            // return _contentData.Root.Elements()
            //     .Where(w => w.Name.ToString()
            //     .EndsWith("sheet"))
            //     .ToList();
            return null;
        }

        private XElement GetTopic(string topicId)
        {
            //TODO:
            XElement topic = null;

            foreach (XElement sheet in GetSheets())
            {
                topic = GetTopics(sheet)
                    .Where(w => GetAttribValue(w, "id") == topicId)
                    .FirstOrDefault();

                if (topic != null) break;
            }

            return topic;
        }

        private List<XElement> GetTopics(XElement sheet)
        {
            //TODO:
            return sheet.Descendants()
                .Where(w => w.Name.ToString()
                .EndsWith("topic"))
                .ToList();
        }

        private string GetAttribValue(XElement el, string attributeName)
        {
            //TODO:
            XAttribute att = el.Attributes(attributeName)
                .FirstOrDefault();

            if (att == null)
            { return null; }
            else
            { return att.Value; }
        }

        private string NewId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        private string GetTimeStamp()
        {
            //TODO: timestamp vs guid
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}

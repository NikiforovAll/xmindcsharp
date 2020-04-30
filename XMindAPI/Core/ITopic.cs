using System.Collections.Generic;
using XMindAPI.Models;

namespace XMindAPI.Core
{
    public interface ITopic : IAdaptable, ITitled, ILabeled, IIdentifiable, ITopicComponent, IRelationshipEnd, IHyperLinked
    {

        TopicType Type { get; set; }

        bool IsFolded { get; set; }

        IList<ITopic> Children { get; set; }

        void Add(ITopic child, int index = -1, TopicType type = TopicType.Attached);
        void AddMarker(string markerId);

        void RemoveMarker(string markerId);

        bool HasMarker(string markerId);

    }
}

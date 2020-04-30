using System.Collections.Generic;
using XMindAPI.Models;

namespace XMindAPI.Core
{
    public interface ITopic : IAdaptable, ITitled, ILabeled, IIdentifiable, ITopicComponent, IRelationshipEnd
    {

        TopicType Type { get; set; }

        bool IsFolded { get; set; }

        IList<ITopic> Children { get; set; }

        void Add(ITopic child, int index = -1, TopicType type = TopicType.Attached);
    }
}

using System.Collections.Generic;

namespace XMindAPI.Core
{
    public interface ILabeled
    {
        HashSet<string> GetLabels();

        void SetLabels(ICollection<string> labels);

        void AddLabel(string label);
        void RemoveLabel(string label);

        void RemoveAllLabels();
    }
}
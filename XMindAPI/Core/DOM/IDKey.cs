using System.Xml.Linq;

namespace XMindAPI.Core.DOM
{
    internal struct IDKey
    {
        internal IDKey(XDocument document, string id)
        {
            Document = document;
            Id = id;
        }
        public XDocument Document { get; }
        public string Id { get; }

        // public override int GetHashCode()
        // {
        //     unchecked // Overflow is fine, just wrap
        //     {
        //         int hash = 17;
        //         hash = hash * 23 + Id.GetHashCode();
        //         hash = hash * 23 + Document.GetHashCode();
        //         return hash;
        //     }
        // }

        // public override bool Equals(object obj)
        // {
        //     if (obj == this)
        //     {
        //         return true;
        //     }
        //     if (obj == null || !(obj is IDKey))
        //     {
        //         return false;
        //     }
        //     IDKey? that = obj as IDKey;
        //     var document = that?.Document;
        //     return Document.Equals(document) && Id.Equals(that?.Id);
        // }
    }
}

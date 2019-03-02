using System.Xml.Linq;

namespace XMindAPI.Core.DOM
{
    internal class IDKey
    {
        public XDocument Document { get; set; }
        public string Id { get; set; }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Document.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj){
            if(obj == this)
            {
                return true;
            }
            if(obj == null || !(obj is IDKey)){
                return false;
            }
            IDKey that = obj as IDKey;
            return this.Document.Equals(that.Document) && this.Id.Equals(that.Id);
        }

    }
}
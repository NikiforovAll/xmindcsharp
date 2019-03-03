using System.Xml.Linq;

namespace XMindAPI.Core
{
    interface INodeAdaptableFactory
    {
        IAdaptable CreateAdaptable(XNode node);
    }
}
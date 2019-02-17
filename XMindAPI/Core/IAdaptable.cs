using System;

namespace XMindAPI.Core
{
    /// <summary>
    /// An interface for an adaptable object. Adaptable objects can be dynamically extended to provide different interfaces (or "adapters"). Workbooks and workbook components implement this interface to provide additional functionalities specific to their implementations. For example,IAdaptable a = [some adaptee];
    /// IFoo x = a.getAdapter(IFoo.class);
    /// if (x != null)
    /// [do IFoo things with x]
    /// </summary>
    public interface IAdaptable
    {

        /// <summary>
        /// Returns an object which is an instance of the given class associated with this object.
        /// </summary>
        /// <param name="adapter">the adapter class to look up</param>
        /// <typeparam name="T">class </typeparam>
        /// <returns>a object of the given class, or <code>null</code> if this object does not have an adapter for the given class</returns>
        T GetAdapter<T>(Type adapter);
    }
}
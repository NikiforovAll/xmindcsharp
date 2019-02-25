using System;
using System.Collections.Generic;

namespace XMindAPI.Core
{
    public abstract class AbstractWorkbook : IWorkbook
    {
        public void AddSheet(ISheet sheet)
        {
            throw new NotImplementedException();
        }

        public abstract IRelationship CreateRelationship(IRelationship rel1, IRelationship rel2);

        public abstract IRelationship CreateRelationship();

        public abstract ISheet CreateSheet();

        public abstract ITopic CreateTopic();

        public abstract object FindElement(string id, IAdaptable source);

        public abstract ITopic FindTopic();

        public T GetAdapter<T>(Type t)
        {
            return default(T);
        }

        public abstract object GetElementById(string id);

        public abstract ISheet GetPrimarySheet();

        public abstract IEnumerable<ISheet> GetSheets();

        public abstract void RemoveSheet(ISheet sheet);

        public abstract void Save();
    }
}
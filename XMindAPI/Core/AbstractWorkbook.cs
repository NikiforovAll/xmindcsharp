using System;
using System.Collections.Generic;

namespace XMindAPI.Core
{
    public abstract class AbstractWorkbook : IWorkbook
    {
        public void AddSheet(ISheet sheet)
        {
            AddSheet(sheet, -1);
        }

        public abstract void AddSheet(ISheet sheet, int index);

        public abstract IRelationship CreateRelationship(IRelationship rel1, IRelationship rel2);

        public abstract IRelationship CreateRelationship();

        public abstract ISheet CreateSheet();

        public abstract ITopic CreateTopic();

        public abstract object FindElement(string id, IAdaptable source);

        public ITopic FindTopic(string id, IAdaptable source)
        {
            Object element = FindElement(id, source);
            return element as ITopic;
        }

        public ITopic FindTopic(string id)
        {
            return FindTopic(id, this);
        }

        public T GetAdapter<T>(Type t)
        {
            return default(T);
        }

        public object GetElementById(string id)
        {
            return FindElement(id, this);
        }

        public abstract ISheet GetPrimarySheet();

        public abstract IEnumerable<ISheet> GetSheets();

        public abstract void RemoveSheet(ISheet sheet);

        public abstract void Save();
    }
}
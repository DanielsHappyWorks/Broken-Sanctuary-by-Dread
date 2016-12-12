using System.Collections.Generic;

namespace GDLibrary
{
    public class GenericList<T> : IEnumerable<T>
    {
        #region Variables
        private string name;
        private List<T> list;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public T this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = value;
            }
        }
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }
        #endregion

        public GenericList()
            : this("default", 10)
        {
        }

        public GenericList(string name)
            : this(name, 10)
        {
        }
        public GenericList(int initialSize)
            : this("default", initialSize)
        {
        }

        public GenericList(string name, int initialSize)
        {
            this.name = name;
            this.list = new List<T>(initialSize);
        }

        public void Add(T obj)
        {
            this.list.Add(obj);
        }
        public void Add(GenericList<T> objList)
        {
            foreach (T obj in objList)
                this.list.Add(obj);
        }
        public bool Remove(T obj)
        {
            return this.list.Remove(obj);
        }
        public bool Remove(IFilter<T> filter)
        {
            T obj = Find(filter);
            if (obj != null)
            {
                this.list.Add(obj);
                return true;
            }
            return false;
        }
        public int RemoveAll(IFilter<T> filter)
        {
            int count = 0;
            List<T> outList = FindAll(filter);
            foreach (T obj in outList)
            {
                this.list.Remove(obj);
                count++;
            }
            return count;
        }
        public T Find(IFilter<T> filter)
        {
            T obj;
            for (int i = 0; i < this.list.Count; i++)
            {
                obj = LanguageUtility.ConvertValue<T>(this.list[i]);
                if (filter.Matches(obj))
                    return obj;
            }
            return default(T);
        }
        public List<T> FindAll(IFilter<T> filter)
        {
            List<T> outList = new List<T>();
            T obj;
            for (int i = 0; i < this.list.Count; i++)
            {
                obj = LanguageUtility.ConvertValue<T>(this.list[i]);
                if (filter.Matches(obj))
                    outList.Add(obj);
            }

            //if nothing found then return null, otherwise return list
            return outList.Count > 0 ? outList : null;
        }

        public void Clear()
        {
            this.list.Clear();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); //calls the method above
        }
    }
}

using System.Collections.Generic;

namespace FW.ErrorHandler
{
    /// <summary>
    /// Responsability Chain
    /// http://www.info-ab.uclm.es/asignaturas/42579/cap4/Comportamiento/chain.htm
    /// </summary>
    /// <typeparam name="T">Receivers of the responsability</typeparam>
    public class ResponsabilityChainBase<T> : IList<T>
    {
        /// <summary>
        /// Creates an instance of the ResponsabilityChainBase class
        /// </summary>
        public ResponsabilityChainBase()
        {
        }

        /// <summary>
        /// Destroys the instance of the ResponsabilityChainBase class
        /// </summary>
        ~ResponsabilityChainBase()
        {
            Chain.Clear();
            Chain = null;
        }

        private System.Collections.Generic.IList<T> m_chain;
        /// <summary>
        /// The responsability chain
        /// </summary>
        protected System.Collections.Generic.IList<T> Chain
        {
            get
            {
                // Double-checked locking and the Singleton pattern
                // More in: http://www.ibm.com/developerworks/java/library/j-dcl.html
                if (null == m_chain)
                {
                    lock (this.SyncRoot)
                    {
                        if (null == m_chain)
                        {
                            m_chain = new List<T>() as IList<T>;
                        }
                    }
                }
                return m_chain;
            }
            set { m_chain = value; }
        }

        private object m_syncRoot = new object();
        /// <summary>
        /// Synchronization object for the Responsability Chain
        /// </summary>
        public object SyncRoot
        {
            get { return m_syncRoot; }
            set { m_syncRoot = value; }
        }


        #region Miembros de IList<T>

        int IList<T>.IndexOf(T item)
        {
            return Chain.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            Chain.Insert(
                index,
                item);
        }

        void IList<T>.RemoveAt(int index)
        {
            Chain.RemoveAt(index);
        }

        T IList<T>.this[int index]
        {
            get
            {
                return Chain[index];
            }
            set
            {
                Chain[index] = value;
            }
        }

        #endregion

        #region Miembros de ICollection<T>

        void ICollection<T>.Add(T item)
        {
            Chain.Add(item);
        }

        void ICollection<T>.Clear()
        {
            Chain.Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
            return Chain.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Chain.CopyTo(
                array,
                arrayIndex);
        }

        int ICollection<T>.Count
        {
            get
            {
                return Chain.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return Chain.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            return Chain.Remove(item);
        }

        #endregion

        #region Miembros de IEnumerable<T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Chain.GetEnumerator();
        }

        #endregion


        #region Miembros de IEnumerable

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Chain as System.Collections.IList).GetEnumerator();
        }

        #endregion
    }
}

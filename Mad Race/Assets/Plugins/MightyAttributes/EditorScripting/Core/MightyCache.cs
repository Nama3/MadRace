#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MightyAttributes.Editor
{
    public class MightyCache<T>
    {
        private readonly Dictionary<long, T> m_cache = new Dictionary<long, T>();

        public bool Contains(BaseMightyMember mightyMember) => Contains(mightyMember.ID);
        public bool Contains(long id) => m_cache.ContainsKey(id);

        public bool TryGetValue(BaseMightyMember mightyMember, out T value) => TryGetValue(mightyMember.ID, out value);
        public bool TryGetValue(long id, out T value) => m_cache.TryGetValue(id, out value);

        public void ClearCache() => m_cache.Clear();

        public T this[BaseMightyMember mightyMember]
        {
            get => this[mightyMember.ID];
            set => this[mightyMember.ID] = value;
        }

        public T this[long id]
        {
            get => m_cache[id];
            set => m_cache[id] = value;
        }

        public Dictionary<long, T>.ValueCollection Values => m_cache.Values;
        public int Count => m_cache.Count;
    }

    public class MightyCache<Tk, Tv>
    {
        private readonly Dictionary<(long, Tk), Tv> m_cache = new Dictionary<(long, Tk), Tv>();

        public bool Contains(BaseMightyMember mightyMember, Tk keyItem) => Contains(mightyMember.ID, keyItem);
        public bool Contains(long id, Tk keyItem) => m_cache.ContainsKey((id, keyItem));

        public bool TryGetValue(BaseMightyMember mightyMember, Tk keyItem, out Tv value) =>
            TryGetValue(mightyMember.ID, keyItem, out value);

        public bool TryGetValue(long id, Tk keyItem, out Tv value) =>
            m_cache.TryGetValue((id, keyItem), out value);

        public void ClearCache() => m_cache.Clear();

        public Tv this[BaseMightyMember mightyMember, Tk keyItem]
        {
            get => this[mightyMember.ID, keyItem];
            set => this[mightyMember.ID, keyItem] = value;
        }

        public Tv this[long id, Tk keyItem]
        {
            get => m_cache[(id, keyItem)];
            set => m_cache[(id, keyItem)] = value;
        }

        public Dictionary<(long, Tk), Tv>.ValueCollection Values => m_cache.Values;
        public int Count => m_cache.Count;
    }

    public class MightyMembersCache
    {
        private readonly List<BaseMightyMember> m_cache = new List<BaseMightyMember>();

        public int Count => m_cache.Count;
        public ICollection<BaseMightyMember> Values => m_cache;

        public MightyMember<T> Add<T>(MightyMember<T> mightyMember) where T : MemberInfo
        {
            m_cache.Add(mightyMember);
            return mightyMember;
        }

        public void ClearCache() => m_cache.Clear();

        public bool TryGetMightyMember<T>(T memberInfo, out MightyMember<T> mightyMember) where T : MemberInfo
        {
            foreach (var member in m_cache)
            {
                if (!(member is MightyMember<T> childMember)) continue;
                if (childMember.MemberInfo != memberInfo) continue;
                mightyMember = childMember;
                return true;
            }

            mightyMember = null;
            return false;
        }

        public BaseMightyMember this[short i] => m_cache[i];

        public void ApplyDrawOrders()
        {
            var count = Count;
            if (count <= 0) return;

            var orderAndMembers = GetOrderedMembers().OrderBy(o => o.drawOrder).ToArray();

            var length = orderAndMembers.Length;

            if (length == 0) return;
            if (length == count)
            {
                for (var i = 0; i < count; i++)
                    m_cache[i] = orderAndMembers[i].mightyMember;

                return;
            }

            var orderedIndexes = new List<short>();

            foreach (var (member, order) in orderAndMembers)
            {
                if (member.DrawIndex > order)
                {
                    short i;
                    for (i = member.DrawIndex; i > order && i > 0; i--)
                    {
                        if (orderedIndexes.Contains((short) (i - 1))) break;
                        SwapCacheItem(i, (short) (i - 1));
                    }

                    orderedIndexes.Add(i);
                }
                else if (member.DrawIndex < order)
                {
                    for (var i = member.DrawIndex; i < order && i < count - 1; i++)
                    {
                        if (orderedIndexes.Contains(i)) continue;
                        SwapCacheItem(i, (short) (i + 1));
                    }
                }
            }
        }
        
        private IEnumerable<(BaseMightyMember mightyMember, short drawOrder)> GetOrderedMembers()
        {
            for (var i = 0; i < Count; i++)
            {
                var member = m_cache[i];
                if (member.TryGetDrawOrder(out var drawOrder))
                    yield return (member, (short) (drawOrder - 1));
            }
        }

        private void SwapCacheItem(short index1, short index2)
        {
            var other = m_cache[index2];
            m_cache[index2] = m_cache[index1];
            m_cache[index1] = other;
            m_cache[index1].DrawIndex = index1;
            m_cache[index2].DrawIndex = index2;
        }
    }

    public class MightyMembersCache<T> where T : MemberInfo
    {
        private readonly List<MightyMember<T>> m_cache = new List<MightyMember<T>>();

        public int Count => m_cache.Count;
        public ICollection<MightyMember<T>> Values => m_cache;

        public MightyMember<T> Add(MightyMember<T> mightyMember)
        {
            m_cache.Add(mightyMember);
            return mightyMember;
        }

        public void ClearCache() => m_cache.Clear();

        public bool TryGetMightyMember(T memberInfo, out MightyMember<T> mightyMember)
        {
            foreach (var member in m_cache)
            {
                if (member.MemberInfo != memberInfo) continue;
                mightyMember = member;
                return true;
            }

            mightyMember = null;
            return false;
        }
    }
}
#endif
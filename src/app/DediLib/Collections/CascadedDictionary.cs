﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DediLib.Collections
{
    public class CascadedDictionary<T1, T2> : ConcurrentDictionary<T1, T2>
    {
        public IEnumerable<T2> AllValues => Values;

        public bool TryRemove(T1 key)
        {
            T2 value;
            return TryRemove(key, out value);
        }
    }

    public class CascadedDictionary<T1, T2, T3> : CascadedDictionary<T1, CascadedDictionary<T2, T3>>
    {
        public bool ContainsKey(T1 t, T2 u)
        {
            CascadedDictionary<T2, T3> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            if (!subDict.ContainsKey(u)) return false;
            return true;
        }

        public T3 this[T1 t, T2 u]
        {
            get
            {
                CascadedDictionary<T2, T3> subDict;
                if (!TryGetValue(t, out subDict)) throw new KeyNotFoundException($"First key {t} not found in dictionary");
                T3 value;
                if (!subDict.TryGetValue(u, out value)) throw new KeyNotFoundException($"Keys {t}, {u} not found in dictionary");
                return value;
            }
            set
            {
                CascadedDictionary<T2, T3> subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3>());
                subDict[u] = value;
            }
        }

        public bool TryGetValue(T1 t, T2 u, out T3 v)
        {
            v = default(T3);
            CascadedDictionary<T2, T3> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            if (!subDict.TryGetValue(u, out v)) return false;
            return true;
        }

        public bool TryRemove(T1 t, T2 u)
        {
            CascadedDictionary<T2, T3> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            return subDict.TryRemove(u);
        }

        public bool TryRemove(T1 t, T2 u, out T3 v)
        {
            CascadedDictionary<T2, T3> subDict;
            if (!TryGetValue(t, out subDict))
            {
                v = default(T3);
                return false;
            }
            return subDict.TryRemove(u, out v);
        }

        public T3 AddOrUpdate(T1 t, T2 u, T3 v, Func<T1, T2, T3, T3> factory)
        {
            var subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3>());
            return subDict.AddOrUpdate(u, v, (au, av) => factory(t, au, av));
        }

        public T3 GetOrAdd(T1 t, T2 u, Func<T1, T2, T3> factory)
        {
            var subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3>());
            return subDict.GetOrAdd(u, x => factory(t, u));
        }

        public new IEnumerable<T3> AllValues
        {
            get
            {
                foreach (var t in Keys)
                {
                    CascadedDictionary<T2, T3> subDict;
                    if (!TryGetValue(t, out subDict)) continue;

                    foreach (var v in subDict.Values)
                        yield return v;
                }
            }
        }
    }

    public class CascadedDictionary<T1, T2, T3, T4> : CascadedDictionary<T1, CascadedDictionary<T2, T3, T4>>
    {
        public bool ContainsKey(T1 t, T2 u, T3 v)
        {
            CascadedDictionary<T2, T3, T4> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            CascadedDictionary<T3, T4> subSubDict;
            if (!subDict.TryGetValue(u, out subSubDict)) return false;
            if (!subSubDict.ContainsKey(v)) return false;
            return true;
        }

        public CascadedDictionary<T3, T4> this[T1 t, T2 u]
        {
            get
            {
                CascadedDictionary<T2, T3, T4> subDict;
                if (!TryGetValue(t, out subDict)) throw new KeyNotFoundException($"First key {t} not found in dictionary");
                CascadedDictionary<T3, T4> subSubDict;
                if (!subDict.TryGetValue(u, out subSubDict)) throw new KeyNotFoundException($"Keys {t}, {u} not found in dictionary");
                return subSubDict;
            }
            set
            {
                CascadedDictionary<T2, T3, T4> subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3, T4>());
                subDict[u] = value;
            }
        }

        public T4 this[T1 t, T2 u, T3 v]
        {
            get
            {
                CascadedDictionary<T2, T3, T4> subDict;
                if (!TryGetValue(t, out subDict)) throw new KeyNotFoundException($"First key {t} not found in dictionary");
                CascadedDictionary<T3, T4> subSubDict;
                if (!subDict.TryGetValue(u, out subSubDict)) throw new KeyNotFoundException($"Keys {t}, {u} not found in dictionary");
                T4 value;
                if (!subSubDict.TryGetValue(v, out value)) throw new KeyNotFoundException($"Keys {t}, {u}, {v} not found in dictionary");
                return value;
            }
            set
            {
                CascadedDictionary<T2, T3, T4> subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3, T4>());
                CascadedDictionary<T3, T4> subSubDict = subDict.GetOrAdd(u, x => new CascadedDictionary<T3, T4>());
                subSubDict[v] = value;
            }
        }

        public bool TryGetValue(T1 t, T2 u, out CascadedDictionary<T3, T4> v)
        {
            v = default(CascadedDictionary<T3, T4>);
            CascadedDictionary<T2, T3, T4> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            if (!subDict.TryGetValue(u, out v)) return false;
            return true;
        }

        public bool TryGetValue(T1 t, T2 u, T3 v, out T4 w)
        {
            w = default(T4);
            CascadedDictionary<T2, T3, T4> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            CascadedDictionary<T3, T4> subSubDict;
            if (!subDict.TryGetValue(u, out subSubDict)) return false;
            if (!subSubDict.TryGetValue(v, out w)) return false;
            return true;
        }

        public bool TryRemove(T1 t, T2 u)
        {
            CascadedDictionary<T2, T3, T4> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            return subDict.TryRemove(u);
        }

        public bool TryRemove(T1 t, T2 u, T3 v)
        {
            CascadedDictionary<T2, T3, T4> subDict;
            if (!TryGetValue(t, out subDict)) return false;
            CascadedDictionary<T3, T4> subSubDict;
            if (!subDict.TryGetValue(u, out subSubDict)) return false;
            return subSubDict.TryRemove(v);
        }

        public bool TryRemove(T1 t, T2 u, T3 v, out T4 w)
        {
            CascadedDictionary<T2, T3, T4> subDict;
            if (!TryGetValue(t, out subDict))
            {
                w = default(T4);
                return false;
            }
            CascadedDictionary<T3, T4> subSubDict;
            if (!subDict.TryGetValue(u, out subSubDict))
            {
                w = default(T4);
                return false;
            }
            return subSubDict.TryRemove(v, out w);
        }

        public T4 AddOrUpdate(T1 t, T2 u, T3 v, T4 w, Func<T1, T2, T3, T4, T4> factory)
        {
            var subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3, T4>());
            var subSubDict = subDict.GetOrAdd(u, x => new CascadedDictionary<T3, T4>());
            return subSubDict.AddOrUpdate(v, w, (av, aw) => factory(t, u, av, aw));
        }

        public T4 GetOrAdd(T1 t, T2 u, T3 v, Func<T1, T2, T3, T4> factory)
        {
            var subDict = GetOrAdd(t, x => new CascadedDictionary<T2, T3, T4>());
            var subSubDict = subDict.GetOrAdd(u, x => new CascadedDictionary<T3, T4>());
            return subSubDict.GetOrAdd(v, x => factory(t, u, v));
        }

        public new IEnumerable<T4> AllValues
        {
            get
            {
                foreach (var t in Keys)
                {
                    CascadedDictionary<T2, T3, T4> subDict;
                    if (!TryGetValue(t, out subDict)) continue;

                    foreach (var u in subDict.Keys)
                    {
                        CascadedDictionary<T3, T4> subSubDict;
                        if (!subDict.TryGetValue(u, out subSubDict)) continue;

                        foreach (var w in subSubDict.Values)
                            yield return w;
                    }
                }
            }
        }
    }
}

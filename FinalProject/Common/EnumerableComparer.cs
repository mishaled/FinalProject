using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EnumerableComparer
    {
        public static bool CompareLists<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null || list2 == null)
                return list1 == list2;

            if (list1.Count != list2.Count)
                return false;

            Dictionary<T, int> hash = new Dictionary<T, int>();
            foreach (T item in list1)
            {
                if (hash.ContainsKey(item))
                {
                    hash[item]++;
                }
                else
                {
                    hash.Add(item, 1);
                }
            }

            foreach (T item in list2)
            {
                if (!hash.ContainsKey(item) || hash[item] == 0)
                {
                    return false;
                }
                hash[item]--;
            }

            return true;
        }
    }
}

namespace AtomSandbox.Tools
{
    internal static class ContainerExtensions
    {
        public static int RemoveIf<T>(this IList<T> list, Func<T, int, bool> removePredicate, Action<T, int>? beforeDeleteAction = null)
        {
            int numOfDeleted = 0;
            for (int i = list.Count - 1; i >= 0; --i)
            {
                var item = list[i];
                if (removePredicate(item, i))
                {
                    beforeDeleteAction?.Invoke(item, i);
                    list.RemoveAt(i);
                    ++numOfDeleted;
                }
            }
            return numOfDeleted;
        }

        public static List<T> KeepRemoveIf<T>(this IList<T> list, Predicate<T> removePredicate)
        {
            var result = new List<T>();
            for (int i = list.Count - 1; i >= 0; --i)
            {
                var item = list[i];
                if (removePredicate(item))
                {
                    result.Add(item);
                    list.RemoveAt(i);
                }
            }
            return result;
        }
    }
}

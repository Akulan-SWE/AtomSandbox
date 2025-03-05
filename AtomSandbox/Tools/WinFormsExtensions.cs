namespace AtomSandbox.Tools
{
    static class WinFormsExtensions
    {
        public static Dictionary<Type, List<Control>> GetAllChildren(this Control parent, List<Type> controlTypes, params Control[] bannedControls)
        {
            var result = new Dictionary<Type, List<Control>>();
            loopAllChildren(parent, controlTypes, result, bannedControls);
            return result;
        }

        private static void loopAllChildren(Control parent, List<Type> controlTypes, Dictionary<Type, List<Control>> container, params Control[] bannedControls)
        {
            if (bannedControls == null || bannedControls.Length == 0 || !bannedControls.Contains(parent))
            {
                foreach (Control item in parent.Controls)
                {
                    if (item != null)
                    {
                        var itemType = item.GetType();
                        if (controlTypes.Contains(itemType))
                        {
                            if (container.ContainsKey(itemType))
                                container[itemType].Add(item);
                            else
                                container.Add(itemType, new List<Control> { item });
                        }
                        else if (item.Controls.Count != 0)
                            loopAllChildren(item, controlTypes, container, bannedControls);
                    }
                }
            }
        }
    }
}

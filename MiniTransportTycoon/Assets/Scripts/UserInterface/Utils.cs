using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface
{
    public static class Utils
    {
        public static void ToggleVisibility(this VisualElement element)
        { 
            if (element.style.display == DisplayStyle.Flex){ Disable(element); }
            else{ Enable(element); }
        }

        public static void Disable(this VisualElement element)
        {
            element.style.display = DisplayStyle.None;
        }
        public static void Enable(this VisualElement element)
        {
            element.style.display = DisplayStyle.Flex;
        }

        public static bool IsEnabled(this VisualElement element)
        {
            return element.style.display == DisplayStyle.Flex;
        }
    }
}
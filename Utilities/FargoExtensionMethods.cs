using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace FargowiltasSouls.Utilities
{
    public static class FargoExtensionMethods
    {
        /// <summary>
        /// Adjusts a TooltipLine to account for prefixes. <br />
        /// Inteded to be used specifically for item names. <br />
        /// This only modifies it in the inventory.
        /// </summary>
        public static TooltipLine ArticlePrefixAdjustment(this TooltipLine itemName, int prefixID, string[] localizationArticles)
        {
            List<string> splitName = itemName.text.Split(' ').ToList();

            for (int i = 0; i < localizationArticles.Length; i++)
                if (splitName.Remove(localizationArticles[i]))
                {
                    splitName.Insert(0, localizationArticles[i]);
                    break;
                }

            itemName.text = string.Join(" ", splitName);
            return itemName;
        }
    }
}
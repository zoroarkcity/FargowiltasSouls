using FargowiltasSouls.Utilities;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items
{
    /// <summary>
    /// Abstract class extended by the items of this mod. <br />
    /// Contains useful code for boilerplate reduction.
    /// </summary>
    public abstract class SoulsItem : ModItem
    {
        /// <summary>
        /// Whether or not this item is excluse to Eternity Mode. <br />
        /// If it is, the item's text color will automatically be set to a custom color (can manually be overriden) and "Eternity" will be added to the end of the item's tooltips.
        /// </summary>
        public virtual bool Eternity => false;

        /// <summary>
        /// A list of articles that this item may begin with depending on localization. <br />
        /// Used for the prefix-article fix.
        /// </summary>
        public virtual List<string> Articles => new List<string> { "The" };

        /// <summary>
        /// Allows you to modify all the tooltips that display for this item. <br />
        /// Called directly after the code in <see cref="SafeModifyTooltips(List{TooltipLine})"/>.
        /// </summary>
        /// <param name="tooltips"></param>
        public virtual void SafeModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (tooltips.TryFindTooltipLine("ItemName", out TooltipLine itemNameLine))
            {
                // If this item is exclusive to e-mode, give it a custom item "rarity" (not an actual rarity, wait for 1.4).
                // This is often overridden.
                if (Eternity)
                {
                    itemNameLine.overrideColor = Fargowiltas.EModeColor();

                    tooltips.Add(new TooltipLine(mod, $"{mod.Name}:Eternity", "Eternity"));
                }

                // Call the artcle-prefix adjustment method.
                // This automatically handles fixing item names that begin with an article.
                itemNameLine.ArticlePrefixAdjustment(item.prefix, Articles.ToArray());
            }

            SafeModifyTooltips(tooltips);
        }
    }
}
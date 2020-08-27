using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Tiles
{
    public class DeviMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Deviantt)");
            Tooltip.SetDefault("Sakuzyo 'Lexus Cyanixs'");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Main.DiscoColor;
                }
            }
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = mod.TileType("DeviMusicBoxSheet");
            item.width = 32;
            item.height = 32;
            item.rare = 3;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.accessory = true;
        }
    }
}

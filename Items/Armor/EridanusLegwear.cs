using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class EridanusLegwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eridanus Legwear");
            Tooltip.SetDefault(@"top text");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 11;
            item.value = Item.sellPrice(0, 14);
            item.defense = 0;
        }

        public override void UpdateEquip(Player player)
        {
            
        }
    }
}

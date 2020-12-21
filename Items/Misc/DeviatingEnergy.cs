using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace FargowiltasSouls.Items.Misc
{
    public class DeviatingEnergy : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deviating Energy");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(0, 1, 0, 0);
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 10));
        }
    }
}
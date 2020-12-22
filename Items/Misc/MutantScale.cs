using Terraria;
using Terraria.ID;

namespace FargowiltasSouls.Items.Misc
{
    public class MutantScale : SoulsItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abominable Energy");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(0, 4, 0, 0);
        }
    }
}
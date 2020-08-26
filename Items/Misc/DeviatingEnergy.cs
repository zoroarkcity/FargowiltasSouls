using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;

namespace FargowiltasSouls.Items.Misc
{
	public class DeviatingEnergy : ModItem
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
            item.rare = 3;
            item.value = Item.sellPrice(0, 1, 0, 0);
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 10));
        }
    }
}

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Misc
{
	public class MutantP1Reset : ModItem
	{
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boss Count Reset");
            Tooltip.SetDefault(@"Makes Mutant forget you have defeated his first phase
Results not guaranteed in multiplayer
You probably shouldn't be reading this...");
		}

		public override void SetDefaults()
		{
            item.width = 20;
            item.height = 20;
            item.rare = 1;
            item.useStyle = 4;
            item.useAnimation = 45;
            item.useTime = 45;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            if (player.itemAnimation > 0 && player.itemTime == 0)
            {
                FargoSoulsWorld.skipMutantP1 = false;
                Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
            }
            return true;
        }
    }
}

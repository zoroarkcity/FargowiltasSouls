using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Debug
{
	public class BossCountDisable : ModItem
	{
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boss Count Disable");
            Tooltip.SetDefault(@"Prevents bosses from scaling when killed
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
                FargoSoulsWorld.SlimeCount = 0;
                FargoSoulsWorld.EyeCount = 0;
                FargoSoulsWorld.EaterCount = 0;
                FargoSoulsWorld.BrainCount = 0;
                FargoSoulsWorld.BeeCount = 0;
                FargoSoulsWorld.SkeletronCount = 0;
                FargoSoulsWorld.WallCount = 0;
                FargoSoulsWorld.TwinsCount = 0;
                FargoSoulsWorld.DestroyerCount = 0;
                FargoSoulsWorld.PrimeCount = 0;
                FargoSoulsWorld.PlanteraCount = 0;
                FargoSoulsWorld.GolemCount = 0;
                FargoSoulsWorld.FishronCount = 0;
                FargoSoulsWorld.CultistCount = 0;
                FargoSoulsWorld.MoonlordCount = 0;
                Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);

                FargoSoulsWorld.NoMasoBossScaling = true;

                if (FargoSoulsWorld.NoMasoBossScaling)
                    Main.NewText("Boss scaling is now DISABLED.");
                else
                    Main.NewText("Boss scaling is now ENABLED.");
            }
            return true;
        }
    }
}

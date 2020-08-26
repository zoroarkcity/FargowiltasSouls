using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Mounts
{
    public class AcornConstruct : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Ride the Squirrel");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 30000;
            item.rare = 2;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = ModContent.MountType<TrojanSquirrelMount>();
        }
    }
}
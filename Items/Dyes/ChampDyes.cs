using Terraria.ID;

namespace FargowiltasSouls.Items.Dyes
{
    public class LifeDye : SoulsItem
    {
        //public override string Texture => "FargowiltasSouls/Items/Dyes/LifeDye";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavenly Dye");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.rare = ItemRarityID.Orange;
        }
    }

    public class WillDye : SoulsItem
    {
        //public override string Texture => "FargowiltasSouls/Items/Dyes/LifeDye";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Willpower Dye");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.rare = ItemRarityID.Orange;
        }
    }

    public class GaiaDye : SoulsItem
    {
        //public override string Texture => "FargowiltasSouls/Items/Dyes/LifeDye";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gaia Dye");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.rare = ItemRarityID.Orange;
        }
    }
}
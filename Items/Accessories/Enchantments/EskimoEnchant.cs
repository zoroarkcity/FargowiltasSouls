using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class EskimoEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"''
goes into frost enchant
You can walk on water and when you do, it freezes and creates spikes";


        public EskimoEnchant() : base("Eskimo Enchantment", TOOLTIP, 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 10), ItemRarityID.Lime, null)
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "爱斯基摩魔石");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"''
变为霜冻魔石
可以水上行走, 如此做时, 水会结冰并产生尖刺
");
        }


        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            /*
             * if(player.walkingOnWater)
{
	Create Ice Rod Projectile right below you
}

NearbyEffects:

if(modPlayer.EskimoEnchant && tile.type == IceRodBlock)
{
	Create spikes
}
             */
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.PinkEskimoHood);
            recipe.AddIngredient(ItemID.PinkEskimoCoat);
            recipe.AddIngredient(ItemID.PinkEskimoPants);
            recipe.AddIngredient(ItemID.FrostMinnow);
            recipe.AddIngredient(ItemID.AtlanticCod);
            recipe.AddIngredient(ItemID.MarshmallowonaStick);
        }
    }
}

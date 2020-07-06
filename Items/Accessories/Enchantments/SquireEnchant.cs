using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class SquireEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Squire Enchantment");
            Tooltip.SetDefault(
@"Continually attacking an enemy will grant you the Power of Squire
You will reduce enemy immunity frames during this time
Ballista pierces more targets and panics when you take damage
'Squire, will you hurry?'");
            DisplayName.AddTranslation(GameCulture.Chinese, "精金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'谁需要瞄准?'
第8个抛射物将会分裂成3个
分裂出的抛射物同样可以分裂");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().SquireEnchant = true;
            player.setSquireT2 = true;
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.SquirePanic))
                player.buffImmune[BuffID.BallistaPanic] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SquireGreatHelm);
            recipe.AddIngredient(ItemID.SquirePlating);
            recipe.AddIngredient(ItemID.SquireGreaves);
            recipe.AddIngredient(ItemID.SquireShield);
            recipe.AddIngredient(ItemID.DD2BallistraTowerT2Popper);
            //rally
            //lance
            recipe.AddIngredient(ItemID.RedPhasesaber);
            recipe.AddIngredient(ItemID.DD2SquireDemonSword);
            //light discs

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.BasicAccessories;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class BeeEnchant : EnchantmentItem
    {
        public int timer;


        public BeeEnchant() : base("Bee Enchantment", "", 20, 20, 
            TileID.CrystalBall, Item.sellPrice(gold: 1), ItemRarityID.Orange, new Color(254, 246, 37))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip = 
@"'According to all known laws of aviation, there is no way a bee should be able to fly'
50% chance for any friendly bee to become a Mega Bee
Mega Bees ignore most enemy defense, immune frames, and last twice as long
";
            string tooltip_ch = 
@"'根据目前所知的所有航空原理, 蜜蜂应该根本不可能会飞'
50%概率使友善的蜜蜂成为巨型蜜蜂
巨型蜜蜂忽略大多数敌人的防御, 无敌帧, 并持续双倍的时间
";
            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip += "Effects of Bee Booties\n";
                tooltip_ch += "拥有蜜蜂靴的效果\n";
            }

            tooltip += "Summons a pet Baby Hornet";
            tooltip_ch += "召唤一只小黄蜂";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "蜜蜂魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().BeeEffect(hideVisual);
            
            if(Fargowiltas.Instance.ThoriumLoaded) Thorium(player, hideVisual);
        }

        private void Thorium(Player player, bool hideVisual)
        {
            //bee booties
            if (SoulConfig.Instance.GetValue("Bee Booties"))
            {
                ModContent.GetInstance<BeeBoots>().UpdateAccessory(player, hideVisual);
                player.moveSpeed -= 0.15f;
                player.maxRunSpeed -= 1f;
            }
        }

        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.BeeHeadgear);
            recipe.AddIngredient(ItemID.BeeBreastplate);
            recipe.AddIngredient(ItemID.BeeGreaves);
            recipe.AddIngredient(ItemID.HiveBackpack);

            recipe.AddIngredient(ItemID.BeeGun);
            recipe.AddIngredient(ItemID.WaspGun);

            recipe.AddIngredient(ItemID.Nectar);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ItemID.NettleBurst);

            recipe.AddIngredient(ModContent.ItemType<BeeBoots>());
            recipe.AddIngredient(ModContent.ItemType<HoneyRecorder>());
        }
    }
}

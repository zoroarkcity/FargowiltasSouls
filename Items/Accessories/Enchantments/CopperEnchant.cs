using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.RangedItems;
using ThoriumMod.Items.ThunderBird;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CopperEnchant : EnchantmentItem
    {
        public int timer;


        public CopperEnchant() : base("Copper Enchantment", "", 20, 20,
            TileID.DemonAltar, Item.sellPrice(silver: 80), ItemRarityID.Orange, new Color(213, 102, 23))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip = @"'Behold'
Attacks have a chance to shock enemies with lightning
If an enemy is wet, the chance and damage is increased
Attacks that cause Wet cannot proc the lightning";

            string tooltip_ch =
@"'注视'
攻击有概率用闪电打击敌人
如果敌人处于潮湿状态,增加概率和伤害
造成潮湿的攻击不能触发闪电";

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip += "\nEffects of the Copper Buckler";
                tooltip_ch += "\n拥有铜制圆盾的效果";
            }

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "铜魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().CopperEnchant = true;

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player);
        }

        private void Thorium(Player player)
        {
            ThoriumPlayer thoriumPlayer = ModContent.GetInstance<ThoriumPlayer>();

            //copper shield
            timer++;

            if (timer >= 30)
            {
                int num = 10;

                if (thoriumPlayer.shieldHealth <= num)
                {
                    thoriumPlayer.shieldHealthTimerStop = true;
                }

                if (thoriumPlayer.shieldHealth < num)
                {
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(51, 255, 255), 1, false, true);
                    thoriumPlayer.shieldHealth++;
                    player.statLife++;
                }

                timer = 0;
            }
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.CopperHelmet);
            recipe.AddIngredient(ItemID.CopperChainmail);
            recipe.AddIngredient(ItemID.CopperGreaves);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<CopperBuckler>());
            recipe.AddIngredient(ModContent.ItemType<ThunderTalon>());
            recipe.AddIngredient(ModContent.ItemType<Zapper>());

            recipe.AddIngredient(ItemID.CopperShortsword);
            recipe.AddIngredient(ItemID.AmethystStaff);
            recipe.AddIngredient(ItemID.PurplePhaseblade);
            recipe.AddIngredient(ItemID.FirstEncounter);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.CopperShortsword);
            recipe.AddIngredient(ItemID.AmethystStaff);
            recipe.AddIngredient(ItemID.FirstEncounter);

            recipe.AddIngredient(ItemID.Wire, 20);
        }
    }
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.Misc;
using ThoriumMod.Items.NPCItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class LeadEnchant : EnchantmentItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;


        public LeadEnchant() : base("Lead Enchantment", "", 20, 20,
            TileID.DemonAltar, Item.sellPrice(silver: 40), ItemRarityID.Blue, new Color(67, 69, 88))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip =
@"'Not recommended for eating'
Attacks may inflict enemies with Lead Poisoning
Lead Poisoning deals damage over time and spreads to nearby enemies";
            string tooltip_ch =
@"'不建议食用'
攻击概率使敌人铅中毒
铅中毒随时间造成伤害,并传播给附近敌人";

            if(thorium != null)
            {
                tooltip += "\nEffects of Lead Shield";
                tooltip_ch += "\n拥有铅盾的效果";
            }

            Tooltip.SetDefault(tooltip);

            DisplayName.AddTranslation(GameCulture.Chinese, "铅魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().LeadEnchant = true;

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player);
        }


        private void Thorium(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            timer++;
            if (timer >= 30)
            {
                int num = 13;
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
            recipe.AddIngredient(ItemID.LeadHelmet);
            recipe.AddIngredient(ItemID.LeadChainmail);
            recipe.AddIngredient(ItemID.LeadGreaves);

            recipe.AddIngredient(ItemID.SulphurButterfly);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<LeadShield>());
            recipe.AddIngredient(ModContent.ItemType<OnyxStaff>());
            recipe.AddIngredient(ModContent.ItemType<RustySword>());

            recipe.AddIngredient(ItemID.LeadShortsword);
            recipe.AddIngredient(ItemID.LeadPickaxe);
            recipe.AddIngredient(ItemID.GrayPaint, 100);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.LeadShortsword);
            recipe.AddIngredient(ItemID.LeadPickaxe);
            recipe.AddIngredient(ItemID.GrayPaint, 100);
        }
    }
}

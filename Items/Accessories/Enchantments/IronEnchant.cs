using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.Misc;
using ThoriumMod.Items.Thorium;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class IronEnchant : EnchantmentItem
    {
        public int timer;


        public IronEnchant() : base("Iron Enchantment", "", 20, 20,
            TileID.DemonAltar, Item.sellPrice(silver: 80), ItemRarityID.Green, new Color(152, 142, 131))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip = "'Strike while the iron is hot'\n";
            string tooltip_ch = "'趁热打铁'\n";

            tooltip += 
@"Allows the player to dash into the enemy
Right Click to guard with your shield
You attract items from a larger range";
            tooltip_ch +=
@"允许使用者向敌人冲刺
右键用盾牌防御
拾取物品半径增大";

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip += "\nEffects of Iron Shield";
                tooltip_ch += "\n拥有铁盾的效果";
            }

            Tooltip.SetDefault(tooltip); 

            DisplayName.AddTranslation(GameCulture.Chinese, "铁魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            //sheild raise
            modPlayer.IronEffect();
            //magnet
            if (SoulConfig.Instance.GetValue("Iron Magnet"))
            {
                modPlayer.IronEnchant = true;
            }
            //EoC Shield
            player.dash = 2;

            if (Fargowiltas.Instance.ThoriumLoaded) 
                Thorium(player);
        }

        private void Thorium(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            //thorium shield
            timer++;
            if (timer >= 30)
            {
                int num = 18;
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
            recipe.AddIngredient(ItemID.IronHelmet);
            recipe.AddIngredient(ItemID.IronChainmail);
            recipe.AddIngredient(ItemID.IronGreaves);

            recipe.AddIngredient(ItemID.ZebraSwallowtailButterfly);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<IronShield>());
            recipe.AddIngredient(ModContent.ItemType<ThoriumShield>());
            recipe.AddIngredient(ModContent.ItemType<OpalStaff>());

            recipe.AddIngredient(ItemID.EoCShield);
            recipe.AddIngredient(ItemID.IronBroadsword);
            recipe.AddIngredient(ItemID.IronAnvil);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.EoCShield);
            recipe.AddIngredient(ItemID.IronBroadsword);
            recipe.AddIngredient(ItemID.IronAnvil);
        }
    }
}

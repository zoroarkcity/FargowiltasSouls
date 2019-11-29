using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Projectiles.Healer;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ForbiddenEnchant : EnchantmentItem
    {
        public ForbiddenEnchant() : base("Forbidden Enchantment", "", 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 3), ItemRarityID.Pink, new Color(231, 178, 28))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip =
@"'Walk like an Egyptian'
Double tap down to call an ancient storm to the cursor location
Any projectiles shot through your storm gain 50% damage
";
            string tooltip_ch =
@"'走路像个埃及人Z(￣ｰ￣)Z'
双击'下'键可召唤一个远古风暴到光标位置
任何穿过风暴的抛射物获得额外50%伤害";

            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip +=
@"Effects of Karmic Holder";
                tooltip_ch +=
@"拥有业果之握的效果";
            }

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "禁忌魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().ForbiddenEffect();

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player);
        }

        private void Thorium(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            thoriumPlayer.karmicHolder = true;
            if (thoriumPlayer.healStreak >= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<KarmicHolderPro>()] < 1)
            {
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<KarmicHolderPro>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.AncientBattleArmorHat);
            recipe.AddIngredient(ItemID.AncientBattleArmorShirt);
            recipe.AddIngredient(ItemID.AncientBattleArmorPants);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("KarmicHolder"));
                recipe.AddIngredient(thorium.ItemType("WhisperRa"));
                recipe.AddIngredient(thorium.ItemType("AxeBlade"), 300);
                recipe.AddIngredient(ItemID.SpiritFlame);
                recipe.AddIngredient(ItemID.BookStaff);
				recipe.AddIngredient(ItemID.Scorpion);
			}
            else
            {
                recipe.AddIngredient(ItemID.SpiritFlame);
                recipe.AddIngredient(ItemID.BookStaff);
				recipe.AddRecipeGroup("Scorpions");
            }

            recipe.AddIngredient(ItemID.DjinnsCurse);
            
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

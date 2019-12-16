using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class RedRidingEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Riding Enchantment");
            Tooltip.SetDefault(
@"'Big Bad Red Riding Hood'
Double tap down to create a localized rain of arrows at the cursor's position for a few seconds
The arrow type and damage is based on your first ammo slot
This has a cooldown of 15 seconds
Greatly enhances Explosive Traps effectiveness
Effects of Celestial Shell
Summons a pet Puppy");
            DisplayName.AddTranslation(GameCulture.Chinese, "红色游侠魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'大坏红帽'
满月时,攻击概率造成大出血
对低血量的敌人伤害增加
大幅加强爆炸陷阱能力
拥有天界贝壳的效果
召唤一只小狗");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(192, 27, 60);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 250000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.RedRidingEffect(hideVisual);
            modPlayer.HuntressEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HuntressAltHead);
            recipe.AddIngredient(ItemID.HuntressAltShirt);
            recipe.AddIngredient(ItemID.HuntressAltPants);
            recipe.AddIngredient(ModContent.ItemType<HuntressEnchant>());
            recipe.AddIngredient(ItemID.CelestialShell);

            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                recipe.AddIngredient(thorium.ItemType("BloodyHighClaws"));
                recipe.AddIngredient(thorium.ItemType("LadyLight"));
                recipe.AddIngredient(ItemID.DD2BetsyBow);
                recipe.AddIngredient(ItemID.DD2ExplosiveTrapT3Popper);
            }
            else
            {
                recipe.AddIngredient(ItemID.DD2BetsyBow);
            }
            
            recipe.AddIngredient(ItemID.DogWhistle);
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

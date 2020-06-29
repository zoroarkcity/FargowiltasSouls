using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    //[AutoloadEquip(EquipType.Shoes)]
    public class SupersonicSoul : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Supersonic Soul"); 

            string tooltip = 
@"'I am speed'
Allows Supersonic running, flight, and extra mobility on ice
Allows the holder to quintuple jump if no wings are equipped
Increases jump height, jump speed, and allows auto-jump
Grants the ability to swim and greatly extends underwater breathing
Provides the ability to walk on water and lava
Grants immunity to lava and fall damage
Effects of Flying Carpet";
            string tooltip_ch =
@"'我就是速度'
获得超音速奔跑,飞行,以及额外的冰上移动力
在没有装备翅膀时,允许使用者进行五段跳
增加跳跃高度,跳跃速度,允许自动跳跃
获得游泳能力以及极长的水下呼吸时间
获得水/岩浆上行走能力
免疫岩浆和坠落伤害
拥有飞毯效果";

            if (thorium != null)
            {
                tooltip += "\nEffects of Air Walkers, Survivalist Boots, and Weighted Winglets";
                tooltip_ch += "\n拥有履空靴,我命至上主义者之飞靴和举足轻重靴的效果";
            }

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "超音速之魂");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 750000;
            item.rare = 11;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(238, 0, 69));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.SupersonicSoul(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<Masomode.AeolusBoots>());
            //hellfire treads
            //mountss

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                recipe.AddIngredient(thorium.ItemType("TerrariumBoots"));
                recipe.AddIngredient(thorium.ItemType("AirWalkers"));
                recipe.AddIngredient(thorium.ItemType("SurvivalistBoots"));
                recipe.AddIngredient(thorium.ItemType("WeightedWinglets"));
                recipe.AddIngredient(ItemID.ArcticDivingGear);
            }
            else
            {
                recipe.AddIngredient(ItemID.ArcticDivingGear);
            }

            if (Fargowiltas.Instance.CalamityLoaded)
            {
                recipe.AddIngredient(calamity.ItemType("MOAB"));
            }
            else
            {
                recipe.AddIngredient(ItemID.FrogLeg); //frog gear
                recipe.AddIngredient(ItemID.BundleofBalloons);
            }

            recipe.AddIngredient(ItemID.BalloonHorseshoeSharkron);
            recipe.AddIngredient(ItemID.FlyingCarpet);
            recipe.AddIngredient(ItemID.MinecartMech);
            recipe.AddIngredient(ItemID.BlessedApple);
            recipe.AddIngredient(ItemID.AncientHorn);
            recipe.AddIngredient(ItemID.ReindeerBells);
            recipe.AddIngredient(ItemID.BrainScrambler);

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

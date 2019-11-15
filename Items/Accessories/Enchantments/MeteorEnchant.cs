using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.RangedItems;
using ThoriumMod.Items.Scouter;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MeteorEnchant : EnchantmentItem
    {
        public MeteorEnchant() : base("Meteor Enchantment", "", 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 2), ItemRarityID.Pink, new Color(95, 71, 82))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip = 
@"'Cosmic power builds your destructive prowess'
A meteor shower initiates every few seconds while attacking";
            string tooltip_ch =
@"'宇宙之力构建你的毁灭力量'
攻击时,每隔几秒爆发一次流星雨";

            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip += "\nSummons a pet Bio-Feeder";
                tooltip_ch += "\n召唤一个奇怪的外星生物";
            }

            Tooltip.SetDefault(tooltip);

            DisplayName.AddTranslation(GameCulture.Chinese, "陨星魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.MeteorEffect(50);

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player, hideVisual);
        }

        private void Thorium(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.bioPet = true;
            player.GetModPlayer<FargoPlayer>().AddPet("Bio-Feeder Pet", hideVisual, thorium.BuffType("BioFeederBuff"), thorium.ProjectileType("BioFeederPet"));
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.MeteorHelmet);
            recipe.AddIngredient(ItemID.MeteorSuit);
            recipe.AddIngredient(ItemID.MeteorLeggings);
            recipe.AddIngredient(ItemID.SpaceGun);
            recipe.AddIngredient(ItemID.StarCannon);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<StarTrail>());
            recipe.AddIngredient(ModContent.ItemType<CometCrossfire>());
            recipe.AddIngredient(ModContent.ItemType<BioPod>());

            recipe.AddIngredient(ItemID.MeteorStaff);
            recipe.AddIngredient(ItemID.PlaceAbovetheClouds);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.MeteorStaff);
            recipe.AddIngredient(ItemID.PlaceAbovetheClouds);
        }
    }
}

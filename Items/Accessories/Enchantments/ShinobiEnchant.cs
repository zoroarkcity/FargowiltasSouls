using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ShinobiEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shinobi Infiltrator Enchantment");

            string tooltip =
@"'Village Hidden in the Wall'
Dash into any walls, to teleport through them to the next opening
Stand still to gain a single use monk dash
Throw a smoke bomb to teleport to it and gain the First Strike Buff
Using the Rod of Discord will also grant this buff
Greatly enhances Lightning Aura effectiveness
Effects of Master Ninja Gear
Summons a pet Gato and Black Cat";
            string tooltip_ch =
@"'藏在墙中的村庄'
冲进墙壁时,会直接穿过去
扔烟雾弹进行传送并获得先发制人Buff
使用裂位法杖也会获得该Buff
大大加强闪电光环的效果
召唤一只宠物小喵和黑色小猫咪";

            Tooltip.SetDefault(tooltip); 
            DisplayName.AddTranslation(GameCulture.Chinese, "潜行忍者魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(147, 91, 24);
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
            //ninja gear
            player.blackBelt = true;
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.ShinobiClimbing))
                player.spikedBoots = 2;
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.ShinobiTabi, false))
                player.dash = 1;
            //tele thru wall
            modPlayer.ShinobiEffect(hideVisual);
            //monk dash mayhem
            modPlayer.MonkEffect();
            //ninja, smoke bombs, pet
            modPlayer.NinjaEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MonkAltHead);
            recipe.AddIngredient(ItemID.MonkAltShirt);
            recipe.AddIngredient(ItemID.MonkAltPants);
            recipe.AddIngredient(null, "NinjaEnchant");
            recipe.AddIngredient(null, "MonkEnchant");
            recipe.AddIngredient(ItemID.MasterNinjaGear);
            recipe.AddIngredient(ItemID.DD2PetGato);
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

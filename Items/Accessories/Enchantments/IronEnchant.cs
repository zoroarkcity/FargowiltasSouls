using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    [AutoloadEquip(EquipType.Shield)]
    public class IronEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron Enchantment");

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

            Tooltip.SetDefault(tooltip); 
            DisplayName.AddTranslation(GameCulture.Chinese, "铁魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(152, 142, 131);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 2;
            item.value = 40000;
            //item.shieldSlot = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            //EoC Shield
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.CthulhuShield))
            {
                player.dash = 2;
            }
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.IronShield))
            {
                //shield
                modPlayer.IronEffect();
            }
            //magnet
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.IronMagnet, false))
            {
                modPlayer.IronEnchant = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronHelmet);
            recipe.AddIngredient(ItemID.IronChainmail);
            recipe.AddIngredient(ItemID.IronGreaves);
            recipe.AddIngredient(ItemID.EoCShield);
            //treasure magnet
            //empty bucket
            recipe.AddIngredient(ItemID.IronBroadsword);
            recipe.AddIngredient(ItemID.IronBow);
            //apricot (high in iron pog)
            recipe.AddIngredient(ItemID.ZebraSwallowtailButterfly);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

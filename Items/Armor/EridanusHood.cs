using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class EridanusHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eridanus Hood");
            Tooltip.SetDefault(@"top text");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 11;
            item.value = Item.sellPrice(0, 14);
            item.defense = 0;
        }

        public override void UpdateEquip(Player player)
        {
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EridanusBattleplate>() && legs.type == ModContent.ItemType<EridanusLegwear>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = @"bottom text";
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Pets
{
    public class ChibiHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chibi Hat");
            Tooltip.SetDefault("Summons Chibi Devi\n'Cute! Cute! Cute!'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WispinaBottle);
            item.value = Item.sellPrice(0, 5);
            item.rare = -13;
            item.shoot = mod.ProjectileType("ChibiDevi");
            item.buffType = mod.BuffType("ChibiDeviBuff");
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
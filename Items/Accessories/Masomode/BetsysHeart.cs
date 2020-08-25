using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class BetsysHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betsy's Heart");
            Tooltip.SetDefault(@"Grants immunity to Oozed, Withered Weapon, and Withered Armor
Your critical strikes inflict Betsy's Curse
Press the Fireball Dash key to perform a short invincible dash
'Lightly roasted, medium rare'");
            DisplayName.AddTranslation(GameCulture.Chinese, "贝特希之心");
            Tooltip.AddTranslation(GameCulture.Chinese, @"'微烤,五分熟'
免疫渗出,枯萎武器和枯萎盔甲
暴击造成贝特希的诅咒
按下火球冲刺按键来进行一次短程的无敌冲刺");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 9;
            item.value = Item.sellPrice(0, 7);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.OgreSpit] = true;
            player.buffImmune[BuffID.WitheredWeapon] = true;
            player.buffImmune[BuffID.WitheredArmor] = true;
            player.GetModPlayer<FargoPlayer>().BetsysHeart = true;
        }
    }
}

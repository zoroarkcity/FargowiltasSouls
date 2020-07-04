using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    [AutoloadEquip(EquipType.Shield)]
    public class SlimyShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slimy Shield");
            Tooltip.SetDefault(@"Grants immunity to Slimed
15% increased fall speed
When you land after a jump, slime will fall from the sky over your cursor
'Torn from the innards of a defeated foe'");
            DisplayName.AddTranslation(GameCulture.Chinese, "粘液盾");
            Tooltip.AddTranslation(GameCulture.Chinese, @"'从被打败的敌人的内脏中撕裂而来'
免疫黏糊
增加15%下落速度
跳跃落地后,在光标处落下史莱姆");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 2;
            item.value = Item.sellPrice(0, 1);
            item.defense = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Slimed] = true;
            player.maxFallSpeed *= 2f;
            player.GetModPlayer<FargoPlayer>().SlimyShield = true;
        }
    }
}

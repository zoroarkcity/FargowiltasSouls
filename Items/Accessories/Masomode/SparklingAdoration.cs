using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class SparklingAdoration : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkling Adoration");
            Tooltip.SetDefault(@"Grants immunity to Lovestruck and Fake Hearts
Graze projectiles to gain up to 30% increased critical damage
Critical damage bonus decreases over time and is fully lost on hit
Your attacks periodically summon life-draining hearts
'With all of your emotion!'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 11));
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 4;
            item.value = Item.sellPrice(0, 3);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Lovestruck] = true;
            player.buffImmune[mod.BuffType("Lovestruck")] = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.Graze, false))
                player.GetModPlayer<FargoPlayer>().Graze = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.DevianttHearts))
                player.GetModPlayer<FargoPlayer>().DevianttHearts = true;
        }
    }
}

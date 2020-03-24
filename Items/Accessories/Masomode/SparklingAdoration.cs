using Terraria;
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
            Tooltip.SetDefault(@"'With all of your emotion!'
Grants immunity to Lovestruck
Graze projectiles to gain up to 30% increased critical damage
Critical damage bonus decreases over time and is fully lost on hit
Critical strikes periodically summon life-draining hearts");
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

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.Graze))
                player.GetModPlayer<FargoPlayer>().Graze = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.DevianttHearts))
                player.GetModPlayer<FargoPlayer>().DevianttHearts = true;
        }
    }
}

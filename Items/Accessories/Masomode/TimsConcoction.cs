using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class TimsConcoction : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tim's Concoction");
            Tooltip.SetDefault(@"Certain enemies will drop potions when defeated
'Smells funny'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 5;
            item.value = Item.sellPrice(0, 4);
            item.GetGlobalItem<EternityItem>().Eternity = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.TimsConcoction))
                player.GetModPlayer<FargoPlayer>().TimsConcoction = true;
        }
    }
}

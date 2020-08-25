using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    [AutoloadEquip(EquipType.Shoes)]
    public class EurusSock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eurus Socks");
            Tooltip.SetDefault(
@"The wearer can run pretty fast");
            DisplayName.AddTranslation(GameCulture.Chinese, "欧洛斯之袜");
            Tooltip.AddTranslation(GameCulture.Chinese, @"穿戴者可跑的非常快");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 50000;
            item.rare = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.accRunSpeed = 4f;
        }
    }
}

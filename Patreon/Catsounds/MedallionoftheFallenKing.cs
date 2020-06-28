using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Patreon.Catsounds
{
    public class MedallionoftheFallenKing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medallion of the Fallen King");
            Tooltip.SetDefault(
@"Spawns a King Slime Minion that scales with summon damage
King Slime Minion uses custom immunity frames and shoots out slime spikes similar to Masochist Mode King Slime");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 1;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<KingSlimeMinionBuff>(), 2);
        }
    }
}

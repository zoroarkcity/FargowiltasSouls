using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Souls
{
    public class MonkBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Meditation");
            Description.SetDefault("You have a one use Monk Dash");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex]++;
            //player.GetModPlayer<FargoPlayer>().FirstStrike = true;
        }
    }
}
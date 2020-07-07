using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Buffs.Souls
{
    public class ValhallaBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Power of Valhalla");
            Description.SetDefault("Removing most enemy immunity frames");
            Main.buffNoSave[Type] = true;

        }
    }
}
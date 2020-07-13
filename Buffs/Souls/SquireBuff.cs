using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Souls
{
    public class SquireBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Power of Squire");
            Description.SetDefault("Removing some enemy immunity frames");
            Main.buffNoSave[Type] = true;
        }
    }
}
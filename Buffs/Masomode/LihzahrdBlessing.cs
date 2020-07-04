using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Masomode
{
    public class LihzahrdBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lihzahrd Blessing");
            Description.SetDefault("Wires enabled in Jungle Temple");
            canBeCleared = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderBuff";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[ModContent.BuffType<LihzahrdCurse>()] = true;
        }
    }
}
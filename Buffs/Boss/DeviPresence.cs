using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Boss
{
    public class DeviPresence : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Deviant Presence");
            Description.SetDefault("Reduced defense and friendly NPCs take massively increased damage");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FargoPlayer>().DevianttPresence = true;
        }
    }
}
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs
{
    public class AbomPresence : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Abominable Presence");
            Description.SetDefault("Defense, damage reduction, and life regen reduced; Moon Leech effect; Chaos State effect");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
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
            player.GetModPlayer<FargoPlayer>().noDodge = true;
            player.GetModPlayer<FargoPlayer>().noSupersonic = true;
            player.moonLeech = true;
            player.chaosState = true;
            player.bleed = true;

            player.statDefense -= 15;
            player.endurance -= 0.15f;
        }
    }
}
using FargowiltasSouls.NPCs;
using FargowiltasSouls.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Souls
{
    public class SquireKBDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Squire KB");
            Main.buffNoSave[Type] = true;
            canBeCleared = false;
            Main.debuff[Type] = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";
            return true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            //after 1 second, revert KB, cooldown for rest of debuff
            if (npc.buffTime[buffIndex] < 900 && npc.knockbackResist == 1f)
            {
                npc.knockbackResist = npc.GetGlobalNPC<FargoSoulsGlobalNPC>().originalKB;
            }
        }
    }
}
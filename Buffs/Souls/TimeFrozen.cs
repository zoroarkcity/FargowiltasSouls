using FargowiltasSouls.NPCs;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Buffs.Souls
{
    public class TimeFrozen : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Time Frozen");
            Description.SetDefault("You are stopped in time");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
            DisplayName.AddTranslation(GameCulture.Chinese, "时间冻结");
            Description.AddTranslation(GameCulture.Chinese, "你停止了时间");
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";

            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.controlLeft = false;
            player.controlRight = false;
            player.controlJump = false;
            player.controlDown = false;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlHook = false;
            player.controlMount = false;
            player.velocity = player.oldVelocity;
            player.position = player.oldPosition;

            if (!Filters.Scene["FargowiltasSouls:TimeStop"].IsActive())
                Filters.Scene.Activate("FargowiltasSouls:TimeStop");
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().TimeFrozen = true;
        }
    }
}
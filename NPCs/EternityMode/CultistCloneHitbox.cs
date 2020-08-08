using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.NPCs.EternityMode
{
    public class CultistCloneHitbox : ModNPC
    {
        public override string Texture => "Terraria/NPC_440";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunatic Cultist");
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 50;
            npc.damage = 0;
            npc.lifeMax = 10000;
            npc.HitSound = SoundID.NPCHit55;
            npc.DeathSound = SoundID.NPCDeath59;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.hide = true;
            npc.lavaImmune = true;
            for (int i = 0; i < npc.buffImmune.Length; i++)
                npc.buffImmune[i] = true;
            npc.aiStyle = -1;
        }

        public override void AI()
        {
            int ai0 = (int)npc.ai[0];
            if (!(ai0 > -1 && ai0 < Main.maxNPCs && Main.npc[ai0].active && Main.npc[ai0].type == NPCID.CultistBossClone))
            {
                npc.active = false;
                return;
            }

            int ai1 = (int)npc.ai[1];
            if (!(ai1 > -1 && ai1 < Main.maxNPCs && Main.npc[ai1].active && Main.npc[ai1].type == NPCID.CultistBoss))
            {
                npc.active = false;
                return;
            }

            npc.Center = Main.npc[ai0].Center; //follow clone
            npc.defense = Main.npc[ai1].defense;
            npc.life = Main.npc[ai1].life;
            npc.realLife = ai1;
            if (Main.npc[ai1].immune[Main.myPlayer] < npc.immune[Main.myPlayer]) //sync iframes
                Main.npc[ai1].immune[Main.myPlayer] = npc.immune[Main.myPlayer];
            else
                npc.immune[Main.myPlayer] = Main.npc[ai1].immune[Main.myPlayer];
            npc.dontTakeDamage = Main.npc[ai1].dontTakeDamage;
            if (Main.npc[ai1].ai[3] == -1)
                npc.dontTakeDamage = true;
        }

        public override bool CheckDead()
        {
            int ai0 = (int)npc.ai[0];
            if (!(ai0 > -1 && ai0 < Main.maxNPCs && Main.npc[ai0].active && Main.npc[ai0].type == NPCID.CultistBossClone))
                return true;

            int ai1 = (int)npc.ai[1];
            if (!(ai1 > -1 && ai1 < Main.maxNPCs && Main.npc[ai1].active && Main.npc[ai1].type == NPCID.CultistBoss))
                return true;

            return false;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace FargowiltasSouls.NPCs
{
    public class Echdeath : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echdeath");
            Main.npcFrameCount[npc.type] = 11;
        }

        public override void SetDefaults()
        {
            npc.width = 86;
            npc.height = 78;
            npc.damage = int.MaxValue / 10;
            npc.defense = int.MaxValue / 10;
            npc.lifeMax = int.MaxValue / 10;
            if (Main.expertMode)
            {
                npc.damage /= 2;
                npc.lifeMax /= 2;
            }
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            for (int i = 0; i < npc.buffImmune.Length; i++)
                npc.buffImmune[i] = true;
            npc.aiStyle = -1;
            npc.boss = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = mod.ItemType("Sadism");
        }

        public override void AI()
        {
            if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
            {
                Main.LocalPlayer.ResetEffects();
                Main.LocalPlayer.KillMe(PlayerDeathReason.ByNPC(npc.whoAmI), npc.damage, 0);
                for (int i = 0; i < 100; i++)
                    CombatText.NewText(Main.LocalPlayer.Hitbox, Color.Red, Main.rand.Next(npc.damage), true);
                Main.NewText(":echdeath:", Color.Red);
            }
            npc.active = false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter > 1)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= Main.npcFrameCount[npc.type] * frameHeight)
                    npc.frame.Y = 0;
            }
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage = 0;
            crit = false;
            return false;
        }

        public override bool CheckDead()
        {
            npc.life = npc.lifeMax;
            npc.active = true;
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override void NPCLoot()
        {
            Main.NewText("HOW", Color.Red);
        }
    }
}
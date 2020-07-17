using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class QueenFlocko : AbomBoss.AbomFlocko
    {
        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            if (projectile.ai[0] < 0 || projectile.ai[0] >= Main.maxNPCs)
            {
                projectile.Kill();
                return;
            }

            NPC npc = Main.npc[(int)projectile.ai[0]];
            if (!(npc.active && npc.type == NPCID.IceQueen && npc.ai[0] != 2))
            {
                projectile.Kill();
                return;
            }

            projectile.timeLeft = 2;

            Player player = Main.player[npc.target];

            Vector2 target = player.Center;
            target.X += 700 * projectile.ai[1];

            Vector2 distance = target - projectile.Center;
            float length = distance.Length();
            if (length > 100f)
            {
                distance /= 8f;
                projectile.velocity = (projectile.velocity * 23f + distance) / 24f;
            }
            else
            {
                if (projectile.velocity.Length() < 12f)
                    projectile.velocity *= 1.05f;
            }

            if (++projectile.localAI[0] > 90 && ++projectile.localAI[1] > 60) //fire frost wave
            {
                projectile.localAI[1] = 0f;
                Main.PlaySound(SoundID.Item120, projectile.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 vel = projectile.DirectionTo(player.Center) * 7f;
                    Projectile.NewProjectile(projectile.Center, vel, ProjectileID.FrostWave, projectile.damage, projectile.knockBack, projectile.owner);
                }
            }

            projectile.rotation += projectile.velocity.Length() / 12f * (projectile.velocity.X > 0 ? -0.2f : 0.2f);
            if (++projectile.frameCounter > 3)
            {
                if (++projectile.frame >= 6)
                    projectile.frame = 0;
                projectile.frameCounter = 0;
            }
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.AbomBoss
{
    public class AbomFlocko : ModProjectile
    {
        public override string Texture => "Terraria/NPC_352";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Flocko");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.timeLeft = 420;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.penetrate = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            if (projectile.ai[0] < 0 || projectile.ai[0] >= Main.maxNPCs)
            {
                projectile.Kill();
                return;
            }

            NPC npc = Main.npc[(int)projectile.ai[0]];

            Vector2 target = npc.Center;
            target.X += 1100 * (float)Math.Sin(2 * Math.PI / 180 * projectile.ai[1]++);
            target.Y -= 1100;

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

            /*projectile.localAI[0]++;
            if (projectile.localAI[0] > 45)
            {
                projectile.localAI[0] = 0f;
                if (projectile.owner == Main.myPlayer)
                {
                    Vector2 vel = distance;
                    vel.Normalize();
                    vel *= 9f;
                    Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("FrostWave"),
                        projectile.damage, projectile.knockBack, projectile.owner);
                }
            }*/
            
            if (++projectile.localAI[0] > 120 && ++projectile.localAI[1] > 6) //spray shards
            {
                projectile.localAI[1] = 0f;
                if (Main.netMode != 1 && Math.Abs(npc.Center.X - projectile.Center.X) > 200)
                {
                    Vector2 speed = new Vector2(Main.rand.Next(-1000, 1001), Main.rand.Next(-1000, 1001));
                    speed.Normalize();
                    speed *= 8f;
                    Projectile.NewProjectile(projectile.Center + speed * 4f, speed, mod.ProjectileType("AbomFrostShard"), projectile.damage, projectile.knockBack, projectile.owner);
                    Projectile.NewProjectile(projectile.Center + Vector2.UnitY * 8f, Vector2.UnitY * 8f, mod.ProjectileType("AbomFrostShard"), projectile.damage, projectile.knockBack, projectile.owner);
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("MutantFang"), 180);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 200);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
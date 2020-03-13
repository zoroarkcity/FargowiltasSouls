using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviButterfly : ModProjectile
    {
        public bool drawLoaded;
        public int drawBase;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Butterfly");
            Main.projFrames[projectile.type] = 24;
        }

        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.timeLeft = 420;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;

            projectile.scale = 3f;
            projectile.hide = true;
        }

        public override void AI()
        {
            if (projectile.ai[0] < 0 || projectile.ai[0] >= Main.maxNPCs)
            {
                projectile.Kill();
                return;
            }

            if (!drawLoaded)
            {
                drawLoaded = true;
                drawBase = Main.rand.Next(8);
                projectile.hide = false;
            }

            NPC npc = Main.npc[(int)projectile.ai[0]];

            Vector2 target = npc.Center;
            target.X += 1100 * (float)Math.Sin(2 * Math.PI / 720 * projectile.ai[1]++);
            target.Y -= 900;

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
            
            if (++projectile.localAI[0] > 60) //spray shards
            {
                if (projectile.localAI[0] > (npc.localAI[3] > 1 ? 120 : 90))
                {
                    projectile.localAI[0] = 0;
                }

                if (++projectile.localAI[1] > 5)
                {
                    projectile.localAI[1] = 0;

                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.UnitY * 5, mod.ProjectileType("DeviLightBall2"),
                            projectile.damage, projectile.knockBack, projectile.owner);
                    }
                }

                /*Main.PlaySound(SoundID.Item27, projectile.position);
                projectile.localAI[1] = 0f;
                if (Main.netMode != 1)
                {
                    if (Math.Abs(npc.Center.X - projectile.Center.X) > (npc.localAI[3] > 1 ? 300 : 450))
                    {
                        Vector2 speed = new Vector2(Main.rand.Next(-1000, 1001), Main.rand.Next(-1000, 1001));
                        speed.Normalize();
                        speed *= 8f;
                        Projectile.NewProjectile(projectile.Center + speed * 4f, speed, mod.ProjectileType("AbomFrostShard"), projectile.damage, projectile.knockBack, projectile.owner);
                        Projectile.NewProjectile(projectile.Center + Vector2.UnitY * 8f, Vector2.UnitY * 8f, mod.ProjectileType("AbomFrostShard"), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                    if (Main.player[npc.target].active && !Main.player[npc.target].dead && Main.player[npc.target].Center.Y < projectile.Center.Y)
                    {
                        Main.PlaySound(SoundID.Item120, projectile.position);
                        if (Main.netMode != 1)
                        {
                            Vector2 vel = projectile.DirectionTo(Main.player[npc.target].Center + new Vector2(Main.rand.Next(-200, 201), Main.rand.Next(-200, 201))) * 12f;
                            Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("AbomFrostWave"), projectile.damage, projectile.knockBack, projectile.owner);
                        }
                    }
                }*/
            }

            if (projectile.frame < drawBase)
                projectile.frame = drawBase;

            if (++projectile.frameCounter > 6)
            {
                projectile.frameCounter = 0;

                if (++projectile.frame >= drawBase + 3)
                    projectile.frame = drawBase;
            }
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 86, 0f, 0f, 0, default(Color), 2.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 8f;
            }
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
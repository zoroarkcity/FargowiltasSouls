using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class RainCloud : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_238";

        private int[] wetProj = { ProjectileID.Kraken, ProjectileID.Trident, ProjectileID.Flairon, ProjectileID.FlaironBubble, ProjectileID.WaterStream, ProjectileID.WaterBolt, ProjectileID.RainNimbus, ProjectileID.Bubble, ProjectileID.WaterGun };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain Cloud");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.RainCloudRaining);
            aiType = ProjectileID.RainCloudRaining;
            projectile.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;

            projectile.timeLeft = 600;

            Main.projFrames[projectile.type] = 6;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (projectile.timeLeft < 540)
            {
                projectile.velocity = Vector2.Zero;
            }

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.owner == player.whoAmI && Array.IndexOf(wetProj, proj.type) > -1 && proj.Hitbox.Intersects(projectile.Hitbox))
                {
                    if (projectile.scale < 3f)
                    {
                        projectile.scale *= 1.05f;
                        projectile.timeLeft += 60;
                    }
                    else
                    {
                        Vector2 rotationVector2 = (proj.Center + proj.velocity * 25) - projectile.Center;
                        rotationVector2.Normalize();

                        Vector2 vector2_3 = rotationVector2 * 8f;
                        float ai_1 = Main.rand.Next(80);
                        Projectile.NewProjectile(projectile.Center.X - vector2_3.X, projectile.Center.Y - vector2_3.Y, vector2_3.X, vector2_3.Y,
                            mod.ProjectileType("LightningArc"), projectile.damage * 2, projectile.knockBack, projectile.owner,
                            rotationVector2.ToRotation(), ai_1);
                        
                    }

                    proj.active = false;

                    break;
                }
            }

            //cancel normal rain
            projectile.ai[0] = 0;

            projectile.localAI[1]++;

            //bigger = more rain
            if (projectile.scale > 3f)
            {
                projectile.localAI[1] += 8;
            }
            else if (projectile.scale > 2f)
            {
                projectile.localAI[1] += 4;
            }
            else if (projectile.scale > 1.5f)
            {
                projectile.localAI[1] += 2;
            }
            else
            {
                projectile.localAI[1]++;
            }

            //do the rain
            if (projectile.localAI[1] >= 8)
            {
                projectile.localAI[1] = 0;

                int num414 = (int)(projectile.Center.X + (float)Main.rand.Next((int)(-20 * projectile.scale), (int)(20 * projectile.scale)));
                int num415 = (int)(projectile.position.Y + (float)projectile.height + 4f);
                Projectile.NewProjectile((float)num414, (float)num415, 0f, 5f, 239, projectile.damage / 2, 0f, projectile.owner, 0f, 0f);
            }
        }
    }
}
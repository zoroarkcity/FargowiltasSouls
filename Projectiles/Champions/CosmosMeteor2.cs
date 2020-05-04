using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class CosmosMeteor2 : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_424";
            
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Meteor");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Meteor1);
            projectile.aiStyle = -1;
            projectile.magic = false;
            projectile.friendly = false;
            projectile.hostile = true;
            
            projectile.extraUpdates = 0;
            cooldownSlot = 1;

            projectile.GetGlobalProjectile<FargoGlobalProjectile>().TimeFreezeImmune = true;
            projectile.GetGlobalProjectile<FargoGlobalProjectile>().ImmuneToMutantBomb = true;

            projectile.scale = 10f;
        }

        public override bool CanHitPlayer(Player target)
        {
            return projectile.Distance(target.Center) < projectile.width * projectile.scale;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;

                Main.PlaySound(SoundID.Item92, projectile.Center);

                projectile.rotation += (float)Math.PI / 2;
                if (projectile.ai[0] < 0)
                    projectile.rotation += (float)Math.PI;

                Vector2 size = new Vector2(500, 500);
                Vector2 spawnPos = projectile.Center;
                spawnPos.X -= size.X / 2;
                spawnPos.Y -= size.Y / 2;

                for (int num615 = 0; num615 < 45; num615++)
                {
                    int num616 = Dust.NewDust(spawnPos, (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num616].velocity *= 1.4f;
                }

                for (int num617 = 0; num617 < 60; num617++)
                {
                    int num618 = Dust.NewDust(spawnPos, (int)size.X, (int)size.Y, DustID.Fire, 0f, 0f, 100, default(Color), 3.5f);
                    Main.dust[num618].noGravity = true;
                    Main.dust[num618].velocity *= 7f;
                    num618 = Dust.NewDust(spawnPos, (int)size.X, (int)size.Y, DustID.Fire, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num618].velocity *= 3f;
                }

                for (int num619 = 0; num619 < 3; num619++)
                {
                    float scaleFactor9 = 0.4f;
                    if (num619 == 1) scaleFactor9 = 0.8f;
                    int num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[num620].velocity *= scaleFactor9;
                    Gore gore97 = Main.gore[num620];
                    gore97.velocity.X = gore97.velocity.X + 1f;
                    Gore gore98 = Main.gore[num620];
                    gore98.velocity.Y = gore98.velocity.Y + 1f;
                    num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[num620].velocity *= scaleFactor9;
                    Gore gore99 = Main.gore[num620];
                    gore99.velocity.X = gore99.velocity.X - 1f;
                    Gore gore100 = Main.gore[num620];
                    gore100.velocity.Y = gore100.velocity.Y + 1f;
                    num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[num620].velocity *= scaleFactor9;
                    Gore gore101 = Main.gore[num620];
                    gore101.velocity.X = gore101.velocity.X + 1f;
                    Gore gore102 = Main.gore[num620];
                    gore102.velocity.Y = gore102.velocity.Y - 1f;
                    num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[num620].velocity *= scaleFactor9;
                    Gore gore103 = Main.gore[num620];
                    gore103.velocity.X = gore103.velocity.X - 1f;
                    Gore gore104 = Main.gore[num620];
                    gore104.velocity.Y = gore104.velocity.Y - 1f;
                }


                for (int k = 0; k < 40; k++) //make visual dust
                {
                    Vector2 dustPos = projectile.position;
                    dustPos.X += Main.rand.Next(projectile.width);
                    dustPos.Y += Main.rand.Next(projectile.height);

                    for (int i = 0; i < 30; i++)
                    {
                        int dust = Dust.NewDust(dustPos, 32, 32, 31, 0f, 0f, 100, default(Color), 3f);
                        Main.dust[dust].velocity *= 1.4f;
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        int dust = Dust.NewDust(dustPos, 32, 32, DustID.Fire, 0f, 0f, 100, default(Color), 3.5f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 7f;
                        dust = Dust.NewDust(dustPos, 32, 32, DustID.Fire, 0f, 0f, 100, default(Color), 1.5f);
                        Main.dust[dust].velocity *= 3f;
                    }

                    float scaleFactor9 = 0.5f;
                    for (int j = 0; j < 4; j++)
                    {
                        int gore = Gore.NewGore(dustPos, default(Vector2), Main.rand.Next(61, 64));
                        Main.gore[gore].velocity *= scaleFactor9;
                        Main.gore[gore].velocity.X += 1f;
                        Main.gore[gore].velocity.Y += 1f;
                    }
                }


                const int num226 = 80;
                for (int num227 = 0; num227 < num226; num227++)
                {
                    Vector2 vector6 = Vector2.UnitX * 40f;
                    vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + projectile.Center;
                    Vector2 vector7 = vector6 - projectile.Center;
                    int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Fire, 0f, 0f, 0, default(Color), 3f);
                    Main.dust[num228].noGravity = true;
                    Main.dust[num228].velocity = vector7;
                }
            }

            int ai1 = (int)projectile.ai[1];
            if (!(ai1 > -1 && ai1 < Main.maxNPCs && Main.npc[ai1].active && Main.npc[ai1].type == ModContent.NPCType<NPCs.Champions.CosmosChampion>()))
            {
                projectile.Kill();
                return;
            }

            projectile.timeLeft = 2;
            
            const float maxAmplitude = 1000;
            const float minDistance = 200;
            float offset = Math.Abs(maxAmplitude * (float)Math.Sin(Main.npc[ai1].ai[2] * 2 * (float)Math.PI / 300));
            if (offset < minDistance)
            {
                offset = minDistance - (minDistance - offset) / 2;
            }
            projectile.rotation += 0.01f;
            projectile.Center = Main.npc[ai1].Center + offset * projectile.rotation.ToRotationVector2();
        }

        public override void Kill(int timeLeft) //vanilla explosion code echhhhhhhhhhh
        {
            Main.PlaySound(SoundID.Item89, projectile.position);

            Vector2 size = new Vector2(500, 500);
            Vector2 spawnPos = projectile.Center;
            spawnPos.X -= size.X / 2;
            spawnPos.Y -= size.Y / 2;

            for (int num615 = 0; num615 < 45; num615++)
            {
                int num616 = Dust.NewDust(spawnPos, (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num616].velocity *= 1.4f;
            }

            for (int num617 = 0; num617 < 60; num617++)
            {
                int num618 = Dust.NewDust(spawnPos, (int)size.X, (int)size.Y, DustID.Fire, 0f, 0f, 100, default(Color), 3.5f);
                Main.dust[num618].noGravity = true;
                Main.dust[num618].velocity *= 7f;
                num618 = Dust.NewDust(spawnPos, (int)size.X, (int)size.Y, DustID.Fire, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num618].velocity *= 3f;
            }

            for (int num619 = 0; num619 < 3; num619++)
            {
                float scaleFactor9 = 0.4f;
                if (num619 == 1) scaleFactor9 = 0.8f;
                int num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore97 = Main.gore[num620];
                gore97.velocity.X = gore97.velocity.X + 1f;
                Gore gore98 = Main.gore[num620];
                gore98.velocity.Y = gore98.velocity.Y + 1f;
                num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore99 = Main.gore[num620];
                gore99.velocity.X = gore99.velocity.X - 1f;
                Gore gore100 = Main.gore[num620];
                gore100.velocity.Y = gore100.velocity.Y + 1f;
                num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore101 = Main.gore[num620];
                gore101.velocity.X = gore101.velocity.X + 1f;
                Gore gore102 = Main.gore[num620];
                gore102.velocity.Y = gore102.velocity.Y - 1f;
                num620 = Gore.NewGore(projectile.Center, default(Vector2), Main.rand.Next(61, 64));
                Main.gore[num620].velocity *= scaleFactor9;
                Gore gore103 = Main.gore[num620];
                gore103.velocity.X = gore103.velocity.X - 1f;
                Gore gore104 = Main.gore[num620];
                gore104.velocity.Y = gore104.velocity.Y - 1f;
            }


            for (int k = 0; k < 40; k++) //make visual dust
            {
                Vector2 dustPos = projectile.position;
                dustPos.X += Main.rand.Next(projectile.width);
                dustPos.Y += Main.rand.Next(projectile.height);

                for (int i = 0; i < 30; i++)
                {
                    int dust = Dust.NewDust(dustPos, 32, 32, 31, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dust].velocity *= 1.4f;
                }

                for (int i = 0; i < 20; i++)
                {
                    int dust = Dust.NewDust(dustPos, 32, 32, DustID.Fire, 0f, 0f, 100, default(Color), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 7f;
                    dust = Dust.NewDust(dustPos, 32, 32, DustID.Fire, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[dust].velocity *= 3f;
                }

                float scaleFactor9 = 0.5f;
                for (int j = 0; j < 4; j++)
                {
                    int gore = Gore.NewGore(dustPos, default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[gore].velocity *= scaleFactor9;
                    Main.gore[gore].velocity.X += 1f;
                    Main.gore[gore].velocity.Y += 1f;
                }
            }


            const int num226 = 80;
            for (int num227 = 0; num227 < num226; num227++)
            {
                Vector2 vector6 = Vector2.UnitX * 40f;
                vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + projectile.Center;
                Vector2 vector7 = vector6 - projectile.Center;
                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Fire, 0f, 0f, 0, default(Color), 3f);
                Main.dust[num228].noGravity = true;
                Main.dust[num228].velocity = vector7;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.BrokenArmor, 300);
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * projectile.Opacity;
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
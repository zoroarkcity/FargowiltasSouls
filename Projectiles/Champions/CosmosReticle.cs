using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class CosmosReticle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Reticle");
        }

        public override void SetDefaults()
        {
            projectile.width = 110;
            projectile.height = 110;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.hostile = true;
            
            //cooldownSlot = 1;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            int ai0 = (int)projectile.ai[0];
            if (!(ai0 > -1 && ai0 < Main.maxNPCs && Main.npc[ai0].active
                && Main.npc[ai0].type == ModContent.NPCType<NPCs.Champions.CosmosChampion>() && Main.npc[ai0].ai[0] == 11))
            {
                projectile.Kill();
                return;
            }

            NPC npc = Main.npc[ai0];
            Player player = Main.player[npc.target];

            projectile.velocity = Vector2.Zero;

            if (++projectile.ai[1] > 45)
            {
                if (projectile.ai[1] % 5 == 0)
                {
                    Main.PlaySound(SoundID.Item89, projectile.Center);

                    if (Main.netMode != 1) //rain meteors
                    {
                        Vector2 spawnPos = projectile.Center;
                        spawnPos.X += Main.rand.Next(-200, 201);
                        spawnPos.Y -= 700;
                        Vector2 vel = Main.rand.NextFloat(10, 15f) * Vector2.Normalize(projectile.Center - spawnPos);
                        Projectile.NewProjectile(spawnPos, vel, ModContent.ProjectileType<CosmosMeteor>(), npc.damage / 4, 0f, Main.myPlayer, 0f, Main.rand.NextFloat(1f, 2f));
                    }
                }

                if (projectile.ai[1] > 90)
                {
                    projectile.ai[1] = 0;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                projectile.Center = player.Center;
                projectile.position.X += player.velocity.X * 30;

                if (projectile.ai[1] == 45)
                {
                    projectile.netUpdate = true;

                    const int num226 = 80;
                    for (int num227 = 0; num227 < num226; num227++)
                    {
                        Vector2 vector6 = Vector2.UnitX * 15f;
                        vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + projectile.Center;
                        Vector2 vector7 = vector6 - projectile.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, 242, 0f, 0f, 0, default(Color), 3f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].velocity = vector7;
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 128) * (1f - projectile.alpha / 255f);
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
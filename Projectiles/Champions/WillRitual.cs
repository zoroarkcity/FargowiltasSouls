using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.NPCs.Champions;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class WillRitual : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_467";

        private const float PI = (float)Math.PI;
        private const float rotationPerTick = PI / 140f;
        private const float threshold = 1200f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will Seal");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 42;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<FargoGlobalProjectile>().TimeFreezeImmune = true;
        }

        public override void AI()
        {
            int ai1 = (int)projectile.ai[1];
            if (projectile.ai[1] >= 0f && projectile.ai[1] < Main.maxNPCs
                && Main.npc[ai1].active && Main.npc[ai1].type == ModContent.NPCType<WillChampion>()
                && projectile.localAI[0] == 0) //disappear if champ is setting a new arena
            {
                projectile.alpha -= 5;
                if (projectile.alpha < 0)
                    projectile.alpha = 0;

                //projectile.Center = Main.npc[ai1].Center;

                if ((Main.npc[ai1].ai[0] == 2 && Main.npc[ai1].ai[1] < 30)
                    || (Main.npc[ai1].ai[0] == -1 && Main.npc[ai1].ai[1] < 10))
                    projectile.localAI[0] = 1;

                Player target = Main.player[Main.myPlayer];
                if (target.active && !target.dead)
                {
                    float distance = target.Distance(projectile.Center);
                    if (Math.Abs(distance - threshold) < 46f && target.hurtCooldowns[0] == 0 && projectile.alpha == 0 && target.whoAmI == Main.npc[ai1].target)
                    {
                        int hitDirection = projectile.Center.X > target.Center.X ? 1 : -1;
                        target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, projectile.whoAmI),
                            Main.npc[ai1].damage / 2, hitDirection, false, false, false, 0);
                        if (FargoSoulsWorld.MasochistMode)
                        {
                            target.AddBuff(ModContent.BuffType<Defenseless>(), 300);
                            target.AddBuff(ModContent.BuffType<Midas>(), 300);
                        }
                        target.AddBuff(BuffID.Bleeding, 300);
                    }
                    if (distance > threshold && distance < threshold * 5f)
                    {
                        if (distance > threshold * 2f)
                        {
                            target.frozen = true;
                            target.controlHook = false;
                            target.controlUseItem = false;
                            if (target.mount.Active)
                                target.mount.Dismount(target);
                            target.velocity.X = 0f;
                            target.velocity.Y = -0.4f;
                        }

                        Vector2 movement = projectile.Center - target.Center;
                        float difference = movement.Length() - threshold;
                        movement.Normalize();
                        movement *= difference < 17f ? difference : 17f;
                        target.position += movement;

                        for (int i = 0; i < 20; i++)
                        {
                            int d = Dust.NewDust(target.position, target.width, target.height, 87, 0f, 0f, 0, default(Color), 2f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 5f;
                        }
                    }
                }
            }
            else
            {
                projectile.velocity = Vector2.Zero;
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.Kill();
                    return;
                }
            }

            projectile.timeLeft = 2;
            projectile.scale = (1f - projectile.alpha / 255f) * 2f;
            projectile.ai[0] += rotationPerTick;
            if (projectile.ai[0] > PI)
            {
                projectile.ai[0] -= 2f * PI;
                projectile.netUpdate = true;
            }

            projectile.rotation += 0.77f;
            
            if (++projectile.frameCounter > 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame > 3)
                    projectile.frame = 0;
            }
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = projectile.GetAlpha(lightColor);

            for (int x = 0; x < 32; x++)
            {
                Vector2 drawOffset = new Vector2(threshold * projectile.scale / 2f, 0f).RotatedBy(projectile.ai[0]);
                drawOffset = drawOffset.RotatedBy(2f * PI / 32f * x);
                const int max = 4;
                for (int i = 0; i < max; i++)
                {
                    Color color27 = color26;
                    color27 *= (float)(max - i) / max;
                    Vector2 value4 = projectile.Center + drawOffset.RotatedBy(rotationPerTick * -i);
                    float num165 = projectile.oldRot[i];
                    Main.spriteBatch.Draw(texture2D13, value4 - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, projectile.scale, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture2D13, projectile.Center + drawOffset - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * projectile.Opacity;
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.MutantBoss
{
    public class MutantScythe1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutant Sickle");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.alpha = 100;
            projectile.light = 0.2f;
            projectile.hostile = true;
            projectile.timeLeft = 90;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            /*NPC mutant = Main.npc[(int)projectile.ai[0]];
            if (!mutant.active || mutant.type != mod.NPCType("MutantBoss"))
            {
                projectile.Kill();
                return;
            }*/
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                Main.PlaySound(SoundID.Item8, projectile.Center);
            }
            projectile.rotation += 0.8f;
            for (int i = 0; i < 2; i++)
            {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, 0f, 0f, 100);
                Main.dust[d].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
                for (int i = 0; i < 8; i++)
                    Projectile.NewProjectile(projectile.Center, Vector2.UnitX.RotatedBy(Math.PI / 4 * i), mod.ProjectileType("MutantScythe2"), projectile.damage, 0f, projectile.owner);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = projectile.GetAlpha(color26);

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
            {
                Color color27 = color26;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[projectile.type];
                Vector2 value4 = projectile.oldPos[i];
                float num165 = projectile.oldRot[i];
                Main.spriteBatch.Draw(texture2D13, value4 + projectile.Size / 2f - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (NPCs.EModeGlobalNPC.BossIsAlive(ref NPCs.EModeGlobalNPC.mutantBoss, mod.NPCType("MutantBoss")))
                target.AddBuff(mod.BuffType("MutantFang"), 180);
            target.AddBuff(mod.BuffType("AbomFang"), 300);
            target.AddBuff(mod.BuffType("Shadowflame"), 300);
            target.AddBuff(BuffID.Bleeding, 600);
        }
    }
}
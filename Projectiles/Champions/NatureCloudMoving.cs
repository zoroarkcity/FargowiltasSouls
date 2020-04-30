using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class NatureCloudMoving : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_237";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature Cloud");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.timeLeft = 40;
            projectile.tileCollide = false;
            
            cooldownSlot = 1;
        }

        public override void AI()
        {
            projectile.rotation = projectile.rotation + projectile.velocity.X * 0.02f;
            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame > 3)
                    projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<NatureCloudRaining>(), projectile.damage, 0f, Main.myPlayer);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 300);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
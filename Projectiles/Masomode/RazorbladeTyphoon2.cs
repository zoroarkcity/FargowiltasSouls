using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class RazorbladeTyphoon2 : RazorbladeTyphoon
    {
        public override string Texture => "Terraria/Projectile_409";
        
        public override void AI()
        {
            if (++projectile.localAI[1] > 30 && projectile.localAI[1] < 120)
                projectile.velocity *= 1.025f;

            //vanilla typhoon dust (ech)
            int cap = Main.rand.Next(3);
            for (int index1 = 0; index1 < cap; ++index1)
            {
                Vector2 vector2_1 = projectile.velocity;
                vector2_1.Normalize();
                vector2_1.X *= projectile.width;
                vector2_1.Y *= projectile.height;
                vector2_1 /= 2;
                vector2_1 = vector2_1.RotatedBy((index1 - 2) * Math.PI / 6);
                vector2_1 += projectile.Center;
                Vector2 vector2_2 = (Main.rand.NextFloat() * (float)Math.PI - (float)Math.PI / 2f).ToRotationVector2();
                vector2_2 *= Main.rand.Next(3, 8);
                int index2 = Dust.NewDust(vector2_1 + vector2_2, 0, 0, 172, vector2_2.X * 2f, vector2_2.Y * 2f, 100, new Color(), 1.4f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = true;
                Main.dust[index2].velocity /= 4f;
                Main.dust[index2].velocity -= projectile.velocity;
            }
            projectile.rotation += 0.2f * (projectile.velocity.X > 0f ? 1f : -1f);
            projectile.frame++;
            if (projectile.frame > 2)
                projectile.frame = 0;
        }
    }
}
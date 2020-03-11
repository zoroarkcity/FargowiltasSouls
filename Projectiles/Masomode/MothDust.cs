using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class MothDust : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Projectiles/Explosion";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moth Dust");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = -1;
            projectile.hide = true;
            projectile.hostile = true;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
            projectile.velocity *= .96f;

            int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 70);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity *= 2.5f;

            Lighting.AddLight(projectile.position, .3f, .1f, .3f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsGlobalNPC.BossIsAlive(ref FargoSoulsGlobalNPC.deviBoss, mod.NPCType("DeviBoss")))
            {
                if (Main.rand.Next(6) == 0)
                    target.AddBuff(mod.BuffType("Berserked"), 240);
                if (Main.rand.Next(6) == 0)
                    target.AddBuff(mod.BuffType("Defenseless"), 240);
                if (Main.rand.Next(6) == 0)
                    target.AddBuff(mod.BuffType("Guilty"), 240);
                if (Main.rand.Next(6) == 0)
                    target.AddBuff(mod.BuffType("Lovestruck"), 240);
                if (Main.rand.Next(6) == 0)
                    target.AddBuff(mod.BuffType("MutantNibble"), 240);
                if (Main.rand.Next(6) == 0)
                    target.AddBuff(mod.BuffType("Rotting"), 240);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    int d = Main.rand.Next(Fargowiltas.DebuffIDs.Count);
                    target.AddBuff(Fargowiltas.DebuffIDs[d], 240);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
                projectile.velocity.X = -oldVelocity.X;
            if (projectile.velocity.Y != oldVelocity.Y)
                projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
    }
}
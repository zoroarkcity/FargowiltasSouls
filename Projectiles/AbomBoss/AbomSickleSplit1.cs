using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.AbomBoss
{
    public class AbomSickleSplit1 : MutantBoss.MutantScythe1
    {
        public override string Texture => "FargowiltasSouls/Projectiles/AbomBoss/AbomSickle";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abominationn Sickle");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 8; i++)
                    Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 4 * i),
                        ModContent.ProjectileType<AbomSickleSplit2>(), projectile.damage, 0f, projectile.owner);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
            {
                target.AddBuff(mod.BuffType("AbomFang"), 300);
                target.AddBuff(mod.BuffType("Berserked"), 120);
            }
            target.AddBuff(BuffID.Bleeding, 600);
        }
    }
}
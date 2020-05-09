using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class NatureFireball : WillFireball
    {
        public override string Texture => "Terraria/Projectile_711";

        public override void SetDefaults()
        {
            base.SetDefaults();
            cooldownSlot = 1;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            base.AI();
            if (!projectile.tileCollide && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                projectile.tileCollide = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
                target.AddBuff(BuffID.Burning, 300);
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
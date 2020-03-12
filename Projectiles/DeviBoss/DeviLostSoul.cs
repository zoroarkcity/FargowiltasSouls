using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviLostSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lost Soul");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.LostSoulHostile);
            aiType = ProjectileID.LostSoulHostile;

            cooldownSlot = 1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Hexed"), 240);
            target.AddBuff(mod.BuffType("ReverseManaFlow"), 600);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class NatureRain : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_239";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature Rain");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.RainNimbus);
            aiType = ProjectileID.RainNimbus;
            cooldownSlot = 1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 300);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class OpticLaser : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_100"; 

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Laser");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DeathLaser);
            aiType = ProjectileID.DeathLaser;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.magic = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.timeLeft = 120 * (projectile.extraUpdates + 1);

            /*projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;*/
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 6;
            target.AddBuff(BuffID.Ichor, 600);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!Main.player[projectile.owner].HeldItem.summon)
                damage /= 4;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
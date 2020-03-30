using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class OpticFlame : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_101"; 

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye Fire");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.EyeFire);
            aiType = ProjectileID.EyeFire;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.magic = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;

            /*projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;*/
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 6;
            target.AddBuff(BuffID.CursedInferno, 600);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!Main.player[projectile.owner].HeldItem.summon)
                damage /= 4;
        }
    }
}
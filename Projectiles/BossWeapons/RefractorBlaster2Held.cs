using FargowiltasSouls.Projectiles.Minions;
using IL.Terraria.Chat.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
	public class RefractorBlaster2Held : ModProjectile
	{
		public override string Texture => "FargowiltasSouls/Items/Weapons/SwarmDrops/RefractorBlaster2";
		public override void SetDefaults()
		{
			projectile.width = 76;
			projectile.height = 38;
			//projectile.aiStyle = 136;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			//projectile.usesLocalNPCImmunity = true;
			//projectile.localNPCHitCooldown = 8;
			projectile.tileCollide = false;
			projectile.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
		}

		public int timer;
        public float lerp = 0.12f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refractor Blaster Ex");
		}
		
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (Main.myPlayer != player.whoAmI)
				return;

			if (player.dead || !player.active)
				projectile.Kill();

			Vector2 center = player.MountedCenter;
			projectile.Center = center;
			projectile.rotation = projectile.velocity.ToRotation();
			Vector2 HoldOffset = new Vector2(projectile.width/4, 0).RotatedBy(projectile.velocity.ToRotation());
			projectile.Center += HoldOffset;

			if (player.channel)
			{
				projectile.velocity = Vector2.Lerp(Vector2.Normalize(projectile.velocity),
					Vector2.Normalize(Main.MouseWorld - player.MountedCenter), lerp); //slowly move towards direction of cursor
				projectile.velocity.Normalize();

				timer++;
				if (timer % 20 == 0)
				{
					Main.PlaySound(player.inventory[player.selectedItem].UseSound, projectile.Center);
					bool checkmana = player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
					if (!checkmana)
						projectile.Kill();

				}
				if (timer > 60)
				{
					int type = ModContent.ProjectileType<DarkStarFriendly>();

					int p = Projectile.NewProjectile(projectile.Center + HoldOffset * 2, projectile.velocity * 18, type, projectile.damage, projectile.knockBack, player.whoAmI);
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 105, 1f, -0.3f);
					if (p < 1000)
					{
						SplitProj(Main.projectile[p], 21);
					}
					timer = 0;
				}
				projectile.timeLeft++;

				if (projectile.ai[1] == 0)
				{
					int type = ModContent.ProjectileType<PrimeDeathray>();

					int p = Projectile.NewProjectile(projectile.Center, projectile.velocity, type, projectile.damage, projectile.knockBack, player.whoAmI, 0, projectile.whoAmI);

					if (p < 1000)
					{
						SplitProj(Main.projectile[p], 17);
					}
					projectile.ai[1]++;
				}
				else if (player.ownedProjectileCounts[ModContent.ProjectileType<PrimeDeathray>()] < 12)
				{
					projectile.Kill();
				}
			}

			float extrarotate = (projectile.direction < 0) ? MathHelper.Pi : 0; 
			player.itemRotation = projectile.velocity.ToRotation() + extrarotate; 
			player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
			player.ChangeDir(projectile.direction);
			projectile.spriteDirection = projectile.direction;
			projectile.rotation -= extrarotate;
			player.heldProj = projectile.whoAmI;
			player.itemTime = 10;
			player.itemAnimation = 10;

			projectile.Center += projectile.velocity * 20;

			if (!player.channel)
			{
				projectile.Kill();
			}
		}

		public static void SplitProj(Projectile projectile, int number)
		{
			//if its odd, we just keep the original 
			if (number % 2 != 0)
			{
				number--;
			}

			double spread = MathHelper.Pi / 2 / number;

			for (int i = 2; i < number / 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int factor = (j == 0) ? 1 : -1;
					float ai0 = (projectile.type == ModContent.ProjectileType<PrimeDeathray>()) ? (i + 1) * factor : 0;
					Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(factor * spread * (i + 1)), projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 
						ai0, projectile.ai[1]);
				}
			}

			projectile.active = false;
		}
	}
}
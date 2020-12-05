using FargowiltasSouls.Projectiles.Minions;
using IL.Terraria.Chat.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.ComponentModel;
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
			Main.projFrames[projectile.type] = 7;
		}

		public int timer;
        public float lerp = 0.12f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refractor Blaster Ex");
		}

        public override bool CanDamage()
        {
            return false;
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

			float extrarotate = (projectile.direction < 0) ? MathHelper.Pi : 0;
			player.itemRotation = projectile.velocity.ToRotation() + extrarotate;
			player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
			player.ChangeDir(projectile.direction);
			player.heldProj = projectile.whoAmI;
			player.itemTime = 10;
			player.itemAnimation = 10;
			Vector2 HoldOffset = new Vector2(projectile.width/3, 0).RotatedBy(MathHelper.WrapAngle(projectile.velocity.ToRotation()));

			projectile.Center += HoldOffset;
			projectile.spriteDirection = projectile.direction;
			projectile.rotation -= extrarotate;

			projectile.frameCounter++;
			if(projectile.frameCounter > 3)
			{
				projectile.frame++;
				if (projectile.frame > Main.projFrames[projectile.type] - 1)
					projectile.frame = 0;

				projectile.frameCounter = 0;
			}
			if (player.channel)
			{
				projectile.velocity = Vector2.Lerp(Vector2.Normalize(projectile.velocity),
					Vector2.Normalize(Main.MouseWorld - player.MountedCenter), lerp); //slowly move towards direction of cursor
				projectile.velocity.Normalize();

				timer++;
				if (timer % 6 == 0)
				{
					Main.PlaySound(player.inventory[player.selectedItem].UseSound, projectile.Center);
					bool checkmana = player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
					if (!checkmana)
						projectile.Kill();

				}
				if (timer > 60)
				{
					int type = ModContent.ProjectileType<DarkStarFriendly>();
                    const int max = 10;
                    double spread = MathHelper.Pi / 2 / max;
                    for (int i = -max; i <= max; i++)
                    {
                        Projectile.NewProjectile(projectile.Center + HoldOffset * 2, 22f * projectile.velocity.RotatedBy(spread * i),
                            type, projectile.damage, projectile.knockBack, projectile.owner);
                    }
					/*int p = Projectile.NewProjectile(projectile.Center + HoldOffset * 2, projectile.velocity * 22, type, projectile.damage, projectile.knockBack, player.whoAmI);
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 105, 1f, -0.3f);
					if (p < 1000)
					{
						SplitProj(Main.projectile[p], 21);
					}*/
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

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int width = Main.projectileTexture[projectile.type].Width;
			int frame = height * projectile.frame;
			SpriteEffects flipdirection = projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Rectangle Origin = new Rectangle(0, frame, width, height);
			spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, Origin, lightColor, projectile.rotation, new Vector2(width/2, height/2), projectile.scale, flipdirection, 0f);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = mod.GetTexture("Items/Weapons/SwarmDrops/RefractorBlaster2Glow");
			int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int width = Main.projectileTexture[projectile.type].Width;
			int frame = height * projectile.frame;
			SpriteEffects flipdirection = projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Rectangle Origin = new Rectangle(0, frame, width, height);
			spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, Origin, Color.White, projectile.rotation, new Vector2(width / 2, height / 2), projectile.scale, flipdirection, 0f);
		}
	}
}
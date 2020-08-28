using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class SlimeSpike2 : SlimeSpike
    {
        public override string Texture => "Terraria/Projectile_605";

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.scale = 1.5f;
        }
    }
}
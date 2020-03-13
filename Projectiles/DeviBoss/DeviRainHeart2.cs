using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviRainHeart2 : DeviRainHeart
    {
        public override string Texture => "FargowiltasSouls/Projectiles/Masomode/FakeHeart";

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
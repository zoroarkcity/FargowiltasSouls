using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.AbomBoss
{
    public class AbomBlast : BossWeapons.PhantasmalBlast
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 50, 50, 127);
        }
    }
}


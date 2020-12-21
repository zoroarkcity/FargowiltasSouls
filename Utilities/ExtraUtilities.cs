using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Utilities
{
    public static class ExtraUtilities
    {
        //Draw an item with given parameters, used to simplify glowmask drawing

        public static void DrawItem(int whoami, Texture2D texture, float rotation, int maxframes, Color color)
        {
            Item item = Main.item[whoami];
            int height = texture.Height / maxframes;
            int width = texture.Width;
            int frame = height * Main.itemFrame[whoami];
            SpriteEffects flipdirection = item.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle Origin = new Rectangle(0, frame, width, height);
            Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition, Origin, color, rotation, Origin.Size() / 2, item.scale, flipdirection, 0f);
        }
    }
}
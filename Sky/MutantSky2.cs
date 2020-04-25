using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;
using FargowiltasSouls.NPCs.MutantBoss;

namespace FargowiltasSouls.Sky
{
    public class MutantSky2 : CustomSky
    {
        private bool isActive = false;
        private bool increase = true;
        private float intensity = 0f;

        public override void Update(GameTime gameTime)
        {
            if (increase)
            {
                float increment = 0.04f;

                intensity += increment;
                if (intensity > 1f)
                {
                    intensity = 1f;
                    increase = false;
                }
            }
            else
            {
                float increment = 0.01f;

                intensity -= increment;
                if (intensity < 0f)
                {
                    intensity = 0f;
                    increase = true;
                    Deactivate();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                spriteBatch.Draw(ModContent.GetTexture("FargowiltasSouls/Sky/MutantSky2"),
                    new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * intensity * 0.5f);
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f - intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
            increase = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
            increase = true;
        }

        public override void Reset()
        {
            isActive = false;
            increase = true;
        }

        public override bool IsActive()
        {
            return isActive;
        }

        public override Color OnTileColor(Color inColor)
        {
            return new Color(Vector4.Lerp(new Vector4(0.6f, 0.9f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
        }
    }
}
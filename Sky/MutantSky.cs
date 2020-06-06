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
    public class MutantSky : CustomSky
    {
        private bool isActive = false;
        private float intensity = 0f;
        private float lifeIntensity = 0f;

        public override void Update(GameTime gameTime)
        {
            const float increment = 0.01f;
            if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantBoss>()))
            {
                intensity += increment;
                if (intensity > 1f)
                {
                    intensity = 1f;
                }

                if (Main.npc[EModeGlobalNPC.mutantBoss].ai[0] != 10)
                {
                    lifeIntensity = 1f - (float)Main.npc[EModeGlobalNPC.mutantBoss].life / Main.npc[EModeGlobalNPC.mutantBoss].lifeMax;
                }
            }
            else
            {
                intensity -= increment;
                lifeIntensity -= increment;
                if (lifeIntensity < 0f)
                    lifeIntensity = 0f;
                if (intensity < 0f)
                {
                    intensity = 0f;
                    lifeIntensity = 0f;
                    Deactivate();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                spriteBatch.Draw(ModContent.GetTexture("FargowiltasSouls/Sky/MutantSky"),
                    new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * (intensity * 0.5f + lifeIntensity * 0.5f));
                
                for (int i = 0; i < 40; i++) //static on screen
                {
                    int width = Main.rand.Next(100, 251);
                        spriteBatch.Draw(ModContent.GetTexture("FargowiltasSouls/Sky/MutantStatic"),
                        new Rectangle(Main.rand.Next(Main.screenWidth) - width / 2, Main.rand.Next(Main.screenHeight), width, 3), 
                        Color.White * lifeIntensity);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f - intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
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
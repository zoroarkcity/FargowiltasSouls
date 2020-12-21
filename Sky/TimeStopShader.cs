using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Souls;

namespace FargowiltasSouls.Sky
{
    public class TimeStopShader : ScreenShaderData
    {
        public TimeStopShader(string passName) : base(passName)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.GetModPlayer<FargoPlayer>().FreezeTime && !Main.LocalPlayer.HasBuff(ModContent.BuffType<TimeFrozen>()))
                Filters.Scene.Deactivate("FargowiltasSouls:TimeStop");
        }

        public override void Apply()
        {
            base.Apply();
        }
    }
}
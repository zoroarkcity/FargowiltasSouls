using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Forces
{
    public class NatureForce : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Nature");

            string tooltip =
@"'Tapped into every secret of the wilds'
Greatly increases life regen
Nearby enemies are ignited
The closer they are to you the more damage they take
When you die, you violently explode dealing massive damage
Icicles will start to appear around you
When there are three, attacking will launch them towards the cursor
Your attacks inflict Frostburn
Summons a ring of leaf crystals to shoot at nearby enemies
Taking damage will release a lingering spore explosion
All herb collection is doubled
Not moving puts you in stealth
While in stealth, crits deal 3x damage
";
            string tooltip_ch =
@"'挖掘了荒野的每一个秘密'
极大增加生命恢复速度
点燃附近敌人
敌人距离越近, 收到的伤害越多
死亡时剧烈爆炸, 造成大量伤害
你的周围将出现冰柱
当存在3枚时, 攻击会将它们向光标位置发射
攻击造成霜火效果
受到伤害会释放出挥之不去的孢子爆炸
召唤一圈叶绿水晶射击附近的敌人
所有药草收获翻倍
站立不动时潜行
潜行时, 暴击造成3倍伤害
";

            if (thorium == null)
            {
                tooltip +=
@"Effects of Flower Boots
Summons several pets";

                tooltip_ch +=
@"拥有花之靴的效果
召唤数个宠物";
            }
            else
            {
                tooltip +=
@"Attacks may inflict Fungal Growth
Effects of Flower Boots
Summons several pets";

                tooltip_ch +=
@"生命值每下降25%, 增加5%攻击速度
随着时间的推移, 被你点燃或烧伤的敌人会受到额外的伤害
造成伤害会释放出有毒的孢子爆炸
攻击概率造成真菌寄生效果
拥有影缀花, 零度音箱和剧毒音箱的效果
拥有花之靴, 弹簧鞋和熔渣重踏的效果
召唤数个宠物";
            }

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "自然之力");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 11;
            item.value = 600000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            //bulb, cryo effect
            modPlayer.NatureForce = true;
            //regen, pets
            modPlayer.CrimsonEffect(hideVisual);
            //inferno and explode
            modPlayer.MoltenEffect(30);
            //icicles, pets
            modPlayer.FrostEffect(75, hideVisual);
            //spores
            modPlayer.JungleEffect();
            //crystal and pet
            modPlayer.ChloroEffect(hideVisual, 100);
            modPlayer.FlowerBoots();
            //stealth, crits, pet
            modPlayer.ShroomiteEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "CrimsonEnchant");
            recipe.AddIngredient(null, "MoltenEnchant");
            recipe.AddIngredient(null, "FrostEnchant");
            recipe.AddIngredient(null, "ChlorophyteEnchant");
            recipe.AddIngredient(null, "ShroomiteEnchant");

            recipe.AddTile(TileID.LunarCraftingStation);

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
// XNA
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using static Microsoft.Xna.Framework.MathHelper;
// Ebonian Mod
global using EbonianMod.Common;
global using EbonianMod.Core.Registries;
global using EbonianMod.Core.Systems;
global using EbonianMod.Core.Systems.Misc;
global using EbonianMod.Core.Utilities;
global using static EbonianMod.Core.Utilities.Easings;
// ReLogic
global using ReLogic.Content;
// Terraria
global using Terraria;
global using Terraria.Audio;
global using Terraria.DataStructures;
global using Terraria.GameContent;
global using Terraria.GameContent.ItemDropRules;
global using Terraria.ID;
global using Terraria.Localization;
global using static Terraria.ModLoader.ModContent;
// tModLoader
global using Terraria.ModLoader;
global using Terraria.UI;
global using Terraria.Utilities;

using EbonianMod.Content.Skies;
using EbonianMod.Content.NPCs.Aureus;
using EbonianMod.Core;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace EbonianMod;

public class EbonianMod : Mod
{
    public static EbonianMod Instance => GetInstance<EbonianMod>();
    public static List<int> projectileFinalDrawList = new List<int>();
    public RenderTarget2D blurrender;
    public EbonianMod() => MusicSkipsVolumeRemap = true;
    public override void HandlePacket(BinaryReader reader, int whoAmI) => Packets.HandlePackets(reader);
    public override void Load()
    {
        LoadDrawCache();
        Main.OnResolutionChanged += (Vector2 obj) => CreateRender();
        CreateRender();
    }
    public static List<Action> invisibleMaskCache = [], affectedByInvisibleMaskCache = [],
        blurDrawCache = [], pixelationDrawCache = [], primitivePixelationDrawCache = [], finalDrawCache = [], garbageFlameCache = [],
        xareusGoopCache = [];
    public void LoadDrawCache()
    {
        invisibleMaskCache ??= [];
        affectedByInvisibleMaskCache ??= [];
        blurDrawCache ??= [];
        pixelationDrawCache ??= [];
        primitivePixelationDrawCache ??= [];
        finalDrawCache ??= [];
        garbageFlameCache ??= [];
        xareusGoopCache ??= [];
    }
    
    public void CreateRender()
    {
        if (Main.netMode != NetmodeID.Server)
            Main.QueueMainThreadAction(() =>
            {
                if (Instance.blurrender is not null)
                    if (!Instance.blurrender.IsDisposed)
                        Instance.blurrender.Dispose();
                Instance.blurrender = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            });
    }
}

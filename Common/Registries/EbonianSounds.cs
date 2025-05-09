namespace EbonianMod.Common.Registries;

public class EbonianSounds : ModSystem
{
    public static SoundStyle Default = new SoundStyle("EbonianMod/Sounds/reiFail")
    {
        MaxInstances = 10,
        PitchVariance = 0.3f,
    };


    public const string ebonianSoundPath = "EbonianMod/Sounds/";
    public static SoundStyle None;
    public static SoundStyle bloodSpit => Default with
    {
        SoundPath = ebonianSoundPath + "bloodSpit"
    };

    public static SoundStyle bowPull => Default with
    {
        SoundPath = ebonianSoundPath + "bowPull",
        Volume = GetInstance<EbonianClientConfig>().BowPullVolume
    };


    public static SoundStyle bowRelease => Default with
    {
        SoundPath = ebonianSoundPath + "bowRelease"
    };

    public static SoundStyle buzz => Default with
    {
        SoundPath = ebonianSoundPath + "buzz",
        PitchVariance = 0
    };

    public static SoundStyle cFlameBreath => Default with
    {
        SoundPath = ebonianSoundPath + "Conglomerate/cFlameBreath",
        MaxInstances = 1,
        IsLooped = true,
        PitchVariance = 0
    };

    public static SoundStyle chargedBeam => Default with
    {
        SoundPath = ebonianSoundPath + "chargedBeam"
    };


    public static SoundStyle chargedBeamWindUp => Default with
    {
        SoundPath = ebonianSoundPath + "chargedBeamWindUp",
        PitchVariance = 0f
    };


    public static SoundStyle chargedBeamImpactOnly => Default with
    {
        SoundPath = ebonianSoundPath + "chargedBeamImpactOnly",
        PitchVariance = 0f
    };


    public static SoundStyle cursedToyCharge => Default with
    {
        SoundPath = ebonianSoundPath + "cursedToyCharge",
        PitchVariance = 0f
    };


    public static SoundStyle chomp0 => Default with
    {
        SoundPath = ebonianSoundPath + "chomp0",
        Volume = 1.3f
    };


    public static SoundStyle chomp1 => Default with
    {
        SoundPath = ebonianSoundPath + "chomp1",
        Volume = 1.3f
    };


    public static SoundStyle chomp2 => Default with
    {
        SoundPath = ebonianSoundPath + "chomp2",
        MaxInstances = 1
    };


    public static SoundStyle cecitiorOpen => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorOpen",
        MaxInstances = 1
    };

    public static SoundStyle cecitiorCloseShort => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorCloseShort",
        MaxInstances = 3
    };


    public static SoundStyle cecitiorClose => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorClose",
        MaxInstances = 3
    };

    public static SoundStyle cecitiorIdle => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorIdle",
        PitchVariance = 0,
        MaxInstances = 1,
        IsLooped = true
    };

    public static SoundStyle cecitiorSlam => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorSlam",
        MaxInstances = 3,
        Variants = [0, 1, 2]
    };

    public static SoundStyle cecitiorSlice => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorSlice",
        MaxInstances = 3,
        Variants = [0, 1, 2]
    };

    public static SoundStyle cecitiorSpit => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorSpit",
        MaxInstances = 3,
        Variants = [0, 1, 2, 3]
    };

    public static SoundStyle deathrayLoop0 => Default with
    {
        SoundPath = ebonianSoundPath + "deathrayLoop0",
        MaxInstances = 1,
        IsLooped = true,
        PitchVariance = 0
    };
    public static SoundStyle deathrayLoop1 => Default with
    {
        SoundPath = ebonianSoundPath + "deathrayLoop1",
        MaxInstances = 1,
        IsLooped = true,
        PitchVariance = 0
    };

    public static SoundStyle eggplosion => Default with
    {
        SoundPath = ebonianSoundPath + "eggplosion"
    };


    public static SoundStyle eruption => Default with
    {
        SoundPath = ebonianSoundPath + "eruption"
    };


    public static SoundStyle exolDash => Default with
    {
        SoundPath = ebonianSoundPath + "exolDash"
    };


    public static SoundStyle exolRoar => Default with
    {
        SoundPath = ebonianSoundPath + "exolRoar"
    };


    public static SoundStyle exolSummon => Default with
    {
        SoundPath = ebonianSoundPath + "exolSummon"
    };


    public static SoundStyle flesh0 => Default with
    {
        SoundPath = ebonianSoundPath + "flesh0"
    };


    public static SoundStyle flesh1 => Default with
    {
        SoundPath = ebonianSoundPath + "flesh1"
    };


    public static SoundStyle flesh2 => Default with
    {
        SoundPath = ebonianSoundPath + "flesh2"
    };


    public static SoundStyle buildup => Default with
    {
        SoundPath = ebonianSoundPath + "buildup",
        PitchVariance = 0
    };


    public static SoundStyle garbageAwaken => Default with
    {
        SoundPath = ebonianSoundPath + "HotGarbage/garbageAwaken",
        PitchVariance = 0
    };


    public static SoundStyle garbageSignal => Default with
    {
        SoundPath = ebonianSoundPath + "HotGarbage/garbageSignal",
        PitchVariance = 0
    };


    public static SoundStyle genericExplosion => Default with
    {
        SoundPath = ebonianSoundPath + "genericExplosion"
    };


    public static SoundStyle heartbeat => Default with
    {
        SoundPath = ebonianSoundPath + "heartbeat"
    };


    public static SoundStyle nuke => Default with
    {
        SoundPath = ebonianSoundPath + "nuke"
    };


    public static SoundStyle reiFail => Default with
    {
        SoundPath = ebonianSoundPath + "reiFail"
    };


    public static SoundStyle reiFail2 => Default with
    {
        SoundPath = ebonianSoundPath + "reiFail2"
    };


    public static SoundStyle reiTP => Default with
    {
        SoundPath = ebonianSoundPath + "reiTP"
    };


    public static SoundStyle rolleg => Default with
    {
        SoundPath = ebonianSoundPath + "rolleg"
    };


    public static SoundStyle terrortomaDash => Default with
    {
        SoundPath = ebonianSoundPath + "Terrortoma/terrortomaDash"
    };


    public static SoundStyle cecitiorDie => Default with
    {
        SoundPath = ebonianSoundPath + "NPCHit/cecitiorDie"
    };


    public static SoundStyle fleshHit => Default with
    {
        SoundPath = ebonianSoundPath + "NPCHit/fleshHit"
    };


    public static SoundStyle terrortomaFlesh => Default with
    {
        SoundPath = ebonianSoundPath + "Terrortoma/terrortomaFlesh"
    };


    public static SoundStyle cecitiorBurp => Default with
    {
        SoundPath = ebonianSoundPath + "Cecitior/cecitiorBurp"
    };


    public static SoundStyle clawSwipe => Default with
    {
        SoundPath = ebonianSoundPath + "clawSwipe"
    };


    public static SoundStyle BeamWindUp => Default with
    {
        SoundPath = ebonianSoundPath + "BeamWindUp",
        PitchVariance = 0
    };


    public static SoundStyle garbageLaser => Default with
    {
        SoundPath = ebonianSoundPath + "HotGarbage/garbageLaser",
        PitchVariance = 0
    };


    public static SoundStyle trumpet => Default with
    {
        SoundPath = ebonianSoundPath + "trumpet",
        PitchVariance = 0.4f,
        Variants = new int[] { 0, 1 }
    };


    public static SoundStyle terrortomaLaugh => Default with
    {
        SoundPath = ebonianSoundPath + "Terrortoma/terrortomaLaugh",
        Variants = new int[] { 0, 1 },
        PitchVariance = 0.25f,
        Volume = 1.1f
    };


    public static SoundStyle HeavySwing => Default with
    {
        SoundPath = ebonianSoundPath + "HeavySwing",
        Variants = new int[] { 0, 1, 2 },
        PitchVariance = 0.25f,
        Volume = 1.1f
    };


    public static SoundStyle FleshImpact => Default with
    {
        SoundPath = ebonianSoundPath + "FleshImpact",
        Variants = new int[] { 0, 1, 2 },
        PitchVariance = 0.25f,
        Volume = .7f
    };


    public static SoundStyle blink => Default with
    {
        SoundPath = ebonianSoundPath + "blink"
    };


    public static SoundStyle xSpirit => Default with
    {
        SoundPath = ebonianSoundPath + "Xareus/xSpirit",
        Volume = 0.7f
    };

    public static SoundStyle xDeath => Default with
    {
        SoundPath = ebonianSoundPath + "Xareus/xDeath",
        PitchVariance = 0.05f
    };


    public static SoundStyle helicopter => Default with
    {
        SoundPath = ebonianSoundPath + "helicopter",
        PitchVariance = 0f
    };


    public static SoundStyle firework => Default with
    {
        SoundPath = ebonianSoundPath + "firework"
    };


    public static SoundStyle magicSlash => Default with
    {
        SoundPath = ebonianSoundPath + "magicSlash"
    };


    public static SoundStyle sheep => Default with
    {
        SoundPath = ebonianSoundPath + "sheep",
        PitchVariance = 0.5f,
        Variants = new int[] { 0, 1, 2 }
    };


    public static SoundStyle shears => Default with
    {
        SoundPath = ebonianSoundPath + "shears",
        PitchVariance = 0.3f,
    };


    public static SoundStyle sheep_player => Default with
    {
        SoundPath = ebonianSoundPath + "sheep",
        PitchVariance = 0.5f,
        Variants = new int[] { 0, 1 }
    };


    public static SoundStyle shriek => Default with
    {
        SoundPath = ebonianSoundPath + "shriek"
    };


    public static SoundStyle vaccum => Default with
    {
        SoundPath = ebonianSoundPath + "vaccum",
        IsLooped = true,
        PitchVariance = 0f
    };


    public static SoundStyle ghizasWheel => Default with
    {
        SoundPath = ebonianSoundPath + "ghizasWheel",
        IsLooped = true,
        PitchVariance = 0f
    };


    public static SoundStyle garbageDeath => Default with
    {
        SoundPath = ebonianSoundPath + "NPCHit/garbageDeath",
        PitchVariance = 0
    };


    public static SoundStyle evilOutro => Default with
    {
        SoundPath = ebonianSoundPath + "Music/Outros/evilOutro",
        PitchVariance = 0,
        MaxInstances = 1,
        Type = SoundType.Music
    };

    public static SoundStyle xareusOutro => Default with
    {
        SoundPath = ebonianSoundPath + "Music/Outros/xareusOutro",
        PitchVariance = 0,
        MaxInstances = 1,
        Type = SoundType.Music
    };
}

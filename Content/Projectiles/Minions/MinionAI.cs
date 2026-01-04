namespace EbonianMod.Content.Projectiles.Minions;

public abstract class MinionAI : ModProjectile
{
    public override void AI()
    {
        CheckActive();
        Behavior();
    }

    public abstract void CheckActive();

    public abstract void Behavior();
}
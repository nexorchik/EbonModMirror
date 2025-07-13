using System;
using System.Collections.Generic;
using System.Linq;

namespace EbonianMod.Common.Systems.Verlets;
public struct VerletTextureData
{
    public string texPath, baseTex, endTex;
    public Rectangle? frame = null, baseFrame = null, endFrame = null;
    public VerletTextureData(string _texPath, string _baseTex = null, string _endTex = null, Rectangle? _frame = null, Rectangle? _baseFrame = null, Rectangle? _endFrame = null)
    {
        texPath = _texPath;
        baseTex = _baseTex;
        endTex = _endTex;
        frame = _frame;
        baseFrame = _baseFrame;
        endFrame = _endFrame;
    }
}
public struct VerletDrawData
{
    public int maxVariants, variantSeed;
    public float scale;
    public float? rot, endRot, firstRot;
    public Color? color;
    public VerletTextureData tex;
    public VerletDrawData(VerletTextureData _tex, Color? _color = null, float _scale = 1, float? _rot = null,
         float? _endRot = null, float? _firstRot = null, int _maxVariants = -1, int _variantSeed = -1)
    {
        tex = _tex;
        variantSeed = _variantSeed;
        color = _color;
        scale = _scale;
        rot = _rot;
        endRot = _endRot;
        firstRot = _firstRot;
        maxVariants = _maxVariants;
    }
}
public class Verlet
{
    public int stiffness { get; set; }
    public List<VerletSegment> segments { get; set; }
    public List<VerletPoint> points { get; set; }
    public VerletPoint firstP { get; set; }
    public VerletPoint lastP { get; set; }
    public float gravity { get; set; }
    public Vector2 gravityDirection { get; set; }
    public float startRot => segments[0].Rotation();
    public float endRot => segments[segments.Count - 1].Rotation();
    public Vector2 startPos => segments[0].pointA.position;
    public Vector2 endPos => segments[segments.Count - 1].pointB.position;

    public Verlet(Vector2 start, float length, int count, float gravity = 0.2f, bool firstPointLocked = true, bool lastPointLocked = true, int stiffness = 6, bool collide = false, float colLength = 1)
    {
        this.gravity = gravity;
        this.stiffness = stiffness;
        gravityDirection = Vector2.UnitY;
        Load(start, length, count, firstPointLocked, lastPointLocked, collide: collide, colLength: colLength);
    }
    private void Load(Vector2 startPosition, float length, int count, bool firstPointLocked = true, bool lastPointLocked = true, Vector2 offset = default, bool collide = false, float colLength = 1)
    {
        segments = new List<VerletSegment>();
        points = new List<VerletPoint>();

        for (int i = 0; i < count; i++)
        {
            points.Add(new VerletPoint(startPosition + (offset == default ? Vector2.Zero : offset * i), gravity, gravityDirection, collide, colLength));
        }

        for (int i = 0; i < count - 1; i++)
        {
            segments.Add(new VerletSegment(length, points[i], points[i + 1]));
        }

        firstP = points.First();
        firstP.locked = firstPointLocked;

        lastP = points.Last();
        lastP.locked = lastPointLocked;
    }
    public void Update(Vector2 start, Vector2 end, float lerpT = 1f)
    {
        if (Main.dedServ) return;
        if (firstP.locked)
            firstP.position = Vector2.Lerp(firstP.position, start, lerpT);
        if (lastP.locked)
            lastP.position = Vector2.Lerp(lastP.position, end, lerpT);
        foreach (VerletPoint point in points)
        {
            point.Update();
            point.gravity = gravity;
            if (point.gravityDirection != gravityDirection)
            {
                if (gravityDirection.Length() > 0)
                    gravityDirection.Normalize();
                point.gravityDirection = gravityDirection;
            }
        }
        for (int i = 0; i < stiffness; i++)
            foreach (VerletSegment segment in segments)
                segment.Constrain();
    }

    public Vector2[] Points()
    {
        List<Vector2> verticeslist = new List<Vector2>();
        foreach (VerletPoint point in points)
            verticeslist.Add(point.position);

        return verticeslist.ToArray();
    }
    public void Draw(SpriteBatch sb, string path) => Draw(sb, new VerletDrawData(new VerletTextureData(path)));
    public void Draw(SpriteBatch sb, VerletDrawData drawData)
    {
        UnifiedRandom rand = new UnifiedRandom(drawData.variantSeed);
        foreach (VerletSegment segment in segments)
        {
            if (drawData.tex.baseTex is not null || drawData.tex.endTex is not null ? segment != segments.First() && segment != segments.Last() : true)
            {
                int variant = rand.Next(drawData.maxVariants > 0 ? drawData.maxVariants : 2);
                segment.DrawSegments(sb, drawData.tex.texPath + (drawData.maxVariants > 0 ? variant.ToString() : ""), drawData.color, drawData.scale, drawData.rot, drawData.tex.frame);
            }
            else if (drawData.tex.endTex is not null && segment == segments.Last())
                segment.Draw(sb, drawData.tex.endTex, drawData.color, drawData.scale, drawData.endRot, drawData.tex.endFrame);
            else if (drawData.tex.baseTex is not null && segment == segments.First())
                segment.Draw(sb, drawData.tex.baseTex, drawData.color, drawData.scale, drawData.firstRot, drawData.tex.baseFrame);
        }
    }
}
public class VerletPoint
{
    public Vector2 position, lastPos, gravityDirection;
    public bool locked;
    public float gravity;
    public bool collide;
    public float colLength;
    public VerletPoint(Vector2 position, float gravity, Vector2 gravityDirection, bool collide = false, float colLength = 1)
    {
        this.position = position;
        this.gravity = gravity;
        this.collide = collide;
        this.colLength = colLength;
        this.gravityDirection = gravityDirection;
    }

    public void Update()
    {
        if (collide)
        {
            if (Helper.TRay.CastLength(position, gravityDirection, colLength) >= colLength || !Collision.SolidCollision(position, (int)colLength, (int)colLength))
            {
                lastPos = position;
                position += gravityDirection * gravity;
            }
        }
        else
        {
            lastPos = position;
            position += gravityDirection * gravity;
        }
    }
}
public class VerletSegment
{
    public bool cut = false;
    public float len;
    public float Rotation()
    {
        return (pointA.position - pointB.position).ToRotation() - 1.57f;
    }
    public VerletPoint pointA, pointB;
    public VerletSegment(float len, VerletPoint pointA, VerletPoint pointB)
    {
        this.len = len;
        cut = false;
        this.pointA = pointA;
        this.pointB = pointB;
    }

    public void Constrain()
    {
        if (cut)
            return;
        Vector2 vel = pointB.position - pointA.position;
        float distance = vel.Length();
        float fraction = (len - distance) / Math.Max(distance, 1) / 2;
        vel *= fraction;

        if (!pointA.locked)
            pointA.position -= vel;
        if (!pointB.locked)
            pointB.position += vel;
    }
    public void Draw(SpriteBatch sb, string texPath, Color? color = null, float scale = 1, float? rot = null, Rectangle? frame = null)
    {
        if (cut)
            return;
        Texture2D tex = Helper.GetTexture(texPath).Value;
        sb.Draw(tex, pointB.position - Main.screenPosition, frame, color ?? Lighting.GetColor((int)pointB.position.X / 16, (int)(pointB.position.Y / 16.0)), rot ?? Rotation(), tex.Size() / 2, scale, SpriteEffects.None, 0);
    }
    public void DrawSegments(SpriteBatch sb, string texPath, Color? color = null, float scale = 1, float? rot = null, Rectangle? frame = null)
    {
        if (cut)
            return;
        Texture2D tex = Helper.GetTexture(texPath).Value;
        Vector2 center = pointB.position;
        Vector2 distVector = pointA.position - pointB.position;
        float distance = distVector.Length();
        int attempts = 0;
        sb.Draw(tex, pointA.position - Main.screenPosition, frame, color ?? Lighting.GetColor((int)pointA.position.X / 16, (int)(pointA.position.Y / 16.0)), rot ?? Rotation(), tex.Size() / 2, scale, SpriteEffects.None, 0);
        while (distance > tex.Height * 0.5f * scale && !float.IsNaN(distance) && ++attempts < 400)
        {
            distVector.Normalize();
            distVector *= tex.Height * scale;
            center += distVector;
            distVector = pointA.position - center;
            distance = distVector.Length();
            sb.Draw(tex, center - Main.screenPosition, frame, color ?? Lighting.GetColor((int)pointB.position.X / 16, (int)(pointB.position.Y / 16.0)), rot ?? Rotation(), tex.Size() / 2, scale, SpriteEffects.None, 0);
        }
        Draw(sb, texPath, color, scale, rot, frame);
    }
}

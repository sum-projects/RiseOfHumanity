using OpenTK.Mathematics;

namespace RiseOfHumanity.Graphics;

public class Voxel
{
    public Vector3 Position { get; set; }
    public Color4 Color { get; set; }

    public Voxel(Vector3 position, Color4 color)
    {
        Position = position;
        Color = color;
    }
}
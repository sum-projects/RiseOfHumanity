using OpenTK.Mathematics;
using RiseOfHumanity.Graphics;

namespace RiseOfHumanity.Game;

public class MapGenerator
{
    private int _width;
    private int _height;
    private int _depth;

    public MapGenerator(int width, int height, int depth)
    {
        _width = width;
        _height = height;
        _depth = depth;
    }

    public Map Generate()
    {
        var voxels = new Voxel[_width, _height, _depth];

        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                for (var z = 0; z < _depth; z++)
                {
                    if (y < _height / 2)
                    {
                        voxels[x, y, z] = new Voxel(new Vector3(x, y, z), new Color4(1.0f, 0.5f, 0.3f, 1.0f));
                    }
                    else
                    {
                        voxels[x, y, z] = null;
                    }
                }
            }
        }
        
        return new Map(voxels);
    }
}
using OpenTK.Mathematics;
using RiseOfHumanity.Graphics;

namespace RiseOfHumanity.Game;

public class Map
{
    public Voxel[,,] _voxels;

    public Map(Voxel[,,] voxels)
    {
        _voxels = voxels;
    }

    public void GetVertexAndIndexData(out Vector3[] vertices, out int[] indices, out Color4[] colors)
    {
        var vertList = new List<Vector3>();
        var indexList = new List<int>();
        var colorList = new List<Color4>();

        var currentIndex = 0;

        for (var x = 0; x < _voxels.GetLength(0); x++)
        {
            for (var y = 0; y < _voxels.GetLength(1); y++)
            {
                for (var z = 0; z < _voxels.GetLength(2); z++)
                {
                    var voxel = _voxels[x, y, z];
                    if (voxel != null)
                    {
                        vertList.AddRange(GetCubeVertices(voxel.Position));
                        colorList.AddRange(Enumerable.Repeat(voxel.Color, 24));
                        indexList.AddRange(GetCubeIndices(currentIndex));
                        currentIndex += 24;
                    }
                }
            }
        }

        vertices = vertList.ToArray();
        indices = indexList.ToArray();
        colors = colorList.ToArray();
    }

    private static IEnumerable<Vector3> GetCubeVertices(Vector3 position)
    {
        var size = 1.0f;
        var halfSize = size / 2.0f;

        return new[]
        {
            // Front face
            position + new Vector3(-halfSize, -halfSize, -halfSize),
            position + new Vector3(halfSize, -halfSize, -halfSize),
            position + new Vector3(halfSize, halfSize, -halfSize),
            position + new Vector3(-halfSize, halfSize, -halfSize),

            // Back face
            position + new Vector3(-halfSize, -halfSize, halfSize),
            position + new Vector3(halfSize, -halfSize, halfSize),
            position + new Vector3(halfSize, halfSize, halfSize),
            position + new Vector3(-halfSize, halfSize, halfSize),

            // Left face
            position + new Vector3(-halfSize, -halfSize, halfSize),
            position + new Vector3(-halfSize, halfSize, halfSize),
            position + new Vector3(-halfSize, halfSize, -halfSize),
            position + new Vector3(-halfSize, -halfSize, -halfSize),

            // Right face
            position + new Vector3(halfSize, -halfSize, halfSize),
            position + new Vector3(halfSize, halfSize, halfSize),
            position + new Vector3(halfSize, halfSize, -halfSize),
            position + new Vector3(halfSize, -halfSize, -halfSize),

            // Top face
            position + new Vector3(-halfSize, halfSize, halfSize),
            position + new Vector3(halfSize, halfSize, halfSize),
            position + new Vector3(halfSize, halfSize, -halfSize),
            position + new Vector3(-halfSize, halfSize, -halfSize),

            // Bottom face
            position + new Vector3(-halfSize, -halfSize, halfSize),
            position + new Vector3(halfSize, -halfSize, halfSize),
            position + new Vector3(halfSize, -halfSize, -halfSize),
            position + new Vector3(-halfSize, -halfSize, -halfSize),
        };
    }

    private static int[] GetCubeIndices(int startIdx)
    {
        return new[]
        {
            // Front face
            startIdx, startIdx + 1, startIdx + 2,
            startIdx, startIdx + 2, startIdx + 3,

            // Back face
            startIdx + 4, startIdx + 5, startIdx + 6,
            startIdx + 4, startIdx + 6, startIdx + 7,

            // Left face
            startIdx + 8, startIdx + 9, startIdx + 10,
            startIdx + 8, startIdx + 10, startIdx + 11,

            // Right face
            startIdx + 12, startIdx + 13, startIdx + 14,
            startIdx + 12, startIdx + 14, startIdx + 15,

            // Top face
            startIdx + 16, startIdx + 17, startIdx + 18,
            startIdx + 16, startIdx + 18, startIdx + 19,

            // Bottom face
            startIdx + 20, startIdx + 21, startIdx + 22,
            startIdx + 20, startIdx + 22, startIdx + 23,
        };
    }
}
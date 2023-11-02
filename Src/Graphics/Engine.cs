using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace RiseOfHumanity.Graphics;

public class Engine
{
    private int _program;
    private Shader _vs;
    private Shader _fs;

    private int _vertexArray;
    private int _vertexBuffer;
    private int _vertexColors;
    private int _elementBuffer;

    private Vector3[] _vertices;
    private int[] _indices;
    private Color4[] _colors;

    public Engine(Vector3[] vertices, int[] indices, Color4[] colors)
    {
        _vertices = vertices;
        _indices = indices;
        _colors = colors;
        _program = GL.CreateProgram();

        _vs = new Shader(_program, "Src/Shaders/vs.glsl", ShaderType.VertexShader);
        _fs = new Shader(_program, "Src/Shaders/fs.glsl", ShaderType.FragmentShader);

        _vertexArray = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArray);

        _vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * Vector3.SizeInBytes, _vertices,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);

        _vertexColors = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexColors);
        GL.BufferData(BufferTarget.ArrayBuffer, _colors.Length * Vector3.SizeInBytes, _colors,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(1);

        _elementBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices,
            BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

        GL.Enable(EnableCap.DepthTest);
    }

    public void Draw(Camera camera)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_program);
        GL.BindVertexArray(_vertexArray);

        var model = GL.GetUniformLocation(_program, "model");
        var view = GL.GetUniformLocation(_program, "view");
        var project = GL.GetUniformLocation(_program, "project");

        GL.UniformMatrix4(model, false, ref camera.Model);
        GL.UniformMatrix4(view, false, ref camera.View);
        GL.UniformMatrix4(project, false, ref camera.Project);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        GL.BindVertexArray(0);
    }
}
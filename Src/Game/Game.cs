using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace RiseOfHumanity.Game;

public class Game : GameWindow
{
    private int _program;
    private int _vertexArray;

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(
        gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        var vsSource = File.ReadAllText("Src/Shaders/vs.glsl");
        var fsSource = File.ReadAllText("Src/Shaders/fs.glsl");

        var vs = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vs, vsSource);
        GL.CompileShader(vs);

        var fs = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fs, fsSource);
        GL.CompileShader(fs);

        _program = GL.CreateProgram();
        GL.AttachShader(_program, vs);
        GL.AttachShader(_program, fs);
        GL.LinkProgram(_program);

        GL.DetachShader(_program, vs);
        GL.DetachShader(_program, fs);
        GL.DeleteShader(vs);
        GL.DeleteShader(fs);

        float[] vertices =
        {
            -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f,
            0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f
        };

        _vertexArray = GL.GenVertexArray();
        var vertexBuffer = GL.GenBuffer();

        GL.BindVertexArray(_vertexArray);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(_program);
        GL.BindVertexArray(_vertexArray);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }
}
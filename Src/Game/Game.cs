using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RiseOfHumanity.Graphics;

namespace RiseOfHumanity.Game;

public class Game : GameWindow
{
    private Vector3[] _vertices;
    private int[] _indices;
    private Color4[] _colors;

    private int _program;
    private int _vertexArray;
    private int _vertexBuffer;
    private int _vertexColors;
    private int _elementBuffer;

    private Matrix4 _project;
    private Matrix4 _view;
    private Matrix4 _model;

    private readonly Camera _camera = new();

    private const int Width = 20;
    private const int Height = 14;
    private const int Depth = 2;

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

        var generator = new MapGenerator(Width, Height, Depth);
        var map = generator.Generate();
        map.GetVertexAndIndexData(out _vertices, out _indices, out _colors);

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

        _project = Matrix4.CreatePerspectiveFieldOfView(1.0f, ClientSize.X / (float)ClientSize.Y,
            0.1f, 100.0f);
        _view = _camera.GetViewMatrix();
        _model = Matrix4.Identity;

        var centerX = Width / 2.0f;
        var centerY = Height / 2.0f;
        var centerZ = Depth / 2.0f;

        _camera.Position = new Vector3(centerX, centerY, centerZ - 15.0f);
        _camera.Target = new Vector3(centerX, centerY, centerZ);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (KeyboardState.IsKeyDown(Keys.W))
        {
            _camera.MoveForward(_camera.Speed * (float)e.Time);
        }

        if (KeyboardState.IsKeyDown(Keys.S))
        {
            _camera.MoveBackward(_camera.Speed * (float)e.Time);
        }

        if (KeyboardState.IsKeyDown(Keys.A))
        {
            _camera.MoveLeft(_camera.Speed * (float)e.Time);
        }

        if (KeyboardState.IsKeyDown(Keys.D))
        {
            _camera.MoveRight(_camera.Speed * (float)e.Time);
        }

        if (KeyboardState.IsKeyDown(Keys.Q))
        {
            _model *= _camera.RotateCamera((float)e.Time * -45);

        }

        if (KeyboardState.IsKeyDown(Keys.E))
        {
            _model *= _camera.RotateCamera((float)e.Time * 45);
        }

        _view = _camera.GetViewMatrix();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_program);
        GL.BindVertexArray(_vertexArray);

        var model = GL.GetUniformLocation(_program, "model");
        var view = GL.GetUniformLocation(_program, "view");
        var project = GL.GetUniformLocation(_program, "project");

        GL.UniformMatrix4(model, false, ref _model);
        GL.UniformMatrix4(view, false, ref _view);
        GL.UniformMatrix4(project, false, ref _project);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        GL.BindVertexArray(0);

        SwapBuffers();
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        var zoomAmount = e.OffsetY * 0.5f;
        var dir = Vector3.Normalize(_camera.Target - _camera.Position);

        _camera.Position += dir * zoomAmount;
    }
}
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
    private float _rotation;

    private Camera _camera = new Camera();

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

        var generator = new MapGenerator(100, 100, 10);
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

        _project = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y,
            0.1f, 100.0f);
        _view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -3.0f), Vector3.Zero, Vector3.UnitY);
        _model = Matrix4.Identity;
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

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
}
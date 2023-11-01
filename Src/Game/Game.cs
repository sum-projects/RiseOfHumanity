using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RiseOfHumanity.Graphics;

namespace RiseOfHumanity.Game;

public class Game : GameWindow
{
    private uint[] _indices =
    {
        0, 1, 2, 2, 3, 0, // Przednia ściana
        4, 5, 6, 6, 7, 4, // Tylna ściana
        4, 5, 1, 1, 0, 4, // Dolna ściana
        7, 6, 2, 2, 3, 7, // Górna ściana
        7, 4, 0, 0, 3, 7, // Lewa ściana
        6, 5, 1, 1, 2, 6 // Prawa ściana
    };

    private int _program;
    private int _vertexArray;
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

        float[] vertices =
        {
            // Pozycje        // Kolory
            -0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, // 0
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // 1
            0.5f, 0.5f, -0.5f, 0.0f, 0.0f, 1.0f, // 2
            -0.5f, 0.5f, -0.5f, 1.0f, 1.0f, 0.0f, // 3
            -0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 1.0f, // 4
            0.5f, -0.5f, 0.5f, 0.0f, 1.0f, 1.0f, // 5
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, // 6
            -0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 0.0f // 7
        };

        _vertexArray = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArray);

        var vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        _elementBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

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

        SwapBuffers();
    }
}
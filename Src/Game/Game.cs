using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RiseOfHumanity.Graphics;

namespace RiseOfHumanity.Game;

public class Game : GameWindow
{
    private Engine _graphics;
    private Camera _camera;

    private const int Width = 200;
    private const int Height = 200;
    private const int Depth = 2;

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(
        gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        var generator = new MapGenerator(Width, Height, Depth);
        var map = generator.Generate();
        map.GetVertexAndIndexData(out var vertices, out var indices, out var colors);

        _graphics = new Engine(vertices, indices, colors);
        _camera = new Camera(ClientSize.X, ClientSize.Y, Width, Height, Depth);
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
            _camera.RotateCamera((float)e.Time * -45);
        }

        if (KeyboardState.IsKeyDown(Keys.E))
        {
            _camera.RotateCamera((float)e.Time * 45);
        }

        _camera.UpdateViewMatrix();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        _graphics.Draw(_camera);

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
using OpenTK.Windowing.Desktop;

namespace RiseOfHumanity.Game;

public class Game : GameWindow
{
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }
}
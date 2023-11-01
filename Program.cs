using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RiseOfHumanity.Game;


var settings = new NativeWindowSettings()
{
    WindowState = WindowState.Fullscreen,
    Title = "Rise Of Humanity",
};
using var game = new Game(GameWindowSettings.Default, settings);
game.Run();
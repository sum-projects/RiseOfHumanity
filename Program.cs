using OpenTK.Windowing.Desktop;
using RiseOfHumanity.Game;


var settings = new NativeWindowSettings()
{
    Size = (800, 600),
    Title = "Rise Of Humanity",
};
using var game = new Game(GameWindowSettings.Default, settings);
game.Run();
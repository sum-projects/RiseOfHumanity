using System.Data.SqlTypes;
using OpenTK.Mathematics;

namespace RiseOfHumanity.Graphics;

public class Camera
{
    public Matrix4 Project;
    public Matrix4 View;
    public Matrix4 Model;

    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public Vector3 Up { get; set; }

    public float Speed { get; set; } = 2.0f;

    public Camera(int winWidth, int winHeight, int mapWidth, int mapHeight, int mapDepth)
    {
        var centerX = mapWidth / 2.0f;
        var centerY = mapHeight / 2.0f;
        var centerZ = mapDepth / 2.0f;

        Position = new Vector3(centerX, centerY, centerZ - 15.0f);
        Target = new Vector3(centerX, centerY, centerZ);
        Up = Vector3.UnitY;

        Project = Matrix4.CreatePerspectiveFieldOfView(1.0f, winWidth / (float)winHeight, 0.1f, 100.0f);
        View = Matrix4.LookAt(Position, Target, Up);
        Model = Matrix4.Identity;
    }

    public void UpdateViewMatrix()
    {
        View = Matrix4.LookAt(Position, Target, Up);
    }

    public void MoveForward(float amount)
    {
        var move = Vector3.UnitY * amount;
        Position += move;
        Target += move;
    }

    public void MoveBackward(float amount)
    {
        var move = Vector3.UnitY * amount;
        Position -= move;
        Target -= move;
    }

    public void MoveRight(float amount)
    {
        var move = Vector3.UnitX * amount;
        Position -= move;
        Target -= move;
    }

    public void MoveLeft(float amount)
    {
        var move = Vector3.UnitX * amount;
        Position += move;
        Target += move;
    }

    public void RotateCamera(float angle)
    {
        var translationToOrigin = Matrix4.CreateTranslation(-Target);
        var rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
        var translationBack = Matrix4.CreateTranslation(Target);

         Model *= translationToOrigin * rotation * translationBack;
    }
}
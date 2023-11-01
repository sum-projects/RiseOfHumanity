using OpenTK.Mathematics;

namespace RiseOfHumanity.Graphics;

public class Camera
{
    public Vector3 Position { get; set; } = new(0.0f, 0.0f, -3.0f);
    public Vector3 Target { get; set; } = Vector3.Zero;
    public Vector3 Up { get; set; } = Vector3.UnitY;

    public float Speed { get; set; } = 2.0f;

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Target, Up);
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
}
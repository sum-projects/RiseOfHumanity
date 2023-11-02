using OpenTK.Graphics.OpenGL4;

namespace RiseOfHumanity.Graphics;

public class Shader
{
    public int shader;
    public Shader(int program, string filepath, ShaderType type)
    {
        var source = File.ReadAllText(filepath);
        shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.AttachShader(program, shader);
        GL.LinkProgram(program);

        GL.DeleteShader(shader);
    }
}
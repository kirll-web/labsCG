using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace z1.Shaders;
public class Shader
{
    private readonly int _handle;
    private bool _disposedValue;

    public Shader(
        string vertexPath = "../../../Shaders/shader.vert",
        string fragmentPath = "../../../Shaders/shader.frag"
    )
    {
        var vertexShaderSource = File.ReadAllText(vertexPath);
        var fragmentShaderSource = File.ReadAllText(fragmentPath);

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);

        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out var successV);
        if (successV == 0)
        {
            var infoLog = GL.GetShaderInfoLog(vertexShader);
            throw new ArgumentException(infoLog);
        }

        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out var successF);
        if (successF == 0)
        {
            var infoLog = GL.GetShaderInfoLog(fragmentShader);
            throw new ArgumentException(infoLog);
        }

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);

        GL.LinkProgram(_handle);
        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out var successH);
        if (successH == 0)
        {
            var infoLog = GL.GetProgramInfoLog(_handle);
            throw new ArgumentException(infoLog);
        }

        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }
    public void SetMatrix4(string name, Matrix4 matrix)
    {
        var location = GetUniformLocation(name);
        GL.UniformMatrix4(location, false, ref matrix);
    }

    public void SetVector3(string name, Vector3 vector)
    {
        var location = GetUniformLocation(name);
        GL.Uniform3(location, vector);
    }


    public void SetInt(string name, int value)
    {
        var location = GetUniformLocation(name);
        GL.Uniform1(location, value);
    }

    public void SetFloat(string name, float value)
    {
        var location = GetUniformLocation(name);
        GL.Uniform1(location, value);
    }

    private int GetUniformLocation(string name)
    {
        return GL.GetUniformLocation(_handle, name);
    }

    ~Shader()
    {
        if (!_disposedValue)
        {
            throw new ArgumentException("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }
}

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace z1.FigureImpl
{
    public class Shader
    {
        private int _programID;

        public Shader(string vertexShaderCode, string fragmentShaderCode)
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);


            GL.ShaderSource(vertexShader, vertexShaderCode);
            GL.ShaderSource(fragmentShader, fragmentShaderCode);


            GL.CompileShader(vertexShader);
            GL.CompileShader(fragmentShader);


            CheckCompileErrors(vertexShader, "VERTEX");
            CheckCompileErrors(fragmentShader, "FRAGMENT");


            _programID = GL.CreateProgram();
            GL.AttachShader(_programID, vertexShader);
            GL.AttachShader(_programID, fragmentShader);
            GL.LinkProgram(_programID);

            CheckCompileErrors(_programID, "PROGRAM");
            
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private void CheckCompileErrors(int shader, string type)
        {
            if (type == "PROGRAM")
            {
                GL.GetProgram(shader, GetProgramParameterName.LinkStatus, out int success);
                if (success == 0)
                {
                    string log = GL.GetProgramInfoLog(shader);
                    Console.WriteLine($"Ошибка линковки программы:\n{log}");
                }
            }
            else
            {
                GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                {
                    string log = GL.GetShaderInfoLog(shader);
                    Console.WriteLine($"Ошибка компиляции шейдера ({type}):\n{log}");
                }
            }
        }

        public void Use()
        {
            GL.UseProgram(_programID);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_programID, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }
    }


    public class MobiusStrip
    {
        private int _vertexBuffer;
        private int _indexBuffer;
        private int _vao;
        private Shader _shader;
        private int _vertexCount;

        public MobiusStrip(float radius, float width, int uSegments, int vSegments)
        {
            GenerateGeometry(radius, width, uSegments, vSegments);
            SetupShaders();
        }

        private void GenerateGeometry(float R, float w, int uSteps, int vSteps)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();


            for (int i = 0; i <= uSteps; i++)
            {
                float u = (float)i / uSteps * 2 * MathHelper.Pi;

                for (int j = 0; j <= vSteps; j++)
                {
                    float v = (float)j / vSteps * 2 * w - w;
                    
                    float x = (R + v * (float)Math.Cos(u / 2)) * (float)Math.Cos(u);
                    float y = (R + v * (float)Math.Cos(u / 2)) * (float)Math.Sin(u);
                    float z = v * (float)Math.Sin(u / 2);

                    vertices.Add(new Vector3(x, y, z));
                }
            }


            for (int i = 0; i < uSteps; i++)
            {
                for (int j = 0; j < vSteps; j++)
                {
                    int a = i * (vSteps + 1) + j;
                    int b = (i + 1) * (vSteps + 1) + j;

                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(a + 1);

                    indices.Add(b);
                    indices.Add(b + 1);
                    indices.Add(a + 1);
                }
            }

            _vertexCount = indices.Count;
            
            GL.GenVertexArrays(1, out _vao);
            GL.GenBuffers(1, out _vertexBuffer);
            GL.GenBuffers(1, out _indexBuffer);

            GL.BindVertexArray(_vao);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes,
                vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int),
                indices.ToArray(), BufferUsageHint.StaticDraw);
            
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

            GL.BindVertexArray(0);
        }

        private void SetupShaders()
        {
            _shader = new Shader(
                @"#version 330 core
                layout(location = 0) in vec3 position;
                uniform mat4 mvp;
                out float zCoord;

                void main()
                {
                    zCoord = position.z;
                    gl_Position = mvp * vec4(position, 1.0); 
                }",
                @"#version 330 core
                    out vec4 FragColor;
                    in float zCoord;

                    void main()
                    {
                        float t = (zCoord + 1.0) * 0.5 - 0.3;
    
                        FragColor = vec4(t, t, t, t);
                    }");
        }

        public void Draw(Matrix4 viewProjection)
        {
            _shader.Use();
            _shader.SetMatrix4("mvp", viewProjection);

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _vertexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
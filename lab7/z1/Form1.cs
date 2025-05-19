using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using z1.presentation;

namespace z1;

public partial class Form1 : Form
{
    int _vao, _vbo, _shaderProgram, _vertexCount;

    public Form1()
    {
        InitializeComponent();
        glControl1.Load += (sender, e) => 
        {
            GL.ClearColor(1, 1, 1, 1.0f);

            var vertices = GenerateLineVertices(-10f, 10f, 0.5f);
            _vertexCount = vertices.Count;
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            GL.Enable(EnableCap.ProgramPointSize);
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _shaderProgram = CreateShaderProgram(vertexShaderSource, fragmentShaderSource);
        };
    }
    
    List<float> GenerateLineVertices(float start, float end, float step)
    {
        var verts = new List<float>();
        for (float x = start; x <= end; x += step)
        {
            float y =  x;
            float z = 0f;
            verts.Add(x);
            verts.Add(y);
            verts.Add(z);
        }
        return verts;
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(Color.DarkGray);
        GL.Enable(EnableCap.DepthTest);
        
        UpdateView();
        glControl1.Focus();
    }

    private void UpdateView()
    {
        float aspectRatio = (float)glControl1.Width / glControl1.Height;
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 100f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref perspective);
    }
    
    private void GlControlMouseDown(object sender, MouseEventArgs e)
    {
        glControl1.Focus();
    }
    
    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(_shaderProgram);
        GL.DrawArrays(PrimitiveType.Points, 0, _vertexCount/3); 
        GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertexCount/3); 
        
        glControl1.SwapBuffers();
    }
    
    int CreateShaderProgram(string vertexCode, string fragmentCode)
    {
        int vs = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vs, vertexCode);
        GL.CompileShader(vs);

        int fs = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fs, fragmentCode);
        GL.CompileShader(fs);

        int program = GL.CreateProgram();
        GL.AttachShader(program, vs);
        GL.AttachShader(program, fs);
        GL.LinkProgram(program);

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);

        return program;
    }

    //сделать измененение цвета от зелёного к красному
    string vertexShaderSource = @"
        #version 330 core
        layout(location = 0) in vec3 aPosition;
        out float xPos; // Pass x position to fragment shader
        
        void main() {
            xPos = aPosition.x;
            float y = 1;
            if (xPos != 0) {
                y = sin(xPos) / xPos;
            }
            gl_Position = vec4(aPosition.x * 0.1, y * 0.5, aPosition.z, 1.0);
            gl_PointSize = 10.0;
        }";

    string fragmentShaderSource = @"
        #version 330 core
        in float xPos; 
        out vec4 FragColor;
        
        void main() {
            float normalizedX = (xPos + 10.0) / 20.0; 
            
            vec3 color = mix(vec3(0.0, 1.0, 0.0), vec3(1.0, 0.0, 0.0), normalizedX);
            
            FragColor = vec4(color, 1.0);
        }";

    private void GlControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
        UpdateView();
    }
}

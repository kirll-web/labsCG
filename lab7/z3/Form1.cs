using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace z1
{
    public partial class Form1 : Form
    {
        private int _vao, _vbo, _shaderProgram, _vertexCount;
        private Vector2 _center = new Vector2(-0.5f, 0f);
        private float _zoom = 1.0f;
        private const float ZoomFactor = 1.2f;

        public Form1()
        {
            InitializeComponent();

            glControl1.Load += (sender, e) =>
            {
                GL.ClearColor(0, 0, 0, 1.0f);


                var vertices = new List<float>
                {
                    -1.0f, -1.0f, 0.0f,
                    1.0f, -1.0f, 0.0f,
                    1.0f, 1.0f, 0.0f,
                    -1.0f, 1.0f, 0.0f
                };

                _vertexCount = vertices.Count;
                _vao = GL.GenVertexArray();
                _vbo = GL.GenBuffer();

                GL.BindVertexArray(_vao);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(),
                    BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                _shaderProgram = CreateShaderProgram(vertexShaderSource, fragmentShaderSource);
            };
        }


        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            float moveStep = 0.1f / _zoom;

            switch (e.KeyCode)
            {
                case Keys.W:
                    _center.Y += moveStep;
                    break;
                case Keys.S:
                    _center.Y -= moveStep;
                    break;
                case Keys.A:
                    _center.X -= moveStep;
                    break;
                case Keys.D:
                    _center.X += moveStep;
                    break;
                case Keys.PageUp:
                    _zoom *= ZoomFactor;
                    break;
                case Keys.PageDown:
                    _zoom /= ZoomFactor;
                    break;
                default:
                    return;
            }

            glControl1.Invalidate();
        }


        private void GlControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            UpdateView();
            glControl1.Focus();
        }

        private void UpdateView()
        {
            glControl1.Focus();
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
        }

        private void GlControlPaint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(_shaderProgram);


            int centerLoc = GL.GetUniformLocation(_shaderProgram, "center");
            int zoomLoc = GL.GetUniformLocation(_shaderProgram, "zoom");
            int aspectLoc = GL.GetUniformLocation(_shaderProgram, "aspectRatio");

            GL.Uniform2(centerLoc, _center);
            GL.Uniform1(zoomLoc, _zoom);
            GL.Uniform1(aspectLoc, (float)glControl1.Width / glControl1.Height);

            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertexCount / 3);

            glControl1.SwapBuffers();
        }

        private int CreateShaderProgram(string vertexCode, string fragmentCode)
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

        string vertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            out vec2 uv;
            void main() {
                gl_Position = vec4(aPosition, 1.0);
                uv = aPosition.xy;
            }";

        string fragmentShaderSource = @"
#version 330 core
in vec2 uv;
out vec4 FragColor;

uniform vec2 center;
uniform float zoom;
uniform float aspectRatio;

vec3 palette(float t) {
    vec3 a = vec3(0.7, 0.3, 0.7);
    vec3 b = vec3(0.7, 0.6, 0.2);
    vec3 c = vec3(1.0, 1.0, 1.0);
    vec3 d = vec3(0.263, 0.416, 0.557);
    return a + b*cos(6.28318*(c*t+d));
}

void main() {
    vec2 c = uv;
    c.x *= aspectRatio;
    c = c / zoom + center;
    
    vec2 z = vec2(0.0);
    int max_iter = 28;
    int iter = 0;
    
    for(iter = 0; iter < max_iter; iter++) {
        z = vec2(z.x*z.x - z.y*z.y, 2.0*z.x*z.y) + c;
        if (dot(z, z) > 4.0) break;
    }
    
    float smoothed = float(iter) - log2(log2(dot(z, z))) + 4.0;
    float col = smoothed / float(max_iter);
    FragColor = vec4(palette(col), 1.0);
}";

        private void GlControlResize(object sender, EventArgs e)
        {
            UpdateView();
            glControl1.Invalidate();
        }
    }
}

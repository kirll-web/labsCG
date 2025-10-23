using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace z1
{
    //убрать изменение пропорций при изменении размеров окна
    public partial class Form1 : Form
    {
        private int _vao, _vbo, _shaderProgram, _vertexCount;

        public Form1()
        {
            InitializeComponent();

            glControl1.Load += (sender, e) =>
            {
                GL.ClearColor(1, 1, 1, 1.0f);


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

        private void GlControlLoad(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);

            UpdateView();
            glControl1.Focus();
        }

        private void UpdateView()
        {
            float aspectRatio = (float)glControl1.Width / glControl1.Height;
            Matrix4 perspective = Matrix4.CreateOrthographicOffCenter(-1, 1, -1, 1, 0.1f, 100f);
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
            float aspectRatio = (float)glControl1.Width / glControl1.Height;
            int aspectLoc = GL.GetUniformLocation(_shaderProgram, "aspectRatio");
            GL.Uniform1(aspectLoc, aspectRatio);
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, _vertexCount / 3);

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

            void main() {
    
            vec2 coord = uv;
            
            float dist = length(coord);
            float headRadius = 0.6;
            if (dist < headRadius) {
                
                FragColor = vec4(1.0, 1.0, 0.0, 1.0);
                
                vec2 leftEyePos = vec2(-0.2, 0.2);
                vec2 rightEyePos = vec2(0.2, 0.2);
                float eyeRadius = 0.05;
                
           
                float outlineThickness = 0.02;
            
                if (dist < headRadius + outlineThickness && dist > headRadius - outlineThickness) {
                    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                    return;
                }
                
                if (length(coord - leftEyePos) < eyeRadius) {
                    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                }
                
                if (length(coord - rightEyePos) < eyeRadius) {
                    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                }

                float smileRadius = 0.3;
                vec2 smileCenter = vec2(0.0, -0.05);
                float smileWidth = 0.4;
                
                float smileThickness = mix(0.001, 0.1, abs(coord.y - smileCenter.y));
               
                if (dist > smileRadius - smileThickness && 
                    dist < smileRadius + smileThickness && 
                    coord.y < smileCenter.y && 
                    abs(coord.x) < smileWidth) {
                    FragColor = vec4(0.0, 0.0, 0.0, 1.0);
                }
            } else {
                FragColor = vec4(1.0, 1.0, 1.0, 1.0);
            }
        }";

        private void GlControlResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            UpdateView();
        }
    }
}

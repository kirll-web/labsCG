using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;

namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Form1()
        {
            InitializeComponent();
            п.Load += GlControl1_Load;
            glControl1.Paint += GlControl1_Paint;
            glControl1.Resize += GlControl1_Resize;
        }

        private void GlControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.Black);
            SetupViewport();
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            // Пример отрисовки треугольника
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(System.Drawing.Color.Red);
            GL.Vertex2(0.0, 1.0);
            GL.Color3(System.Drawing.Color.Green);
            GL.Vertex2(-1.0, -1.0);
            GL.Color3(System.Drawing.Color.Blue);
            GL.Vertex2(1.0, -1.0);
            GL.End();

            glControl1.SwapBuffers();
        }

        private void GlControl1_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.Viewport(0, 0, w, h);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1, 1, -1, 1, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
        }
    }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
        }

        #endregion
    }
}

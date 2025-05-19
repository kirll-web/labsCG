using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using z1.utils;

namespace z1;

public partial class Form1 : Form
{

    private Model3D model;
    
    public Form1()
    {
        InitializeComponent(); 
        glControl1.Load += (sender, e) => 
        {
            GL.ClearColor(Color.DarkGray);
            GL.Enable(EnableCap.DepthTest);
    
            model = new Model3D("assets/ship3.obj"); // Только теперь грузим модель!
            Console.WriteLine($"Loaded model with {model.vertexCount} vertices.");
            UpdateView();
            glControl1.Focus();
        };
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        Console.WriteLine(e.KeyValue);
    }

    private void Form1_KeyUp(object sender, KeyEventArgs e)
    {
        Console.WriteLine(e.KeyValue);
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
        GL.MatrixMode(MatrixMode.Modelview);
    }
    
    private void GlControlMouseDown(object sender, MouseEventArgs e)
    {
        glControl1.Focus();
    }

    
    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        Matrix4 lookAt = Matrix4.LookAt(
            new Vector3(0, 0, 5),   // Камера стоит в точке (0,0,5)
            Vector3.Zero,           // Смотрит в (0,0,0)
            Vector3.UnitY           // Ось вверх
        );
        GL.LoadMatrix(ref lookAt);
        
        GL.Color3(Color.Aqua);
        GL.CullFace(CullFaceMode.Front);
        model.Render();

        GL.CullFace(CullFaceMode.Back);
        model.Render();

        glControl1.SwapBuffers();
    }


    private void GlControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
        UpdateView();
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
      
    }
}

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using z1.FigureImpl;

namespace z1;

public partial class Form1 : Form
{
    private float _xRotate = 0f;    
    private float _yRotate = 0f;    
    private bool _isClicking = false;
    private Point _lastPos = Point.Empty;
    private SnubDodecahedron _snubDodecahedron;
    private const float RotationSensitivity = 0.005f; // Чувствительность вращения
    
    public Form1()
    {
        InitializeComponent();
        _snubDodecahedron = new SnubDodecahedron();
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(Color.White);
        GL.Enable(EnableCap.DepthTest);
        float aspectRatio = (float)Width / Height;

        Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), aspectRatio, 0.1f, 100);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref p);

        Matrix4 modelview = Matrix4.LookAt(20, 20, 20, 0, 0, 0, 0, 1, 0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);
    }

    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Enable(EnableCap.DepthTest);
        GL.Rotate(_xRotate, Vector3.UnitX);
        GL.Rotate(_yRotate, Vector3.UnitY);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);
        //отрисовка лабиринта
        GL.CullFace(CullFaceMode.Back);
        //отрисовка лабиринта
        glControl1.SwapBuffers();
    }

    private void GlControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, Width, Height);
        float aspectRatio = (float)Width / Height;
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), aspectRatio, 0.1f, 100);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref perspective);
    }
    
    private void GlControlMouseDown(object sender, MouseEventArgs args)
    {
        _lastPos = args.Location;
        _isClicking = true;
    }
    
    private void GlControlMouseUp(object sender, MouseEventArgs args)
    {
        _lastPos = Point.Empty;
        _isClicking = false;
    }

    private void GlControlMouseMove(object sender, MouseEventArgs args)
    {
        if (!_isClicking)
        {
            return;
        }
        float dx = args.Location.X - _lastPos.X;
        float dy = args.Location.Y - _lastPos.Y;
      
        
        _lastPos = args.Location;

        // Накопление углов с учетом чувствительности
        _xRotate += dx * RotationSensitivity;
        _yRotate += dy * RotationSensitivity;

        glControl1.Invalidate();
    }
    
}

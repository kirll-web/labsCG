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
    private Matrix4 _viewProjection;
    private float _angle;
    private MobiusStrip _mobius;
    private const float RotationSensitivity = 0.1f; // Чувствительность вращения
    
    public Form1()
    {
        InitializeComponent();
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(Color.Cyan);
        GL.Enable(EnableCap.DepthTest);
        float aspectRatio = (float)Width / Height;
        _mobius = new MobiusStrip(radius: 15.0f, width: 5.0f, uSegments: 500, vSegments: 100);

        Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), aspectRatio, 0.1f, 100);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref p);

        Matrix4 modelview = Matrix4.LookAt(20, 20, 20, 0, 0, 0, 0, 1, 0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);
            
        _viewProjection = modelview * p;
    }

    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        var modelY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_yRotate));
        
        var modelX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_xRotate));
        var model = modelX * modelY;
        var mvp = model * _viewProjection;
        
        _mobius.Draw(mvp);
        
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

        _xRotate += dx * RotationSensitivity;
        _yRotate += dy * RotationSensitivity;

        glControl1.Invalidate();
    }
    
}

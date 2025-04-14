using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using z1.FigureImpl;
using z1.presentation;
using Timer = System.Windows.Forms.Timer;

namespace z1;

public partial class Form1 : Form
{
    private LabirintViewModel _viewModel;
    private readonly Timer updateTimer; 
    private Cube _cube;
    private bool[] _keysPressed;
    private float _moveSpeed = 0.1f;
    private float _rotationSpeed = 2f;
    
    public Form1()
    {
        InitializeComponent();
        updateTimer = new Timer
        {
            Interval = 16 
        };
        
        _viewModel = new LabirintViewModel();
        _keysPressed = new bool[256];
        
        updateTimer.Tick += UpdateTimer_Tick; 
        updateTimer.Start(); 
        glControl1.Load += (sender, e) => 
        {
            _cube = new Cube(); 
        };
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        Console.WriteLine(e.KeyValue);
        _keysPressed[e.KeyValue] = true;
    }

    private void Form1_KeyUp(object sender, KeyEventArgs e)
    {
        Console.WriteLine(e.KeyValue);
        _keysPressed[e.KeyValue] = false;
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        GL.ClearColor(Color.DarkGray);
        GL.Enable(EnableCap.DepthTest);
        
        UpdateView();
    }

    private void UpdateView()
    {
        float aspectRatio = (float)glControl1.Width / glControl1.Height;
        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 100f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref perspective);
        
        Matrix4 lookat = Matrix4.LookAt(
            _viewModel.PlayerX, _viewModel.PlayerY, _viewModel.PlayerZ,
            _viewModel.PlayerX + (float)Math.Sin(_viewModel.PlayerRotation), 
            _viewModel.PlayerY, 
            _viewModel.PlayerZ + (float)Math.Cos(_viewModel.PlayerRotation),
            0, 1, 0);
            
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref lookat);
    }


    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);
        for (int z = 0; z < _viewModel.Map.Length; z++)
        {
            for (int x = 0; x < _viewModel.Map[z].Length; x++)
            {
                if (_viewModel.Map[z][x] != 0)
                {
                    _cube.DrawCube(x, 0, z);
                }
            }
        }
        GL.Begin(BeginMode.Polygon);
        GL.Color3(Color.Olive);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0, _viewModel.Map.Length);
        GL.Vertex3(_viewModel.Map.Length, 0 , _viewModel.Map.Length);
        GL.Vertex3(_viewModel.Map.Length, 0, 0);
        GL.End();
        
        GL.Begin(BeginMode.Polygon);
        GL.Color3(Color.Aqua);
        GL.Vertex3(0, 1, 0);
        GL.Vertex3(0, 1, _viewModel.Map.Length);
        GL.Vertex3(_viewModel.Map.Length, 1 , _viewModel.Map.Length);
        GL.Vertex3(_viewModel.Map.Length, 1, 0);
        GL.End();
        GL.CullFace(CullFaceMode.Back);
        for (int z = 0; z < _viewModel.Map.Length; z++)
        {
            for (int x = 0; x < _viewModel.Map[z].Length; x++)
            {
                if (_viewModel.Map[z][x] != 0)
                {
                    _cube.DrawCube(x, 0, z);
                }
            }
        }
        

        
        glControl1.SwapBuffers();
    }

    private void GlControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
        UpdateView();
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
        bool moved = false;
        
        if (_keysPressed[(int)Keys.W])
        {
            Console.WriteLine("move w");
            float newX = _viewModel.PlayerX + (float)Math.Sin(_viewModel.PlayerRotation) * _moveSpeed;
            float newZ = _viewModel.PlayerZ + (float)Math.Cos(_viewModel.PlayerRotation) * _moveSpeed;


            if (_viewModel.CanMoveTo(newX, newZ))
            {
                _viewModel.PlayerX = newX;
            }
         
            if (_viewModel.CanMoveTo(newX, newZ))
            {
                _viewModel.PlayerZ = newZ;
            }
         
                
            moved = true;
        }
        
        if (_keysPressed[(int)Keys.S])
        {
            float newX = _viewModel.PlayerX - (float)Math.Sin(_viewModel.PlayerRotation) * _moveSpeed;
            float newZ = _viewModel.PlayerZ - (float)Math.Cos(_viewModel.PlayerRotation) * _moveSpeed;
            
            
            if (_viewModel.CanMoveTo(newX, newZ))
            {
                _viewModel.PlayerX = newX;
            }
         
            if (_viewModel.CanMoveTo(newX, newZ))
            {
                _viewModel.PlayerZ = newZ;
            }
            moved = true;
        }
        
        if (_keysPressed[(int)Keys.A])
        {
            _viewModel.PlayerRotation += _rotationSpeed * 0.01f;
            moved = true;
        }
        
        if (_keysPressed[(int)Keys.D])
        {
            _viewModel.PlayerRotation -= _rotationSpeed * 0.01f;
            moved = true;
        }
        
        if (moved)
        {
            UpdateView();
            glControl1.Invalidate();
        }
    }
}

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using z1.Shaders;

namespace z1;

public partial class Form1 : Form
{
    int _vaoPlane, _vaoPlaneDown, _vboPlane,  _vboPlaneDown, _shaderProgram, _vertexCount;
    private List<int> _vaoCube = [], _vboCube = [];
    private static int cubeCount = 2;
    Shader _shader;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    static Vector3 _cameraPos = new(3f, 3f, 3f);

    bool _isMousePressed = false;
    Point _lastMousePos;
    float _sensitivity = 0.2f;

    private float _verticalAngle = 30f;
    private float _horizontalAngle = -90f;
    private const float CameraDistance = 2.5f;

    private readonly Vector3 _lightPos = new(CameraDistance * 1, CameraDistance * 1, CameraDistance);
    private readonly Vector3 _lightColor = new(1f, 1f, 1f);
 
    float centerX = -0.5f;
    float centerY =  width / 2;
    float centerZ = -0.5f;
    
    float centerPlaneX = -1f;
    float centerPlaneY = 0;
    float centerPlaneZ = -1f;
    
    static float width = 0.3f; 
    private float cubeOffset = width * 2;

    private List<Vector3> cubeCenters = [];

    public Form1()
    {
        InitializeComponent();
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        _shader = new Shader();
        GL.ClearColor(Color.DarkGray);
        GL.Enable(EnableCap.DepthTest);
       

        for (int i = 0; i < cubeCount; i++)
        {
            cubeCenters.Add(new Vector3(centerX + i * cubeOffset, centerY, centerZ + i * cubeOffset));
            CreateCube( cubeCenters[i].X,  cubeCenters[i].Y,  cubeCenters[i].Z, width);
        }
        
        CreatePlane();
        UpdateView();
  
    }

    private void CreateCube(float centerX, float centerY, float centerZ, float width )
    {
        var cubeVertices = GenerateCubeVertices(centerX, centerY, centerZ, width);
        
        var vao = GL.GenVertexArray();
        var vbo = GL.GenBuffer();
        _vaoCube.Add(vao);
        _vboCube.Add(vbo);
        
        GL.BindVertexArray(vao);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, cubeVertices.Length * sizeof(float), cubeVertices,
            BufferUsageHint.StaticDraw);

        
        int stride = 9 * sizeof(float);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0); 
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float)); 
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float)); 
        GL.EnableVertexAttribArray(2);

 
        GL.BindVertexArray(0);
    }


    private float[] GenerateCubeVertices(float x, float y, float z, float width)
    {
        // Грани куба: {позиция x, y, z, нормаль nx, ny, nz, color r, g, b}
        var vertices = new List<float>();

        
        AddFace(vertices,
            new Vector3(x, y, z),
            new Vector3(x + width, y, z),
            new Vector3(x + width, y, z + width),
            new Vector3(x, y, z + width),
            new Vector3(0.0f, -1.0f, 0.0f), 
            new Vector3(1.0f, 0.0f, 0.0f) 
        );

        // Верхняя грань (грань "смотрит" вверх)
        AddFace(vertices,
            new Vector3(x, y + width, z),
            new Vector3(x + width, y + width, z),
            new Vector3(x + width, y + width, z + width),
            new Vector3(x, y + width, z + width),
            new Vector3(0.0f, 1.0f, 0.0f), 
            new Vector3(0.0f, 1.0f, 0.0f) 
        );

        // Передняя грань (грань "смотрит" вперед)
        AddFace(vertices,
            new Vector3(x, y, z + width),
            new Vector3(x + width, y, z + width),
            new Vector3(x + width, y + width, z + width),
            new Vector3(x, y + width, z + width),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 1.0f) 
        );

        // Задняя грань (грань "смотрит" назад)
        AddFace(vertices,
            new Vector3(x, y, z),
            new Vector3(x + width, y, z),
            new Vector3(x + width, y + width, z),
            new Vector3(x, y + width, z),
            new Vector3(0.0f, 0.0f, -1.0f), 
            new Vector3(1.0f, 1.0f, 0.0f)
        );

        // Левая грань (грань "смотрит" влево)
        AddFace(vertices,
            new Vector3(x, y, z),
            new Vector3(x, y, z + width),
            new Vector3(x, y + width, z + width),
            new Vector3(x, y + width, z),
            new Vector3(-1.0f, 0.0f, 0.0f), 
            new Vector3(1.0f, 0.0f, 1.0f) 
        );

        // Правая грань (грань "смотрит" вправо)
        AddFace(vertices,
            new Vector3(x + width, y, z),
            new Vector3(x + width, y, z + width),
            new Vector3(x + width, y + width, z + width),
            new Vector3(x + width, y + width, z),
            new Vector3(1.0f, 0.0f, 0.0f), 
            new Vector3(1.0f, 1.0f, 1.0f) 
        );

        return vertices.ToArray();
    }
    
    
    private void AddFace(List<float> vertices, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 normal,
        Vector3 color)
    {
        
        AddVertex(vertices, v1, normal, color);
        AddVertex(vertices, v2, normal, color);
        AddVertex(vertices, v3, normal, color);

        
        AddVertex(vertices, v3, normal, color);
        AddVertex(vertices, v4, normal, color);
        AddVertex(vertices, v1, normal, color);
    }


    private void AddVertex(List<float> vertices, Vector3 position, Vector3 normal, Vector3 color)
    {
        vertices.Add(position.X);
        vertices.Add(position.Y);
        vertices.Add(position.Z);
        vertices.Add(normal.X);
        vertices.Add(normal.Y);
        vertices.Add(normal.Z);
        vertices.Add(color.X);
        vertices.Add(color.Y);
        vertices.Add(color.Z);
    }

    private void CreatePlane()
    {
        float size = 1.0f; 
        Vector3 normal = new Vector3(0.0f, 1.0f, 0.0f); 
        Vector3 color = new Vector3(0.0f, 1f, 0.0f); 
        
        var planeVertices = GeneratePlaneVertices(centerPlaneX, centerPlaneY, centerPlaneZ, size * 2, normal, color);
        
        _vaoPlane = GL.GenVertexArray();
        _vboPlane = GL.GenBuffer();
        GL.BindVertexArray(_vaoPlane);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPlane);
        GL.BufferData(BufferTarget.ArrayBuffer, planeVertices.Length * sizeof(float), planeVertices,
            BufferUsageHint.StaticDraw);

        
        int stride = 9 * sizeof(float);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0); 
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float)); 
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float)); 
        GL.EnableVertexAttribArray(2);

        
        GL.BindVertexArray(0);
        
        Vector3 normalDown = new Vector3(0.0f, -1.0f, 0.0f); 
        var planeVerticesDown = GeneratePlaneVertices(centerPlaneX, centerPlaneY - 0.001f, centerPlaneZ, size * 2, normalDown, color);
        _vaoPlaneDown = GL.GenVertexArray();
        _vboPlaneDown = GL.GenBuffer();
        GL.BindVertexArray(_vaoPlaneDown);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPlaneDown);
        GL.BufferData(BufferTarget.ArrayBuffer, planeVerticesDown.Length * sizeof(float), planeVerticesDown,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0); 
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float)); 
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float)); 
        GL.EnableVertexAttribArray(2);
        
        
        GL.BindVertexArray(0);
    }


    private float[] GeneratePlaneVertices(float x, float y, float z, float size, Vector3 normal, Vector3 color)
    {
        var vertices = new List<float>();

        
        Vector3 bottomLeft = new Vector3(x, y, z);
        Vector3 bottomRight = new Vector3(x + size, y, z);
        Vector3 topRight = new Vector3(x + size, y, z + size);
        Vector3 topLeft = new Vector3(x, y, z + size);

        
        AddFace(vertices, bottomLeft, bottomRight, topRight, topLeft, normal, color);

        return vertices.ToArray();
    }
    

    private void UpdateView()
    {
        
        float horizontalAngleRad = MathHelper.DegreesToRadians(_horizontalAngle);
        float verticalAngleRad = MathHelper.DegreesToRadians(_verticalAngle);

        float x = CameraDistance * (float)(Math.Cos(verticalAngleRad) * Math.Cos(horizontalAngleRad));
        float y = CameraDistance * (float)(Math.Sin(verticalAngleRad));
        float z = CameraDistance * (float)(Math.Cos(verticalAngleRad) * Math.Sin(horizontalAngleRad));

        _cameraPos = new Vector3(x, y, z);

        _view = Matrix4.LookAt(_cameraPos, Vector3.Zero, Vector3.UnitY);

        glControl1.Invalidate();

        _projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45f),
            (float)glControl1.Width / (float)glControl1.Height,
            0.1f,
            100f);
    }

    private void GlControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
    }

    private void GLControlMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isMousePressed) return;
        Console.WriteLine(e.X);
        float deltaX = e.X - _lastMousePos.X;
        float deltaY = e.Y - _lastMousePos.Y;
        _lastMousePos = e.Location;

        _horizontalAngle += deltaX * _sensitivity;
        _verticalAngle += deltaY * _sensitivity;

        _verticalAngle = Math.Clamp(_verticalAngle, -89f, 89f);

        UpdateView();
    }

    private void GLControlMouseDown(object sender, MouseEventArgs e)
    {
        if (!_isMousePressed)
        {
            _isMousePressed = true;
            _lastMousePos = e.Location;
        }
    }

    private void GlControlMouseUp(object sender, MouseEventArgs e)
    {
        if (_isMousePressed)
        {
            _isMousePressed = false;
        }
    }


    private void GlControlPaint(object sender, PaintEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();
        SetMatrixToShader();
        SetLightToShader();
        _shader.SetInt("cubeCount", cubeCount);
        for (int i = 0; i < cubeCount; i++)
        {
            _shader.SetVector3("cubeCentres", cubeCenters[i]);
            _shader.SetFloat("cubeSizes", width);

            
            GL.BindVertexArray(_vaoCube[i]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 100);
        }
        
        GL.BindVertexArray(_vaoPlane);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 100);
        
        GL.BindVertexArray(_vaoPlaneDown);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 100);
        UpdateView();
        glControl1.SwapBuffers();
    }

    private void SetMatrixToShader()
    {
        _shader.SetMatrix4("model", _model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);
    }

    private void SetLightToShader()
    {
        _shader.SetVector3("lightPos", _lightPos);
        _shader.SetVector3("lightColor", _lightColor);
        _shader.SetVector3("viewPos", _cameraPos);
    }
}

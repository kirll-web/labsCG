using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using z1.Shaders;

namespace z1;

public struct BufferData(int vao, int vertexCount)
{
    public int Vao = vao;
    public int VertexCount = vertexCount;
}

public struct Torus(Vector3 position, float torRadius, float pipeRadius, Vector3 color)
{
    public Vector3 Position = position;
    public readonly float TorRadius = torRadius;
    public readonly float PipeRadius = pipeRadius;
    public readonly Vector3 Color = color;
    public Matrix4 ModelMatrix = Matrix4.CreateTranslation(position);
}

public class Shapes
{
    private readonly List<Torus> _shapes;

    public Shapes(List<Torus> shapes)
    {
        _shapes = shapes;
        CreatePoints();
    }

    private readonly List<BufferData> _torusBuffers = [];

    public List<Torus> GetShapes() => _shapes;

    public void Paint(Shader shader)
    {
        for (int i = 0; i < _shapes.Count; i++)
        {
            shader.SetMatrix4("model", _shapes[i].ModelMatrix);

            GL.BindVertexArray(_torusBuffers[i].Vao);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, _torusBuffers[i].VertexCount);
        }

        GL.BindVertexArray(0);
    }

    public void CreatePoints()
    {
        int slices = 50;
        int stacks = 30;

        for (int i = 0; i < _shapes.Count; i++)
        {
            float[] points = CreatePoints(_shapes[i], _shapes[i].Color, slices, stacks);
            _torusBuffers.Add(CreateBufferData(points));
        }
    }

    private static float[] CreatePoints(Torus torus, Vector3 color, int slices, int stacks)
    {
        List<float> data = [];

        for (int i = 0; i < slices; i++)
        {
            float u0 = (float)i / slices * 2 * MathF.PI;
            float u1 = (float)(i + 1) / slices * 2 * MathF.PI;

            for (int j = 0; j <= stacks; j++)
            {
                float v = (float)j / stacks * 2 * MathF.PI;

                AddVertex(data, torus, u0, v, color);
                AddVertex(data, torus, u1, v, color);
            }
        }

        return data.ToArray();
    }

    private static void AddVertex(List<float> data, Torus torus, float u, float v, Vector3 color)
    {
        var torRadius = torus.TorRadius;
        var pipeRadius = torus.PipeRadius;

        var cosU = MathF.Cos(u);
        var sinU = MathF.Sin(u);
        var cosV = MathF.Cos(v);
        var sinV = MathF.Sin(v);

        var x = (torRadius + pipeRadius * cosV) * cosU;
        var y = (torRadius + pipeRadius * cosV) * sinU;
        var z = pipeRadius * sinV;

        Vector3 pos = new(x, y, z);

        Vector3 normal = Vector3.Normalize(new(cosV * cosU, cosV * sinU, sinV));

        data.AddRange(
        [
            pos.X, pos.Z, pos.Y,
            normal.X, normal.Z, normal.Y,
            color.X, color.Y, color.Z
        ]);
    }


    private static BufferData CreateBufferData(float[] points)
    {
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);
        ConfigurateShaderLayout();

        GL.BindVertexArray(0);

        return new BufferData(vao, points.Length / 9);
    }

    private static void ConfigurateShaderLayout()
    {
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);
    }
}

public partial class Form1 : Form
{
    Shader _shader;

    Matrix4 _model = Matrix4.Identity;
    Matrix4 _view = Matrix4.Identity;
    Matrix4 _projection = Matrix4.Identity;

    Vector3 _cameraPos = new(1, 1f, 1f);

    private bool _isMousePressed;
    private Point _lastMousePos;
    private const float Sensitivity = 0.2f;

    private float _verticalAngle = 30f;
    private float _horizontalAngle = -90f;
    private const float CameraDistance = 2f;
    private Shapes _shapes;

    private readonly Vector3 _lightPos = new(CameraDistance * 1, CameraDistance * 1, CameraDistance);
    private readonly Vector3 _lightColor = new(1f, 1f, 1f);


    public Form1()
    {
        InitializeComponent();
    }

    private void GlControlLoad(object sender, EventArgs e)
    {
        _shader = new Shader();
        _shapes = new Shapes(
            new List<Torus>
            {
                new Torus(new Vector3(0, 0, 0), 0.31f, 0.12f, new Vector3(0, 1f, 0)),
                new Torus(new Vector3(0, 0.2f, 0), 0.25f, 0.12f, new Vector3(1f, -0.8f, 0)),
                new Torus(new Vector3(0, 0.4f, 0), 0.19f, 0.12f, new Vector3(0, 0.8f, 1f)),
            }
        );
        GL.ClearColor(Color.DarkGray);
        GL.Enable(EnableCap.DepthTest);


        UpdateView();
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
            (float)glControl1.Width / glControl1.Height,
            0.1f,
            100f);
    }

    private void GlControlResize(object sender, EventArgs e)
    {
        GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
    }

    private void GlControlMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isMousePressed) return;
        Console.WriteLine(e.X);
        float deltaX = e.X - _lastMousePos.X;
        float deltaY = e.Y - _lastMousePos.Y;
        _lastMousePos = e.Location;

        _horizontalAngle += deltaX * Sensitivity;
        _verticalAngle += deltaY * Sensitivity;

        _verticalAngle = Math.Clamp(_verticalAngle, -89f, 89f);

        UpdateView();
    }

    private void GlControlMouseDown(object sender, MouseEventArgs e)
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
        List<Torus> toruses = _shapes.GetShapes();
        _shader.SetInt("torusCount", toruses.Count);

        for (int i = 0; i < toruses.Count; i++)
        {
            _shader.SetVector3($"torusPositions[{i}]", toruses[i].Position);
            _shader.SetFloat($"torRadius[{i}]", toruses[i].TorRadius);
            _shader.SetFloat($"pipeRadius[{i}]", toruses[i].PipeRadius);
        }

        _shapes.Paint(_shader);

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

using OpenTK.Mathematics;

namespace Task1.Figure;

public class Face
{
    List<Point3F> _vertexes;
    private Color4 _color { get; }

    public Color4 Color => _color;
    public IReadOnlyList<Point3F> Vertexes => _vertexes;

    public Face(Color4 color, List<Point3F> vertexes)
    {
        _color = color;
        _vertexes = vertexes;
    }
}

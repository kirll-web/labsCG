using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Task1.Figure;

namespace z1.FigureImpl
{
    partial class SnubDodecahedron
    {
        private List<Vector3> _vertexes;
        private List<Face> _faces;

        public SnubDodecahedron()
        {
            _vertexes = new List<Vector3>();
            _faces = new List<Face>();
            _vertexes = ComputeCuboctahedronVertices(10);
            _faces = GetFaces();
        }


        public void Draw()
        {
            foreach (var face in _faces)
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Color4(face.Color);
                foreach (var faceVertex in face.Vertexes)
                {
                    GL.Vertex3(faceVertex.X, faceVertex.Y, faceVertex.Z);
                }
                GL.End();
            }

            foreach (var face in _faces)
            {
                GL.Color3(Color.Black);
                Point3F currentPoint = face.Vertexes[0];
                for (int i = 1; i < face.Vertexes.Count; i++)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(currentPoint.X, currentPoint.Y, currentPoint.Z);
                    GL.Vertex3(face.Vertexes[i].X, face.Vertexes[i].Y, face.Vertexes[i].Z);
                    currentPoint = face.Vertexes[i];
                    GL.End();
                }

                GL.Vertex3(currentPoint.X, currentPoint.Y, currentPoint.Z);
                GL.Vertex3(face.Vertexes[0].X, face.Vertexes[0].Y, face.Vertexes[0].Z);
            }
        }


        private List<Face> GetFaces()
        {
            List<int[]> squareFaceIndexes = GetSquareFaceIndices();
            List<Face> faces = new List<Face>();
            Color4 transparentRed = new Color4(1.0f, 0.0f, 0.0f, 0.3f);
            Color4 transparentBlue = new Color4(0.0f, 0.0f, 1.0f, 0.3f);
            for (int i = 0; i < squareFaceIndexes.Count; i++)
            {
                List<Point3F> vertexes = new List<Point3F>();
                for (int k = 0; k < squareFaceIndexes[i].Length; k++)
                {
                    vertexes.Add(new Point3F(_vertexes[squareFaceIndexes[i][k]].X, _vertexes[squareFaceIndexes[i][k]].Y, _vertexes[squareFaceIndexes[i][k]].Z));
                }

                faces.Add(new Face(transparentRed, vertexes));
            }

            List<int[]> triangleFaceIndexes = GetTriangleFaceIndices();
            for (int b = 0; b < triangleFaceIndexes.Count; b++)
            {
                List<Point3F> vertexes = new List<Point3F>();
                for (int z = 0; z < triangleFaceIndexes[b].Length; z++)
                {
                    vertexes.Add(new Point3F(this._vertexes[triangleFaceIndexes[b][z]].X,
                        this._vertexes[triangleFaceIndexes[b][z]].Y, this._vertexes[triangleFaceIndexes[b][z]].Z));
                }

                faces.Add(new Face(transparentBlue, vertexes));
            }

            return faces;
        }

        private static List<Vector3> ComputeCuboctahedronVertices(float R)
        {
            return new List<Vector3>
            {
                new Vector3(R, R, 0),
                new Vector3(R, -R, 0),
                new Vector3(-R, R, 0),
                new Vector3(-R, -R, 0),
                new Vector3(R, 0, R),
                new Vector3(R, 0, -R),
                new Vector3(-R, 0, R),
                new Vector3(-R, 0, -R),
                new Vector3(0, R, R),
                new Vector3(0, R, -R),
                new Vector3(0, -R, R),
                new Vector3(0, -R, -R)
            };
        }

        private static List<int[]> GetSquareFaceIndices()
        {
            return new List<int[]>
            {
                new[] { 0, 4, 1, 5 },
                new[] { 2, 6, 3, 7 },

                new[] { 0, 8, 2, 9 },
                new[] { 1, 10, 3, 11 },

                new[] { 4, 8, 6, 10 },
                new[] { 5, 9, 7, 11 }
            };
        }

        private static List<int[]> GetTriangleFaceIndices()
        {
            return new List<int[]>
            {
                new[] { 0, 8, 4 },
                new[] { 0, 9, 5 },
                new[] { 1, 10, 4 },
                new[] { 1, 11, 5 },
                new[] { 2, 8, 6 },
                new[] { 2, 9, 7 },
                new[] { 3, 10, 6 },
                new[] { 3, 11, 7 }
            };
        }
    }

    partial class SnubDodecahedron
    {
        private float angle = 9.0f;

        public void DrawCube()
        {
            var width = 30f;
            /*задняя*/
            GL.Color3(Color.Red);
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*левая*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            /*нижняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, 0, 0);
            GL.End();

            /*верхняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(0, width, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            /*передняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            /*правая*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, width, 0);
            GL.Vertex3(width, width, 0);
            GL.Vertex3(width, 0, 0);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(width, 0, 0);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(width, width, 0);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(width, 0, width);
            GL.Vertex3(width, width, width);
            GL.Vertex3(0, width, width);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, width);
            GL.Vertex3(0, width, width);
            GL.Vertex3(0, width, 0);
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(50, 0, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 50, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 50);
            GL.End();
        }
    }
}
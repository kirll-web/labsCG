using OpenTK.Mathematics;

namespace z1.utils;

using Assimp;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

public class Model3D
{
    private int vao;
    private List<int> vboList = new(); 
    public int vertexCount; // добавили

    public Model3D(string path)
    {
        LoadModel(path);
    }

    private void LoadModel(string path)
    {
        var importer = new Assimp.AssimpContext();
        var scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);

        if (scene == null || scene.RootNode == null)
            throw new Exception("Ошибка загрузки модели.");

        // Списки для вершин и граней
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        // Обрабатываем все сетки (meshes) в сцене
        foreach (var mesh in scene.Meshes)
        {
            // Добавляем вершины
            foreach (var vertex in mesh.Vertices)
            {
                vertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }

            // Обрабатываем грани
            foreach (var face in mesh.Faces)
            {
                foreach (var index in face.Indices)
                {
                    indices.Add(index); // Индексы вершин для грани
                }
            }
        }

        // Преобразование в OpenGL
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        vboList.Add(vbo);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

        int stride = Vector3.SizeInBytes;

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);

        // Создание EBO (Element Buffer Object) для индексов
        int ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);

        vertexCount = indices.Count; // Количество индексов
    }

    private void ProcessNode(Assimp.Node node, Assimp.Scene scene)
    {
        foreach (int meshIndex in node.MeshIndices)
        {
            ProcessMesh(scene.Meshes[meshIndex], scene);
        }

        foreach (var child in node.Children)
        {
            ProcessNode(child, scene);
        }
    }

    private void ProcessMesh(Assimp.Mesh mesh, Assimp.Scene scene)
    {
        float[] vertices = new float[mesh.VertexCount * 8];

        for (int i = 0; i < mesh.VertexCount; i++)
        {
            vertices[i * 8 + 0] = mesh.Vertices[i].X;
            vertices[i * 8 + 1] = mesh.Vertices[i].Y;
            vertices[i * 8 + 2] = mesh.Vertices[i].Z;

            vertices[i * 8 + 3] = mesh.Normals[i].X;
            vertices[i * 8 + 4] = mesh.Normals[i].Y;
            vertices[i * 8 + 5] = mesh.Normals[i].Z;

            if (mesh.HasTextureCoords(0))
            {
                vertices[i * 8 + 6] = mesh.TextureCoordinateChannels[0][i].X;
                vertices[i * 8 + 7] = mesh.TextureCoordinateChannels[0][i].Y;
            }
            else
            {
                vertices[i * 8 + 6] = 0;
                vertices[i * 8 + 7] = 0;
            }
        }

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        int vbo = GL.GenBuffer();
        vboList.Add(vbo);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        int stride = 8 * sizeof(float);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);

        vertexCount = mesh.VertexCount; // сохраняем реальное число вершин!
    }

    public void Render()
    {
        GL.BindVertexArray(vao);
        GL.DrawArrays(OpenTK.Graphics.OpenGL4.PrimitiveType.Quads, 0, vertexCount); // используем правильное значение
    }
}

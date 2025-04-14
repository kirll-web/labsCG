using OpenTK.Graphics.OpenGL;
using z1.util;


namespace z1.FigureImpl
{
    public class Sphere
    {
        private int _textureID;
        private int _displayList;
        
        public Sphere()
        {
            TextureLoader textureLoader = new TextureLoader();
            _textureID = textureLoader.Load("./Textures/8.jpg");
            _displayList = GL.GenLists(1);
            CreateSphere(90f, 32, 32);
        }
        
        private void CreateSphere(float radius, int slices, int stacks)
        {
            GL.NewList(_displayList, ListMode.Compile);
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
            
            GL.FrontFace(FrontFaceDirection.Cw);
            
            for (int i = 0; i < stacks; ++i)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i) / stacks);
                double z0 = Math.Sin(lat0);
                double zr0 = Math.Cos(lat0);
                
                double lat1 = Math.PI * (-0.5 + (double)(i + 1) / stacks);
                double z1 = Math.Sin(lat1);
                double zr1 = Math.Cos(lat1);
                
                GL.Begin(BeginMode.QuadStrip);
                for (int j = 0; j <= slices; ++j)
                {
                    double lng = 2 * Math.PI * (double)(j) / slices;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);
                    
                    GL.TexCoord2((double)j / slices, (double)i / stacks);
                    GL.Normal3(-x * zr0, -y * zr0, -z0);
                    GL.Vertex3(x * zr0 * radius, y * zr0 * radius, z0 * radius);
                    
                    GL.TexCoord2((double)j / slices, (double)(i + 1) / stacks);
                    GL.Normal3(-x * zr1, -y * zr1, -z1);
                    GL.Vertex3(x * zr1 * radius, y * zr1 * radius, z1 * radius);
                }
                GL.End();
            }
            
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.EndList();
        }
        
        public void Draw()
        {
            GL.CallList(_displayList);
        }
    }
}

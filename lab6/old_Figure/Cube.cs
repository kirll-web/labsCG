using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using z1.presentation;
using z1.util;

namespace z1.FigureImpl
{
    partial class Cube
    {
        private List<int> _textures = new List<int>();
        
        public  Cube()
        {
            TextureLoader textureLoader = new TextureLoader();
            _textures.Add(textureLoader.Load("./Textures/1.jpg"));
            _textures.Add(textureLoader.Load("./Textures/2.jpg"));
            _textures.Add(textureLoader.Load("./Textures/3.jpg"));
            _textures.Add(textureLoader.Load("./Textures/4.png"));
            _textures.Add(textureLoader.Load("./Textures/5.png"));
            _textures.Add(textureLoader.Load("./Textures/6.png"));
            _textures.Add(textureLoader.Load("./Textures/7.png"));
        }
        
        public void Draw(int x, int y, int z, int wallType = 0)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textures[wallType]);
            var width = 1f;
            var offsetX = x * width;
            var offsetY = y * width;
            var offsetZ = z * width;

            GL.Color3(Color.Gray);
            
           
            /*задняя*/
            GL.Begin(BeginMode.Polygon);
            GL.TexCoord2(0, 0); 
            GL.Vertex3(offsetX, offsetY, offsetZ);
            GL.TexCoord2(1, 0); 
            GL.Vertex3(offsetX + width, offsetY, offsetZ);
            GL.TexCoord2(1, 1);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ);
            GL.TexCoord2(0, 1);
            GL.Vertex3(offsetX, offsetY + width, offsetZ);
            GL.End();

            
            /*левая*/
            GL.Begin(BeginMode.Polygon);
            GL.TexCoord2(0, 0); 
            GL.Vertex3(offsetX, offsetY, offsetZ);
            GL.TexCoord2(1, 0); 
            GL.Vertex3(offsetX, offsetY, offsetZ + width);
            GL.TexCoord2(1, 1);
            GL.Vertex3(offsetX, offsetY + width, offsetZ + width);
            GL.TexCoord2(0, 1);
            GL.Vertex3(offsetX, offsetY + width, offsetZ);
            GL.End();
            
            /*нижняя*/
            GL.Begin(BeginMode.Polygon);
            GL.TexCoord2(0, 0); 
            GL.Vertex3(offsetX, offsetY, offsetZ);
            GL.TexCoord2(1, 0); 
            GL.Vertex3(offsetX, offsetY, offsetZ + width);
            GL.TexCoord2(1, 1);
            GL.Vertex3(offsetX + width, offsetY, offsetZ + width);
            GL.TexCoord2(0, 1);
            GL.Vertex3(offsetX + width, offsetY, offsetZ);
            GL.End();
            
            /*верхняя*/
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(offsetX, offsetY + width, offsetZ);
            GL.Vertex3(offsetX, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ);
            GL.End();
            

            /*передняя*/
            GL.Begin(BeginMode.Polygon);
            GL.TexCoord2(0, 0); 
            GL.Vertex3(offsetX, offsetY, offsetZ + width);
            GL.TexCoord2(1, 0); 
            GL.Vertex3(offsetX + width, offsetY, offsetZ + width);
            GL.TexCoord2(1, 1);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ + width);
            GL.TexCoord2(0, 1);
            GL.Vertex3(offsetX, offsetY + width, offsetZ + width);
            GL.End();
            
            /*правая*/
            GL.Begin(BeginMode.Polygon);
            GL.TexCoord2(0, 0); 
            GL.Vertex3(offsetX + width, offsetY, offsetZ);
            GL.TexCoord2(1, 0); 
            GL.Vertex3(offsetX + width, offsetY, offsetZ + width);
            GL.TexCoord2(1, 1);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ + width);
            GL.TexCoord2(0, 1);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ);
            GL.End();
        }
    }
}

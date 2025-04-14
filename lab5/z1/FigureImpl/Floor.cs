using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using z1.presentation;
using z1.util;

namespace z1.FigureImpl
{
    partial class Floor
    {
        private int _textureFloor;
        
        public Floor()
        {
            TextureLoader textureLoader = new TextureLoader();
            _textureFloor =  textureLoader.Load("./Textures/1.jpg");
        }
      
        
        public void Draw(int x, int y, int z)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textureFloor);
            var width = 1f;
            var offsetX = x * width;
            var offsetY = y * width;
            var offsetZ = z * width;

            
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
        }
    }
}

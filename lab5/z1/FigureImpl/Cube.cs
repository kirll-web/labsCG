using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using z1.presentation;

namespace z1.FigureImpl
{
    partial class Cube
    {
        private int _textureID;
        public  Cube()
        { 
            _textureID = LoadTexture("./Textures/6.png");
        }
        
        private int LoadTexture(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Texture file not found at {path}");

            Bitmap bitmap = new Bitmap(path);
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);

            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            // Настройки фильтрации и оборачивания
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return texID;
        }
        
        public void DrawCube(int x, int y, int z)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
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
            
            GL.LineWidth(5f);
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(offsetX, offsetY, offsetZ);
            GL.Vertex3(offsetX + width, offsetY, offsetZ);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ);
            GL.Vertex3(offsetX, offsetY + width, offsetZ);
            GL.End();
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(offsetX, offsetY, offsetZ);
            GL.Vertex3(offsetX, offsetY, offsetZ + width);
            GL.Vertex3(offsetX, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX, offsetY + width, offsetZ);
            GL.End();
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(offsetX, offsetY, offsetZ);
            GL.Vertex3(offsetX, offsetY, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY, offsetZ);
            GL.End();  
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(offsetX, offsetY + width, offsetZ);
            GL.Vertex3(offsetX, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ);
            GL.End();
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(offsetX, offsetY, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX, offsetY + width, offsetZ + width);
            GL.End();
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(offsetX + width, offsetY, offsetZ);
            GL.Vertex3(offsetX + width, offsetY, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ + width);
            GL.Vertex3(offsetX + width, offsetY + width, offsetZ);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
    }
}

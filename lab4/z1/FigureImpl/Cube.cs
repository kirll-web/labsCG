using OpenTK.Graphics.OpenGL;
using z1.presentation;

namespace z1.FigureImpl
{
    partial class Cube
    {
        private int _textureID;
        public  Cube()
        { 
        }
        
        public void DrawCube(int x, int y, int z)
        {
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
            
        }
    }
}

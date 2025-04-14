using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace z1.util;

public class TextureLoader
{
    public int Load(string path)
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

}

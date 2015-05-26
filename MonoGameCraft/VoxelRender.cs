using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameCraft
{
    class VoxelRender
    {
        public Texture2D Texture { get; private set; }
        Color[] TextureData;
        Random rand = new Random();

        public VoxelRender(GraphicsDevice device, int width, int height)
        {
            Texture = new Texture2D(device, width, height);
            TextureData = new Color[width * height];
        }

        public void Update(float timeStep)
        {

            for (int y = 0; y < Texture.Height; ++y)
            {
                for (int x = 0; x < Texture.Width; ++x)
                {
                    TextureData[x + (y * Texture.Width)] = new Color(rand.Next(256), rand.Next(256), rand.Next(256));
                }
            }

            Texture.SetData<Color>(TextureData);
        }
    }
}

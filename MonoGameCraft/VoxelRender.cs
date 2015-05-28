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
        Color[] pixels;

        int w = 212 * 2;
        int h = 120 * 2;

        int [] map = new int[64 * 64 * 64];
        int [] texmap = new int[16 * 16 * 3 * 16];

        Random rand = new Random();

        public VoxelRender(GraphicsDevice device)
        {
            Texture = new Texture2D(device, w, h);
            pixels = new Color[w * h];
        }
        
        // Replacement for the javascript Math.random()
        public double random()
        {
            return rand.NextDouble();
        }

        // Initialise everything
        // Notes:
        //  Replaced use of ("some expression" | 0) with relevant cast to int or byte
        public void Init()
        {
            for (var i = 1; i < 16; i++)
            {
                var br = 255 - (byte)((random() * 96));
                for (var y = 0; y < 16 * 3; y++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var color = 0x966C4A;
                        if (i == 4)
                            color = 0x7F7F7F;
                        if (i != 4 || (int)((random() * 3)) == 0)
                        {
                            br = 255 - (byte)((random() * 96));
                        }
                        if ((i == 1 && y < (((x * x * 3 + x * 81) >> 2) & 3) + 18))
                        {
                            color = 0x6AAA40;
                        }
                        else if ((i == 1 && y < (((x * x * 3 + x * 81) >> 2) & 3) + 19))
                        {
                            br = br * 2 / 3;
                        }
                        if (i == 7)
                        {
                            color = 0x675231;
                            if (x > 0 && x < 15
                                    && ((y > 0 && y < 15) || (y > 32 && y < 47)))
                            {
                                color = 0xBC9862;
                                var xd = (x - 7);
                                var yd = ((y & 15) - 7);
                                if (xd < 0)
                                    xd = 1 - xd;
                                if (yd < 0)
                                    yd = 1 - yd;
                                if (yd > xd)
                                    xd = yd;

                                br = 196 - (byte)((random() * 32)) + xd % 3 * 32;
                            }
                            else if ((int)((random() * 2)) == 0)
                            {
                                br = br * (150 - (x & 1) * 100) / 100;
                            }
                        }

                        if (i == 5)
                        {
                            color = 0xB53A15;
                            if ((x + (y >> 2) * 4) % 8 == 0 || y % 4 == 0)
                            {
                                color = 0xBCAFA5;
                            }
                        }
                        if (i == 9)
                        {
                            color = 0x4040ff;
                        }
                        var brr = br;
                        if (y >= 32)
                            brr /= 2;

                        if (i == 8)
                        {
                            color = 0x50D937;
                            if ((int)((random() * 2)) == 0)
                            {
                                color = 0;
                                brr = 255;
                            }
                        }

                        var col = (((color >> 16) & 0xff) * brr / 255) << 16
                                | (((color >> 8) & 0xff) * brr / 255) << 8
                                | (((color) & 0xff) * brr / 255);
                        texmap[x + y * 16 + i * 256 * 3] = col;
                    }
                }
            }

            for (var x = 0; x < 64; x++)
            {
                for (var y = 0; y < 64; y++)
                {
                    for (var z = 0; z < 64; z++)
                    {
                        var i = z << 12 | y << 6 | x;
                        var yd = (y - 32.5) * 0.4;
                        var zd = (z - 32.5) * 0.4;
                        map[i] = (int)(random() * 16);
                        if (random() > Math.Sqrt(Math.Sqrt(yd * yd + zd * zd)) - 0.8)
                            map[i] = 0;
                    }
                }
            }

            pixels = new Color[w * h];
            
            for (var i = 0; i < w * h; i++)
            {
                pixels[i] = Color.White;
            }

        }

        double DateNow()
        {
            return (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        void renderMinecraft()
        {
            var xRot = Math.Sin(DateNow() % 10000 / 10000 * Math.PI * 2) * 0.4
                    + Math.PI / 2;
            var yRot = Math.Cos(DateNow() % 10000 / 10000 * Math.PI * 2) * 0.4;
            var yCos = Math.Cos(yRot);
            var ySin = Math.Sin(yRot);
            var xCos = Math.Cos(xRot);
            var xSin = Math.Sin(xRot);

            var ox = 32.5 + DateNow() % 10000 / 10000 * 64;
            var oy = 32.5;
            var oz = 32.5;

            for (var x = 0; x < w; x++)
            {
                double ___xd = (double)(x - w / 2) / (double)h;
                for (var y = 0; y < h; y++)
                {
                    double __yd = (double)(y - h / 2) / (double)h;
                    var __zd = 1;

                    var ___zd = __zd * yCos + __yd * ySin;
                    var _yd = __yd * yCos - __zd * ySin;

                    var _xd = ___xd * xCos + ___zd * xSin;
                    var _zd = ___zd * xCos - ___xd * xSin;

                    var col = 0;
                    var br = 255;
                    var ddist = 0;

                    var closest = 32.0;
                    for (var d = 0; d < 3; d++)
                    {
                        var dimLength = _xd;
                        if (d == 1)
                            dimLength = _yd;
                        if (d == 2)
                            dimLength = _zd;

                        var ll = 1 / (dimLength < 0 ? -dimLength : dimLength);
                        var xd = (_xd) * ll;
                        var yd = (_yd) * ll;
                        var zd = (_zd) * ll;

                        var initial = ox - (int)(ox);
                        if (d == 1)
                            initial = oy - (int)(oy);
                        if (d == 2)
                            initial = oz - (int)(oz);
                        if (dimLength > 0)
                            initial = 1 - initial;

                        var dist = ll * initial;

                        var xp = ox + xd * initial;
                        var yp = oy + yd * initial;
                        var zp = oz + zd * initial;

                        if (dimLength < 0)
                        {
                            if (d == 0)
                                xp--;
                            if (d == 1)
                                yp--;
                            if (d == 2)
                                zp--;
                        }

                        while (dist < closest)
                        {
                            var tex = map[(((int)zp & 63) << 12) | (((int)yp & 63) << 6) | (((int)xp & 63))];

                            if (tex > 0)
                            {
                                var u = (int)((xp + zp) * 16) & 15;
                                var v = ((int)(yp * 16) & 15) + 16;
                                if (d == 1)
                                {
                                    u = (int)(xp * 16) & 15;
                                    v = ((int)(zp * 16) & 15);
                                    if (yd < 0)
                                        v += 32;
                                }

                                var cc = texmap[u + v * 16 + tex * 256 * 3];
                                if (cc > 0)
                                {
                                    col = cc;
                                    ddist = 255 - (int)((dist / 32 * 255));
                                    br = 255 * (255 - ((d + 2) % 3) * 50) / 255;
                                    closest = dist;
                                }
                            }
                            xp += xd;
                            yp += yd;
                            zp += zd;
                            dist += ll;
                        }
                    }

                    var r = ((col >> 16) & 0xff) * br * ddist / (255 * 255);
                    var g = ((col >> 8) & 0xff) * br * ddist / (255 * 255);
                    var b = ((col) & 0xff) * br * ddist / (255 * 255);

                    pixels[x + (y * w)] = new Color((byte)r, (byte)g, (byte)b);
                }
            }
        }

        public void Update(float timeStep)
        {
            renderMinecraft();
            Texture.SetData<Color>(pixels);
        }
    }
}

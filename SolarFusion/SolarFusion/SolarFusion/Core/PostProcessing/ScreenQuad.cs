using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core.PostProcessing
{
    public class ScreenQuad
    {
        GraphicsDevice _obj_graphics;
        VertexBuffer _obj_vb;
        short[] _obj_ib;
        VertexDeclaration _obj_vertDec;
        VertexPositionTexture[] mCorners;

        public ScreenQuad(GraphicsDevice _graphics)
        {
            this._obj_graphics = _graphics;
            this.mCorners = new VertexPositionTexture[4];
            this.mCorners[0].Position = new Vector3(0, 0, 0);
            this.mCorners[0].TextureCoordinate = Vector2.Zero;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize()
        {
            this._obj_vertDec = VertexPositionTexture.VertexDeclaration;

            this.mCorners = new VertexPositionTexture[]
                    {
                        new VertexPositionTexture(
                            new Vector3(1,-1,0),
                            new Vector2(1,1)),
                        new VertexPositionTexture(
                            new Vector3(-1,-1,0),
                            new Vector2(0,1)),
                        new VertexPositionTexture(
                            new Vector3(-1,1,0),
                            new Vector2(0,0)),
                        new VertexPositionTexture(
                            new Vector3(1,1,0),
                            new Vector2(1,0))
                    };

            this._obj_ib = new short[] { 0, 1, 2, 2, 3, 0 };
            this._obj_vb = new VertexBuffer(this._obj_graphics, typeof(VertexPositionTexture), this.mCorners.Length, BufferUsage.None);
            this._obj_vb.SetData(this.mCorners);
        }

        public virtual void Draw()
        {
            this._obj_graphics.SetVertexBuffer(this._obj_vb);
            this._obj_graphics.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, this.mCorners, 0, 4, this._obj_ib, 0, 2);
        }
    }
}

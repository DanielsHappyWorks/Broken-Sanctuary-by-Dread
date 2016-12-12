using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class VertexData<T> : IVertexData where T : struct, IVertexType
    {
        #region Variables
        private T[] vertices;
        private PrimitiveType primitiveType;
        private int primitiveCount;
        #endregion

        #region Properties
        public PrimitiveType PrimitiveType
        {
            get
            {
                return this.primitiveType;
            }
        }
        public int PrimitiveCount
        {
            get
            {
                return this.primitiveCount;
            }
        }
        public T[] Vertices
        {
            get
            {
                return this.vertices;
            }
            set
            {
                this.vertices = value;
            }
        }
        #endregion

        public VertexData(T[] vertices,
            PrimitiveType primitiveType, int primitiveCount)
        {
            this.vertices = vertices;
            this.primitiveType = primitiveType;
            this.primitiveCount = primitiveCount;
        }

        public virtual void Draw(GameTime gameTime, Effect effect)
        {
            effect.GraphicsDevice.DrawUserPrimitives<T>(this.primitiveType, this.vertices, 0, this.primitiveCount);
        }

        public object Clone()
        {
            return new VertexData<T>(this.Vertices, this.PrimitiveType, this.PrimitiveCount);
        }
    }
}

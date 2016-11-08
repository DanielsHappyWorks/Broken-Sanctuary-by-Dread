using System;
using GDApp;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Actor : IActor, ICloneable
    {
        #region Variables
        public static Main game;

        private string id;
        private ActorType actorType;
        #endregion

        #region Properties
        public ActorType ActorType
        {
            get
            {
                return this.actorType;
            }
            set
            {
                this.actorType = value;
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        #endregion
        
        public Actor(string id, ActorType actorType)
        {
            this.id = id;
            this.actorType = actorType;
        }
        public virtual void Update(GameTime gameTime)
        {           
        }
        public virtual void Draw(GameTime gameTime)
        {       
        }

        public virtual Matrix GetWorldMatrix()
        {
            return Matrix.Identity; //does nothing - see derived classes especially CollidableObject
        }

        public object Clone()
        {
            return this.MemberwiseClone(); //deep because all variables are either C# types, structs, or enums
        }
    }
}

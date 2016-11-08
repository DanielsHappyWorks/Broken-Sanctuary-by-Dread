/*
Function: 		Demo class to explore ICloneable, IDisposable, GameComponent, DrawableGameComponent and other methods
Author: 		NMCG
Version:		1.0
Date Updated:	26/9/16
Bugs:			None
Fixes:			None
*/
namespace GDLibrary
{
    public class Sprite
    {
        #region Variables
        private string name; 
        private int x, y;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
        #endregion

        #region Constructors
        public Sprite(string name, int x, int y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
        #endregion

        #region Other
        public override string ToString()
        {
            return "Name: " + this.name
                        + "\tX: " + this.x 
                            + "\tY: " + this.y;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.Name.GetHashCode();
            hash = hash * 17 + this.X.GetHashCode();
            hash = hash * 13 + this.Y.GetHashCode();
            return hash;  
        }

        public override bool Equals(object obj)
        {
            Sprite s = (Sprite)obj;

            if((s.Name.Equals(this.name)
                && (s.X == this.X) && (s.Y == this.Y)))
                return true;
            
            return false;
        }

        public virtual void Reset()
        {
            System.Diagnostics.Debug.WriteLine("Reset:Sprite with name: " 
                    + this.name);
        }

        #endregion
    }
}

/*
Function: 		Store, update, and draw all visible UI objects based on GenericDrawableManager
Author: 		NMCG
Version:		1.0
Date Updated:	29/11/16
Bugs:			None
Fixes:			None
*/
using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    public class UIManager : GenericDrawableManager
    {
        #region Variables
        private bool bMouseVisible;
        #endregion

        #region Properties
        public bool MouseVisible
        {
            get
            {
                return this.bMouseVisible;
            }
            set
            {
                this.Game.IsMouseVisible = this.bMouseVisible = value;
            }
        }
        #endregion

        public UIManager(Main game, string name, int initialSize, bool bMouseVisible)
            : base(game, name, initialSize)
        {
            this.MouseVisible = bMouseVisible;
        }

        public override void Draw(GameTime gameTime)
        {    
            if (!this.PauseDraw)
            {
                this.Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                foreach (IActor actor in this.OpaqueDrawList)
                {
                    Actor2D actor2D = actor as Actor2D;

                    if((actor2D != null) && ((actor2D.StatusType & StatusType.Drawn) == StatusType.Drawn))
                        actor.Draw(gameTime);
                }

                this.Game.SpriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

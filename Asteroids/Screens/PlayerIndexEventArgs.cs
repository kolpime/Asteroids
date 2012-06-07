#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace Asteroids.Screens
{

    class PlayerIndexEventArgs : EventArgs
    {

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex;
    }
}

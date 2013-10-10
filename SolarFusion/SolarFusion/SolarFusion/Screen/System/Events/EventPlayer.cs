using System;
using Microsoft.Xna.Framework;

namespace SolarFusion.Screen.System.Events
{
    public class EventPlayer : EventArgs
    {
        PlayerIndex? _player_index;

        public EventPlayer(PlayerIndex? pplayerindex)
        {
            this._player_index = pplayerindex;
        }

        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public PlayerIndex? PlayerIndex
        {
            get { return this._player_index; }
        }
    }
}

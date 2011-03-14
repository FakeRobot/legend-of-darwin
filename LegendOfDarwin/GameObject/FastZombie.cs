using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfDarwin.GameObject
{
    class FastZombie : Zombie
    {
   
        public static new int ZOMBIE_MOVE_RATE = 100;

        public Boolean sleeping = true;

        public FastZombie(int startX, int startY, int mymaxX, int myminX, int mymaxY, int myminY, GameBoard myboard) :
            base(startX, startY, mymaxX, myminX, mymaxY, myminY, myboard)
        {
            
        }

        /// <summary>
        ///
        /// </summary>
        public void wakeUp()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void runFast(Darwin darwin)
        {

        }

    }
}

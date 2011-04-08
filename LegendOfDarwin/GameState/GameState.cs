using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfDarwin
{
    class GameState
    {
        public enum state { Start, Start2, Level, End };
        private state myState;

        public GameState()
        {
            myState = new state();
            myState = state.Start;
        }

        public state getState()
        {
            return myState;
        }

        public void setState(state toSet)
        {
            myState = toSet;
        }

    }
}

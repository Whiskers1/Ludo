﻿using System;
using System.Linq;

namespace Ludo2
{
    public enum TokenState { Home, InPlay, Safe }; //Used to define if the token is on the main gameboard

    public class Token
    {
        private readonly int tokenId; // the unique id of the token
        private readonly GameColor color; //The color of the token
        private TokenState state; //Defines if the token is Home, safe or in play
        private int positionId = 0; //defines the position of the tokens //FIX needs better definition...
        /*private readonly int startPos;*/

        //---------------- Constructor ----------------
        public Token(int tokenId, GameColor color/*, int startPos*/)
        {
            //Self explanatory
            this.tokenId = tokenId;
            this.color = color;
            this.state = TokenState.Home; //sets the default state to 'Home'
            //this.startPos = startPos;
        }

        //---------------- Getters ----------------

        //Gets the id of the currently selected token
        public int GetTokenId()
        {
            return this.tokenId;
        }

        //Gets the color of the currently selected token
        public GameColor GetColor()
        {
            return this.color;
        }

        /*public int TokenStart
        {
            get => startPos;
        }*/

        public TokenState TokenState
        {
            get => state;
            set => state = value;
        }

        public int TokenPosition
            {
            get => positionId;
            set => positionId = value;
            }

    }
}

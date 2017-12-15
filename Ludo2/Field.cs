﻿using System;
using System.Linq;

namespace Ludo2
{

    public class Field
    {
        private GameColor color; //used if there is a token on the field
        private readonly int fieldId; //Every field needs an id
        private Token[] tokens = new Token[2]; //creates an array to hold up to two tokens at the same time

        //---------------- Constructor ----------------
        public Field(int fieldId, GameColor color = GameColor.None)
        {
            //Self explanatory
            this.fieldId = fieldId;
            this.color = color;
        }

        //Places the token on the field
        public bool PlaceToken(Token token, GameColor color, int dieRoll)
        {
            if(tokens.Any()) //checks if there is any tokens on the field
            {
                if(token.GetColor() != this.color) //FIX Tokens will currently always return false when trying to move
                {
                    //Make the Kill function to send the enemy token home
                    //Probably KillToken(TokenToKill);
                    tokens[0] = token;
                    this.color = token.GetColor();

                    token.SetPosition(fieldId + dieRoll);

                    return false;
                }
                else
                {
                    tokens[1] = token; //Insert the token into the array
                    return true;
                }
            }
            else //No tokens found
            {
                tokens[0] = token;
                this.color = token.GetColor();
                return true;
            }
        }

        private void KillToken(Token enemyToken)
        {
            //TODO Make The Code To Reset A Token
        }

        //---------------- Getters ----------------
        
        //Gets the color of the field
        public GameColor GetFieldColor()
        {
            return this.color;
        }

        //Gets the id of the field
        public int GetFieldId()
        {
            return this.fieldId;
        }

        //----->>>WARNING<<<-----
        //NOT USELESS BUT WILL GET REMOVED LATER (hopefully)
        
        //Checks if there is a token on the field
        public bool IsTokenPlaced()
        {
            switch(color)
            {
                case (GameColor.None):
                    return false;
                default:
                    return true;
            }
        }
    }
}

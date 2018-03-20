﻿using System;
using System.Linq;

namespace Ludo2
{
    public class Dice
    {
        //private int diceValue; //The variable to hold the value of the ThrowDice

        //---------------- Constructor ----------------
        public Dice()
        {
            //rolls the die at least once so that ther always will be a value
            this.ThrowDice();
        }
        
        //Throws the die
        public int ThrowDice()
        {
            Random rand = new Random(); //Creates a new object from the class 'Random'

            this.GetValue = rand.Next(1, 7); //gets a random value from 1 - 6

            return this.GetValue;
        }

        /// <summary>
        /// Gets the value of the last throw
        /// </summary>
        public int GetValue { get; private set; }

        public override string ToString()
        {
            return "Value: " + this.GetValue;
        }
    }
}

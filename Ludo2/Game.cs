﻿using System;
using System.Linq;

namespace Ludo2
{
    //defining the different enums used in the program
    public enum GameColor { Yellow, Blue, Red, Green, None}; //The colors that can be used --notice 'None' is used for fields
    public enum GameState { InPlay, Finished}; //Tells if the game is finished or if it is currently running

    public class Game
    {
        
        private GameState state; //Defines a gamestate variable

        private bool hasMoveSucceded; //Debug purpose

        private readonly int numberOfPlayers; //Defines the number of players in the game
        private Player[] players; //defines the array of players
        private int plrArrayId = 0; //Defines the player's turn
        private Field[] fields; //Defines the fields in the game

        Dice die = new Dice(); //Makes an object of the class 'Dice'

        //---------------- Constructor ----------------
        public Game()
        {

            //IMPLEMENT Music
            //MusicHandler.DeathSound();

            Design.Clear(500);
            this.numberOfPlayers = SetNumberOfPlayers(); //Sets the number of players before the game begins
            CreatePlayers(); //This method creates the players
            CreateField(); //Creates the fields used in the game
            GetPlayers();
            GetField();
            this.state = GameState.InPlay; //Changes the gamestate to play since we're actually beginning to play the game
            Turn(); //Begins player one's turn
        }

        #region Initialization


        //Sets the number of players before the game begins
        private int SetNumberOfPlayers()
        {
            int numOfPlayers = 0;

            Console.Write("How many players?: "); //Asks for how many players there will be in this game

            while(numOfPlayers < 2 || numOfPlayers > 4) //hecks if there is less than 2 or more than 4
            {
                if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out numOfPlayers)) //Tries to save the input as 'this.numberOfPlayers'
                {
                    Console.WriteLine(); //Makes a blank space
                    Design.Clear(160);
                    Console.Write("Unknown input, choose between 2 and 4 players: ");
                }
            }
            return numOfPlayers;
        }

        //--------------------- DIVIDER ---------------------

        //This method creates the players
        private void CreatePlayers()
        {
            this.players = new Player[this.numberOfPlayers]; //makes the player array

            Console.WriteLine();
            for (int i = 0; i < this.numberOfPlayers; i++) //Runs until all users have names
            {
                Design.Clear(300);
                Console.Write("What is the name of player {0}: ", (i+1)); //Asks for the players name
                string name = Console.ReadLine(); //saves the name as a temporary variable called 'name'
                
                Token[] token = TokenAssign(i); //Assigns the tokens for the different users

                players[i] = new Player(name, (i + 1), token); //Defines the players
            }
        }

        //--------------------- DIVIDER ---------------------

        //Assigns the tokens -- used in the method above
        private Token[] TokenAssign(int index)
        {
            Token[] tokens = new Token[4]; //Makes a new array

            int startPos = StartpointAssignment(index);

            int endPos = startPos - 2;

            if (endPos < 0)
            {
                endPos = 50;
            }

            for (int i = 0; i <= 3; i++) //runs four times
            {
                switch (index) //gives the same color to the tokens as the player
                {
                    case 0:
                        tokens[i] = new Token((i + 1), GameColor.Yellow, startPos, endPos); //Defines the color for the token
                        break;
                    case 1:
                        tokens[i] = new Token((i + 1), GameColor.Blue, startPos, endPos); //Defines the color for the token
                        break;
                    case 2:
                        tokens[i] = new Token((i + 1), GameColor.Red, startPos, endPos); //Defines the color for the token
                        break;
                    case 3:
                        tokens[i] = new Token((i + 1), GameColor.Green, startPos, endPos); //Defines the color for the token
                        break;
                }
            }
            return tokens;
        }

        //--------------------- DIVIDER ---------------------

        private int StartpointAssignment(int i)
        {
            int startPoint = 0; //the startpoint used by the player
            switch (i)
            {
                case 0:
                    startPoint = 0;
                    break;
                case 1:
                    startPoint = 13;
                    break;
                case 2:
                    startPoint = 26;
                    break;
                case 3:
                    startPoint = 39;
                    break;
            }
            return startPoint;
        }

        //--------------------- DIVIDER ---------------------

        //Creates the fields used in the game
        private void CreateField()
        {
            fields = new Field[57]; //creates the field array

            for(int i = 0; i < 57; i++)
            {
                fields[i] = new Field(i + 1); //gives the fields the correct data
            }
        }

        #endregion

        //--------------------- DIVIDER ---------------------

        //Each players turn
        private void Turn()
        {
            while(state == GameState.InPlay) //Checks if the game is on
            {
                Player turn = players[(plrArrayId)]; //Finds the player in the array
                Design.Clear(200);
                Console.WriteLine("It is " + turn.GetName() + "'s turn\n"); //Some 'nice' output
                do
                {
                    Design.Clear();
                    Console.Write("Press 'K' to roll the die: ");
                }
                while (Console.ReadKey().KeyChar != 'k');
                Console.WriteLine();
                Design.Clear(300);
                Console.WriteLine("You got: " + die.ThrowDice().ToString());//.tostring is completely useless
                Console.WriteLine();
                CanIMove(turn.GetTokens()); //Checks if the player can move
            }
        }

        //--------------------- DIVIDER ---------------------

        //OPTIMIZE CanIMove
        //Checks if the player can move
        private void CanIMove(Token[] tokens)
        {
            //IMPLEMENT Throw die more than once per player

            int choice = 0; //How many tokens can the player move

            Console.WriteLine("Here are your pieces:");
            Design.WriteLine("", 70);
            foreach (Token tkn in tokens) //Begins to write the tokens of the player
            {
                Console.Write("piece number: " + tkn.GetTokenId() + " are placed: " + tkn.TokenState); //Writes the id and state of each of the tokens
                switch (tkn.TokenState) //Begins to check if the player can do anything with his/hers tokens
                {
                    case TokenState.Home:
                        if(die.GetValue() == 6)
                        {
                            Console.Write(" - Can move");
                            choice++; //Can move this token AKA a choice
                            tkn.CanMove = true;
                        }
                        else
                        {
                            Console.Write(" - Can not move");
                            tkn.CanMove = false;
                        }
                        break;
                    case TokenState.Finished:
                        Console.Write(" <- Is finished");
                        tkn.CanMove = false;
                        break;
                    default:
                        Console.Write(" <- Can move : " + tkn.TokenPosition + " ");
                        choice++;
                        tkn.CanMove = true;
                        break;
                }
                Design.WriteLine("", 120);
            }
            Design.WriteLine("<------------------------------------------------>", 4500);
            Console.WriteLine("You have " + choice.ToString() + " options in this turn\n");

            if(choice == 0) //Cant do anything this turn skips the player
            {
                ChangeTurn();
            }
            else
            {
                MoveToField(players , die.GetValue());
                ChangeTurn();
            }
        }

        //--------------------- DIVIDER ---------------------

        //Changes the turn to the next player
        private void ChangeTurn()
        {
            Console.WriteLine();
            if(plrArrayId == (numberOfPlayers - 1))
            {
                plrArrayId = 0;
            }
            else
            {
                plrArrayId++;
            }

            Console.WriteLine("Changing player\n");
            Turn();
        }

        //--------------------- DIVIDER ---------------------

        //Lets the player choose the token to move
        private int ChooseTokenToMove()
        {
            int tokenToMove = 0; //Only used in this method

            Console.WriteLine("Choose a piece to move (Use a number between 1 and 4)");
            while (tokenToMove < 1 || tokenToMove > 4)
            {
                if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out tokenToMove))
                {
                    Console.WriteLine();
                    Console.WriteLine("Unknown input, Use a number between 1 and 4");
                }
            }
            return tokenToMove -1;
        }

        //--------------------- DIVIDER ---------------------

        //Moves the token
        //OPTIMIZE MoveToField
        private void MoveToField(Player[] plr, int dieRoll)
        {
            foreach (Player pl in plr)
            {
                Console.WriteLine("Player: " + pl.GetName() + " " + pl.GetId() + "\n");
            }
            Token turnToken = plr[plrArrayId].GetToken(ChooseTokenToMove());

            int fieldToRemove = turnToken.TokenPosition; //The field to remove the token from
            int fieldToMove = turnToken.TokenPosition + (die.GetValue()); //The field the token should move to
            int dieCount = dieRoll; //Temporary variable used to get and modify the dieRoll
            int dieRest; //How many dots left on the die

            while (dieCount > 0)
            {
                if (turnToken.TokenPosition + dieRoll >= 51)
                {
                    turnToken.TokenPosition += dieRoll - 51;
                }

                if (turnToken.Counter + dieRoll >= 56) //If the token is out of the game
                {
                    fields[fieldToRemove].RemoveToken();
                    turnToken.TokenState = TokenState.Finished; //The token is finished
                }
                else if (turnToken.Counter + dieRoll >= 51) //The token is in the safe field
                {
                    dieRest = dieCount;
                    turnToken.Counter += dieRoll;
                    if (turnToken.TokenState != TokenState.Safe)
                    {
                        fieldToMove = 51 + dieRest;
                        dieCount = 0;
                        turnToken.TokenState = TokenState.Safe;
                    }
                }
                else
                {
                    if (turnToken.TokenState == TokenState.Home && turnToken.CanMove) //The first move
                    {
                        dieCount = 0; //It shall not move away from the startposition
                        turnToken.TokenState = TokenState.InPlay; //The token are now in the 'InPlay' state
                        fieldToMove = turnToken.StartPosition;
                    }
                    else if (turnToken.CanMove)
                    {
                        turnToken.Counter += dieRoll;
                        fieldToMove = turnToken.TokenPosition + (dieRoll - 1); //This line is NOT USELESS
                        dieCount = 0;
                    }
                    else
                    {
                        Console.WriteLine("\nThats not a valid piece");
                        MoveToField(plr, dieRoll); // Calls itself but fail to give a new value
                    }
                }
            }
            MoveToken(turnToken, fieldToMove, fieldToRemove);
        }

        //--------------------- DIVIDER ---------------------

        //Moves the token
        private void MoveToken(Token token, int fieldToMove, int fieldToRemove)
        {          
            fields[fieldToRemove].RemoveToken();

            hasMoveSucceded = fields[fieldToMove].PlaceToken(token, token.GetColor());
            
        }

        #region Getters

        //Gets a list of all the players
        private void GetPlayers()
        {
            foreach(Player pl in players)
            {
                Console.WriteLine("#" + pl.GetName() + " - " + pl.GetColor() + " - " + pl.GetToken(1).StartPosition);

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine(pl.GetToken(i));
                }
            }
        }
        
        //DEBUG remove later/remove in release
        //Gets a list of all the fields USED FOR DEBUGGING
        private void GetField()
        {
            foreach(Field fi in this.fields)
            {
                Console.WriteLine(fi.GetFieldId() + " - " + fi.GetFieldColor());
            }
        }
        #endregion
    }
}
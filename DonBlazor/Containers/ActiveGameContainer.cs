using DevExpress.Data.Async.Helpers;
using DonBlazor.Models;
using SharedClassLibrary;
using SharedClassLibrary.Exceptions;
using System.ComponentModel.DataAnnotations;


namespace DonBlazor.Containers
{

    /// <summary>
    /// Singleton creater of a Game instance
    /// </summary>

    public sealed class ActiveGameContainer: Game
    {
        private static ActiveGameContainer? GameInstance = null;

        public static ActiveGameContainer GetGameInstance   
        {
            get
            {
                GameInstance ??= new ActiveGameContainer();  // ?? is null-coalescing operator. ??= returns lhs if its not null else assigns rhs
                return GameInstance;
            }
        }

        private ActiveGameContainer()
        {
     
        }

        public void DestroyGameInstance()
        {
            GameInstance = null;
        }

        // Properties      
        
        public int CurrentCharacter{ 
            get 
            {
                if (AllCharacters.Count == 0)
                {
                    throw new NoPLayersInGameException();  
                }
                
                return CurrentTurn % AllCharacters.Count;
            } 
        }

        // Methods
        public void UpdateToNewGame(Game newGame)
        {
            Name = newGame.Name;
            HumanPlayers = newGame.HumanPlayers;
            AllCharacters.Clear();
            AllCharacters.AddRange(newGame.HumanPlayers); // At start of game, there are only Humanplayers  
            CurrentTurn = 0;
        }

        public void NextTurn()
        {
            CurrentTurn++;
        }

       
        public void AddPlayerToGame(Player newPlayer)
        {
            HumanPlayers.Add(newPlayer);
        }
    }
}
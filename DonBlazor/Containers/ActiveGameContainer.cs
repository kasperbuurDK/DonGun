using SharedClassLibrary;


namespace DonBlazor.Containers
{

    /// <summary>
    /// Singleton creater of a Game instance
    /// </summary>

    public sealed class ActiveGameContainer : Game
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

        // Methods
       

        public void NextTurn()
        {
            CurrentTurn++;
        }


    }
}
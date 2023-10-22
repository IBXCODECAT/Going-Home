namespace Hexoidra
{
    class Program
    {
        private static void Main(String[] args)
        {
            using(Game game = new Game(920, 540))
            {
                game.Title = "Going Home";
                game.Run();
            }
        }
    }
}
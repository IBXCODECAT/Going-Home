
namespace Hexoidra
{
    class Program
    {
        private static void Main(String[] args)
        {
            using(Game game = new Game(500, 500))
            {
                game.Run();
            }
        }
    }
}
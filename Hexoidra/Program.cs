
namespace Hexoidra
{
    class Program
    {
        private static void Main(String[] args)
        {
            using(Game game = new Game(1920, 1080))
            {
                game.Run();
            }
        }
    }
}
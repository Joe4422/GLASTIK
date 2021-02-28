using System;

namespace GLASTIK
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Glastik())
                game.Run();
        }
    }
}

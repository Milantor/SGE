using System;
using OpenTK.Graphics.OpenGL4;

namespace ScientificGraphicsEngine;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("opening window");
        using var game = new Game(720, 480, "test");
        game.Run();
    }
}
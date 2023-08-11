using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography;

namespace cSharpRaylib
{
    static class Program
    {
        private static float boidSpeed = 10;
        private static int boidCount = 500;

        private static List<Boid> boids = new List<Boid>();
        private static Vector2 windowSize = new Vector2(800, 480);

        public static void Main(string[] args)
        {
            initBoids(boidCount);

            Raylib.InitWindow((int)windowSize.X, (int)windowSize.Y, "Hello World");

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);

                drawBoids();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        private static void initBoids(int boidCount)
        {
            var rnd = new Random();

            for (int i = 0; i < boidCount; i++)
            {
                boids.Add(new Boid(
                    windowSize, 
                    new Vector2(
                        (float)rnd.NextDouble()*windowSize.X,
                        (float)rnd.NextDouble() * windowSize.Y
                    ), 
                    new Vector2(
                        (float)(rnd.NextDouble()*2 - 1) * boidSpeed,
                        (float)(rnd.NextDouble()*2 - 1) * boidSpeed
                    )));
            }
        }

        private static void drawBoids()
        {
            foreach (var boid in boids)
            {
                boid.Update(boids);
                boid.Draw();
            }
        }
    }
}
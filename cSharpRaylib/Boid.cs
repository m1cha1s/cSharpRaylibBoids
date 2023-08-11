using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace cSharpRaylib
{
    internal class Boid
    {
        private Vector2 windowSize;

        private float maxSpeed = 75;
        private float minSpeed = 25;

        private float separationDistance = 30;
        private float separationForce = 15f;

        private float alignmentDistance = 30;
        private float alignmentForce = 13f;

        private float cohesionDistance = 20;
        private float cohesionForce = 12f;

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration = new Vector2(0, 0);

        private Stopwatch timer = new Stopwatch();

        public Boid(Vector2 windowSize, Vector2 position, Vector2 velocity)
        {
            this.windowSize = windowSize;

            this.position = position;
            this.velocity = velocity;
        }

        private void Separation(List<Boid> flockmates)
        {
            foreach (Boid bo in flockmates)
            {
                // Skip yourself
                if (bo == this) continue;

                // Skip far away flockmates
                var d = (separationDistance - Vector2.Distance(this.position, bo.position))/separationDistance;
                if (d < 0) continue;

                var norm = Vector2.Normalize(position - bo.position);

                acceleration += norm * separationForce;
            }
        }

        private void Alignment(List<Boid> flockmates)
        {
            foreach (Boid bo in flockmates)
            {
                // Skip yourself
                if (bo == this) continue;

                // Skip far away flockmates
                var d = (alignmentDistance - Vector2.Distance(this.position, bo.position)) / alignmentDistance;
                if (d < 0) continue;

                var normBo = Vector2.Normalize(bo.velocity);
                var norm = Vector2.Normalize(velocity);

                var af = normBo - norm;

                acceleration += af * d * alignmentForce; 
            }
        }

        private void Cohesion(List<Boid> flockmates)
        {
            Vector2 avrgPos = new Vector2(0);
            int count = 0;

            foreach (Boid bo in flockmates)
            {
                // Skip yourself
                if (bo == this) continue;

                // Skip far away flockmates
                var d = (cohesionDistance - Vector2.Distance(this.position, bo.position)) / cohesionDistance;
                if (d < 0) continue;

                avrgPos += bo.position;
                count ++;
            }

            if (count == 0) return;

            avrgPos /= count;

            acceleration += (avrgPos - position) * cohesionForce;
        }

        public void Update(List<Boid> flockmates)
        {
            float dt = (float)timer.Elapsed.TotalSeconds;

            Separation(flockmates);
            Alignment(flockmates);
            Cohesion(flockmates);

            velocity += acceleration * dt;

            velocity = Vector2.Normalize(velocity)*maxSpeed;

            position += velocity * dt;
            acceleration *= 0;

            if (position.X >= windowSize.X)
                position.X -= windowSize.X;
            if (position.Y >= windowSize.Y)
                position.Y -= windowSize.Y;
            if (position.X < 0)
                position.X += windowSize.X;
            if (position.Y < 0)
                position.Y += windowSize.Y;

            timer.Restart();
        }

        public void Draw()
        {
            var normal = new Vector2(0, 1);
            var angle = MathF.Atan2(velocity.Y, velocity.X) + MathF.PI/2;

            var back = new Vector2(0, 15);
            var backL = Vector2.Transform(back, Matrix3x2.CreateRotation(angle - 0.2f));
            var backR = Vector2.Transform(back, Matrix3x2.CreateRotation(angle + 0.2f));
            Raylib.DrawTriangle(position, backR+position, backL+position, Color.BLACK);
        }
    }
}

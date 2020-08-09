using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace VerletPhysicsTest1
{
    class VerletObject
    {
        public Texture2D StickTexture, PointTexture;
        float Bounce = 0.9f;
        float Gravity = 0.5f;
        float Friction = 0.999f;

        static Random Random = new Random();

        public class Stick
        {
            public Node Point1, Point2;
            public float Length;
        }


        public class Node
        {
            public Vector2 CurrentPosition, PreviousPosition, Velocity;
        }

        List<Node> Nodes = new List<Node>();
        List<Stick> Sticks = new List<Stick>();

        public VerletObject()
        {
            Nodes.Add(new Node() 
            { 
                CurrentPosition = new Vector2(0, 0),
                PreviousPosition = new Vector2(Random.Next(-15, 15), Random.Next(-15, 15)) 
            });

            Nodes.Add(new Node()
            {
                CurrentPosition = new Vector2(100, 0),
                PreviousPosition = new Vector2(Random.Next(-15, 5), Random.Next(-15, 15))
            });

            Nodes.Add(new Node()
            {
                CurrentPosition = new Vector2(0, 100),
                PreviousPosition = new Vector2(0, 100)
            });
            
            Sticks.Add(new Stick()
            {
                Point1 = Nodes[0],
                Point2 = Nodes[1],
                Length = 100
            });

            Sticks.Add(new Stick()
            {
                Point1 = Nodes[0],
                Point2 = Nodes[2],
                Length = 100
            });

        }

        public void LoadContent(ContentManager contentManager)
        {
            StickTexture = contentManager.Load<Texture2D>("RopeTexture");
            PointTexture = contentManager.Load<Texture2D>("Point");
        }

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && Vector2.Distance(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Nodes[0].CurrentPosition) < 32)
                Nodes[0].CurrentPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            foreach (Stick stick in Sticks)
            {
                Vector2 Direction = stick.Point2.CurrentPosition - stick.Point1.CurrentPosition;
                float Dist = Vector2.Distance(stick.Point1.CurrentPosition, stick.Point2.CurrentPosition);
                float Diff = stick.Length - Dist;
                float percent = Diff / Dist / 2;
                Vector2 Offset = Direction * percent;

                stick.Point1.CurrentPosition -= Offset;
                stick.Point2.CurrentPosition += Offset;
            }

            foreach (Node node in Nodes)
            {
                node.Velocity = (node.CurrentPosition - node.PreviousPosition) * Friction;
                node.PreviousPosition = node.CurrentPosition;
                node.CurrentPosition += node.Velocity;
                node.CurrentPosition.Y += Gravity;

                #region Handle bouncing
                if (node.CurrentPosition.X > 1272)
                {
                    node.CurrentPosition.X = 1272;
                    node.PreviousPosition.X = node.CurrentPosition.X + node.Velocity.X * Bounce;
                }

                if (node.CurrentPosition.X < 0)
                {
                    node.CurrentPosition.X = 0;
                    node.PreviousPosition.X = node.CurrentPosition.X + node.Velocity.X * Bounce;
                }

                if (node.CurrentPosition.Y > 712)
                {
                    node.CurrentPosition.Y = 712;
                    node.PreviousPosition.Y = node.CurrentPosition.Y + node.Velocity.Y * Bounce;
                }

                if (node.CurrentPosition.Y < 0)
                {
                    node.CurrentPosition.Y = 0;
                    node.PreviousPosition.Y = node.CurrentPosition.Y + node.Velocity.Y * Bounce;
                }
                #endregion

                if (node.CurrentPosition.Y == 712)
                {
                    Friction = 0.92f;
                }
                else
                {
                    Friction = 0.999f;
                }
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Node node in Nodes)
            {
                spriteBatch.Draw(PointTexture, new Rectangle((int)node.CurrentPosition.X, (int)node.CurrentPosition.Y, 8, 8), 
                                 null, Color.White, 0, new Vector2(4, 4), SpriteEffects.None, 0);
            }

            foreach (Stick stick in Sticks)
            {
                for (int l = 0; l < (int)Vector2.Distance(stick.Point1.CurrentPosition, stick.Point2.CurrentPosition); l++)
                {
                    Vector2 Direction = stick.Point2.CurrentPosition - stick.Point1.CurrentPosition;
                    Direction.Normalize();

                    spriteBatch.Draw(StickTexture, stick.Point1.CurrentPosition + l * Direction, Color.White);
                }
            }
        }
    }
}

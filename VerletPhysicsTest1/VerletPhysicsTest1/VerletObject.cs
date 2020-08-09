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
        float Bounce = 0.7f;
        float Gravity = 0.5f;
        float Friction = 0.999f;
        double Time;
        float offset;

        static Random Random = new Random();

        public class Stick
        {
            public Node Point1, Point2;
            public float Length;
        }

        public class Node
        {
            public Vector2 CurrentPosition, PreviousPosition, Velocity;
            public bool Pinned;
        }

        List<Node> Nodes = new List<Node>();
        List<Node> Nodes2 = new List<Node>();
        List<Stick> Sticks = new List<Stick>();

        public VerletObject()
        {
            //0 - Top Left
            Nodes.Add(new Node()
            {
                CurrentPosition = new Vector2(0, 0),
                PreviousPosition = new Vector2(Random.Next(-15, 15), Random.Next(-15, 15))
            });

            //1 - Top Right
            Nodes.Add(new Node()
            {
                CurrentPosition = new Vector2(100, 0),
                PreviousPosition = new Vector2(Random.Next(-15, 15), Random.Next(-15, 15))
            });

            //2 - Bottom Right
            Nodes.Add(new Node()
            {
                CurrentPosition = new Vector2(100, 100),
                PreviousPosition = new Vector2(Random.Next(-15, 5), Random.Next(-15, 15))
            });

            //3 - Bottom Left
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
                Point1 = Nodes[1],
                Point2 = Nodes[2],
                Length = 100
            });

            Sticks.Add(new Stick()
            {
                Point1 = Nodes[2],
                Point2 = Nodes[3],
                Length = 100
            });

            Sticks.Add(new Stick()
            {
                Point1 = Nodes[3],
                Point2 = Nodes[0],
                Length = 100
            });

            //Cross brace
            Sticks.Add(new Stick()
            {
                Point1 = Nodes[2],
                Point2 = Nodes[0],
                Length = 141f
            });
        }

        public void LoadContent(ContentManager contentManager)
        {
            StickTexture = contentManager.Load<Texture2D>("RopeTexture");
            PointTexture = contentManager.Load<Texture2D>("Point");
        }

        public void Update(GameTime gameTime)
        {
            Time += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                offset += 0.025f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                offset -= 0.025f;
            }

            if (Time > 16)
            {
                UpdateNodes();
                if (Nodes[0].Pinned == true)
                    Nodes[0].CurrentPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                //if (Nodes2[0].Pinned == true)
                    //Nodes2[0].CurrentPosition = new Vector2(Mouse.GetState().X + 40, Mouse.GetState().Y - offset);

                for (int i = 0; i < 30; i++)
                {
                    ConstrainNodes();
                    UpdateSticks();
                }

                Time = 0;
            }

            //Nodes[0].CurrentPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
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

        public void UpdateNodes()
        {
            foreach (Node node in Nodes)
            {
                if (node.Pinned == false)
                {
                    node.Velocity = (node.CurrentPosition - node.PreviousPosition) * Friction;
                    node.PreviousPosition = node.CurrentPosition;
                    node.CurrentPosition += node.Velocity;
                    node.CurrentPosition.Y += Gravity;
                }
            }

            foreach (Node node in Nodes2)
            {
                if (node.Pinned == false)
                {
                    node.Velocity = (node.CurrentPosition - node.PreviousPosition) * Friction;
                    node.PreviousPosition = node.CurrentPosition;
                    node.CurrentPosition += node.Velocity;
                    node.CurrentPosition.Y += Gravity;
                }
            }
        }

        public void ConstrainNodes()
        {
            foreach (Node node in Nodes)
            {
                if (node.Pinned == false)
                {
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
                }
            }

            foreach (Node node in Nodes2)
            {
                if (node.Pinned == false)
                {
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
                }
            }
        }

        public void UpdateSticks()
        {
            foreach (Stick stick in Sticks)
            {
                Vector2 Direction = stick.Point2.CurrentPosition - stick.Point1.CurrentPosition;
                float Dist = Vector2.Distance(stick.Point1.CurrentPosition, stick.Point2.CurrentPosition);
                float Diff = stick.Length - Dist;
                float percent = Diff / Dist / 2;
                Vector2 Offset = Direction * percent;

                if (stick.Point1.Pinned == false)
                stick.Point1.CurrentPosition -= Offset;

                if (stick.Point2.Pinned == false)
                stick.Point2.CurrentPosition += Offset;
            }
        }
    }
}

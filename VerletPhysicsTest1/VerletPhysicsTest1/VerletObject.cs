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

        public class Stick
        {
            public Node Point1, Point2;
            public float Length;
        }

        public class Node
        {
            public Vector2 CurrentPosition, PreviousPosition, Velocity;
            public bool Tether = false;
        }

        List<Node> Nodes = new List<Node>();
        List<Stick> Sticks = new List<Stick>();

        public VerletObject()
        {
            for (int i = 0; i < 61; i++)
            {
                Nodes.Add(new Node()
                {
                    CurrentPosition = new Vector2(0, 0),
                    PreviousPosition = new Vector2(0, 0)
                });
            }

            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                Sticks.Add(new Stick()
                {
                    Point1 = Nodes[i],
                    Point2 = Nodes[i+1],
                    Length = 10
                });
            }

        }

        public void LoadContent(ContentManager contentManager)
        {
            StickTexture = contentManager.Load<Texture2D>("RopeTexture");
            PointTexture = contentManager.Load<Texture2D>("Point");
        }

        public void Update(GameTime gameTime)
        {
            Nodes[0].CurrentPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Nodes[0].Tether = true;

            Nodes[60].CurrentPosition = new Vector2(1280 / 2, 720);
            Nodes[60].Tether = true;


            foreach (Node node in Nodes)
            {
                if (node.Tether == false)
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
                }
            }

            for (int i = 0; i < 30; i++)
            {
                foreach (Stick stick in Sticks)
                {
                    Vector2 Direction = stick.Point2.CurrentPosition - stick.Point1.CurrentPosition;
                    float Dist = Vector2.Distance(stick.Point1.CurrentPosition, stick.Point2.CurrentPosition);
                    float Diff = stick.Length - Dist;
                    float percent = Diff / Dist / 2;
                    Vector2 Offset = Direction * percent;

                    if (stick.Point1.Tether == false)
                        stick.Point1.CurrentPosition -= Offset;

                    if (stick.Point2.Tether == false)
                        stick.Point2.CurrentPosition += Offset;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //foreach (Node node in Nodes)
            //{
            //    spriteBatch.Draw(PointTexture, new Rectangle((int)node.CurrentPosition.X, (int)node.CurrentPosition.Y, 8, 8),
            //                     null, Color.White, 0, new Vector2(4, 4), SpriteEffects.None, 0);
            //}

            foreach (Stick stick in Sticks)
            {
                for (int l = 0; l < (int)Vector2.Distance(stick.Point1.CurrentPosition, stick.Point2.CurrentPosition); l+=10)
                {
                    Vector2 Direction = stick.Point2.CurrentPosition - stick.Point1.CurrentPosition;
                    Direction.Normalize();

                    spriteBatch.Draw(StickTexture, new Rectangle((int)(stick.Point1.CurrentPosition.X + l * Direction.X), (int)(stick.Point1.CurrentPosition.Y + l * Direction.Y), StickTexture.Width, StickTexture.Height), null, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}

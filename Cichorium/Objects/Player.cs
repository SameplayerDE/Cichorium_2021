using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cichorium.Managers;
using Cichorium.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;
using tainicom.Aether.Physics2D.Collision;

namespace Cichorium.Objects
{
    public class Player : Sprite
    {

        public float Speed { get; private set; } = 48f;
        public Point Offset { get; private set; }
        public bool IsMoving { get { return moving; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        int Frame = 0;
        TimeSpan LastUpdate = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan Delay = new TimeSpan(0, 0, 0, 2, 500);
        TimeSpan UpdateRate = new TimeSpan(0, 0, 0, 0, 45);
        public Direction Direction { get; set; } = Direction.DOWN;

        bool moving = false;
        bool prevMoving = false;
        Vector2 velocity = Vector2.Zero;

        public Body secondBody = new Body();

        public Player(float x, float y) : base("player", x, y)
        {
            
            BodyType = BodyType.Dynamic;
            CreateRectangle(10f, 10f, 100f, new Vector2(0, 8));
            FixedRotation = true;
            SetCollidesWith(Category.Cat1);
            SetCollisionCategories(Category.Cat1);

            secondBody.Position = Position;
            secondBody.BodyType = BodyType.Kinematic;
            secondBody.CreateRectangle(10f, 10f, 100f, new Vector2(0, 8));
            secondBody.FixedRotation = true;
            secondBody.SetCollidesWith(Category.Cat2);
            secondBody.SetCollisionCategories(Category.Cat2);

            WeldJoint weldJoint = new WeldJoint(this, secondBody, Position, secondBody.Position);

        }


        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Invisible)
            {
                if (DrawRect != null)
                    spriteBatch.Draw(Texture, Position, DrawRect, Color * Alpha, Rotation, DrawRect.Value.Size.ToVector2() / 2, Scale, SpriteEffects.None, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {

             Depth = 1f;

            Speed = SaveFileManager.Data.Speed;

            if (prevMoving == false && moving == true)
            {
                LastUpdate = gameTime.TotalGameTime;
            }
            
            //Blinking animation
            if (gameTime.TotalGameTime - LastUpdate > UpdateRate)
            {
                Frame++;
                LastUpdate = gameTime.TotalGameTime;
                if (!moving)
                {
                    if (Frame > 2)
                    {
                        Frame = 0;
                        LastUpdate += Delay;
                    }
                }
                else
                {
                    if (Frame < 3)
                    {
                        Frame = 3;
                    }
                    if (Frame > 10)
                    {
                        Frame = 3;
                    }
                }

            }

            prevMoving = moving;
            moving = false;

            velocity = Vector2.Zero;

            //Checking ever input action

            

            if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Down) || Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y < -0.05f)
            {
                velocity = new Vector2(0, Speed);
                //ApplyTorque(1f);
                Direction = Direction.DOWN;
                moving = true;
            }
            else if ((Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Right) || Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X > 0.05f) && !Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up) && !Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up))
            {
                velocity = new Vector2(Speed, 0);
                Direction = Direction.RIGHT;
                moving = true;
            }
            else if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up) || Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y > 0.05f)
            {
                velocity = new Vector2(0, -Speed);
                Direction = Direction.UP;
                moving = true;
            }
            else if ((Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Left) || Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X < -0.05f) && !Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up) && !Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up))
            {
                velocity = new Vector2(-Speed, 0);
                Direction = Direction.LEFT;
                moving = true;
            }
            

            if ((Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Left) && Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up)) || (Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X < -0.05f && Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y > 0.05f))
            {
                velocity = (new Vector2(-Speed / 1.5f, -Speed / 1.5f));
                Direction = Direction.UP;
                moving = true;
            }
            else if ((Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Left) && Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Down)) || (Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X < -0.05f && Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y < -0.05f))
            {
                velocity = new Vector2(-Speed / 1.5f, Speed / 1.5f);
                Direction = Direction.DOWN;
                moving = true;
            }
            else if ((Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Right) && Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Up)) || (Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X > 0.05f && Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y > 0.05f))
            {
                velocity = new Vector2(Speed / 1.5f, -Speed / 1.5f);
                Direction = Direction.UP;
                moving = true;
            }
            else if ((Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Right) && Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.Down)) || (Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X > 0.05f && Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y < -0.05f))
            {
                velocity = new Vector2(Speed / 1.5f, Speed / 1.5f);
                Direction = Direction.DOWN;
                moving = true;
            }

            LinearVelocity = velocity;
            secondBody.Position = Position;
            //secondBody.LinearVelocity = Vector2.Zero;
            //Setting Sprite

            int x = 32 * Frame;

            switch (Direction)
            {
                case Direction.DOWN:
                    DrawRect = new Rectangle(x, 0, 32, 32);
                    break;
                case Direction.UP:
                    DrawRect = new Rectangle(x, 64, 32, 32);
                    break;
                case Direction.LEFT:
                    DrawRect = new Rectangle(x, 96, 32, 32);
                    break;
                case Direction.RIGHT:
                    DrawRect = new Rectangle(x, 32, 32, 32);
                    break;
            }

            SaveFileManager.Data.Position = Position.ToPoint();

            base.Update(gameTime);
        }

    }



    public enum Direction
    {
        UP,
        LEFT,
        DOWN,
        RIGHT
    }
}

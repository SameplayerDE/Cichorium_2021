using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cichorium.Managers;
using Cichorium.Objects.ScreenObjects;

namespace Cichorium.Objects.Screens
{
    public class SkillTreeScreen : DragableScreen
    {

        private int scale = 3;
        private Label points;
        private Label displayname;
        private Label[] description = new Label[10];
        private Label costs;

        private Image Cursor;
        public static Dictionary<int, List<Line>> lines = new Dictionary<int, List<Line>>();

        private static List<Point> positions = new List<Point>()
        {
            new Point(0, 0),
            new Point(1, -1),
            new Point(-1, -1),
            new Point(-1, 1),
            new Point(1, 1),
            new Point(2, 1)
        };

        public SkillTreeScreen() : base(1)
        {
            DrawBackground = true;
            Position = Cichorium.Middle;
            Size = Cichorium.Scale;
            Alpha = 0.9f;
            Color = Color.Black;

            displayname = new Label("name");
            displayname.Color = Color.Yellow;
            displayname.Scale = 1f;
            displayname.Background = true;

            for (int i = 0; i < 10; i++)
            {
                description[i] = new Label("desc");
                description[i].Color = Color.Gray;
                description[i].Scale = 0.75f;
                description[i].Background = true;
            }

            costs = new Label("Kosten");
            costs.Color = Color.Crimson;
            costs.Scale = 1f;
            costs.Background = true;

            Cursor = new Image("cursor");

            int loop = 0;
            foreach (SkillData Skill in SkillManager.Skills)
            {
                Add(SkillManager.GetSkillWithPosition(positions[loop].X * 32, positions[loop].Y * 32, loop));
                loop++;
            }

            for (int i = 1; i < SkillManager.Skills.Count; i++)
            {
                lines.Add(i, new List<Line>());
                for (int j = 0; j < SkillManager.Skills[i].Parent.Length; j++)
                {
                    Line line = new Line(positions[SkillManager.Skills[i].Parent[j]].ToVector2(), positions[i].ToVector2(), 1, Color.White);
                    lines[i].Add(line);
                }
                
            }

            Mouse.SetPosition(Cichorium.Middle.X * Cichorium.Instance.Resolution, Cichorium.Middle.Y * Cichorium.Instance.Resolution);
            points = new Label("FP: " + SaveFileManager.Data.SkillPoints) { Color = Color.Yellow};

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Console.WriteLine(lines.Count);
            for (int i = 1; i < SkillManager.Skills.Count; i++)
            {
                for (int j = 0; j < SkillManager.Skills[i].Parent.Length; j++)
                {
                    if (SkillManager.Skills[SkillManager.Skills[i].Parent[j]].Unlocked)
                    {
                        lines[i][j].color = Color.Orange;
                    }
                    lines[i][j].p1 = positions[SkillManager.Skills[i].Parent[j]].ToVector2() * 32 + Position.ToVector2();
                    lines[i][j].p2 = positions[i].ToVector2() * 32 + Position.ToVector2();
                    lines[i][j].Update(gameTime);
                }
            }

            if (Cichorium.InputManager.UsingGamePad)
            {
                Mouse.SetPosition(Cichorium.Middle.X * Cichorium.Instance.Resolution, Cichorium.Middle.Y * Cichorium.Instance.Resolution);
                Cursor.Position = Cichorium.Middle;
            }

/**
            if (!Cichorium.InputManager.UsingGamePad)
            {
                Cichorium.Instance.IsMouseVisible = true;
                
            }

            if (Cichorium.InputManager.UsingGamePad)
            {
                Point Pos = Mouse.GetState().Position;
                Cursor.Position += (new Vector2(Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.X, -Cichorium.InputManager.CurrentGamepadState.ThumbSticks.Left.Y) * 4).ToPoint();
                Cichorium.Instance.IsMouseVisible = false;
                Mouse.SetPosition(Cursor.Position.X * Cichorium.Instance.Resolution, Cursor.Position.Y * Cichorium.Instance.Resolution);
            }
 **/

            foreach (var obj in Components)
            {
                if (obj is Skill)
                {
                    Skill s = obj as Skill;

                    displayname.Visible = false;
                    costs.Visible = false;

                    for (int i = 0; i < 10; i++)
                    {
                        description[i].Visible = false;
                    }

                    if (s.Hover)
                    {

                        displayname.SetText(s.Data.Displayname);

                        for (int i = 0; i < 10; i++)
                        {
                            if (s.Data.DescriptionLines > i)
                                description[i].SetText(s.Data.Description[i]);
                        }

                        costs.SetText("Kosten: " + s.Data.SkillPointCosts);

                        if (s.Data.SkillPointCosts <= SaveFileManager.Data.SkillPoints)
                        {
                            costs.Color = Color.LawnGreen;
                        }
                        else
                        {
                            costs.Color = Color.Crimson;
                        }

                        displayname.Update(gameTime);

                        for (int i = 0; i < 10; i++)
                        {
                            description[i].Update(gameTime);
                        }

                        costs.Update(gameTime);


                        for (int i = 0; i < s.Data.DescriptionLines; i++)
                        {
                            if (!s.Data.Locked) description[i].Visible = true;
                        }

                        if (!s.Data.Locked) displayname.Visible = true;
                        if (!s.Data.Locked && !s.Data.Unlocked) costs.Visible = true;

                        displayname.SetPosition((latestMouseState.Position.ToVector2() / Cichorium.Instance.Resolution).ToPoint() + new Point(5, 5));
                        costs.SetPosition(displayname.Position + new Point(0, displayname.Size.Y));

                        for (int i = 1; i <= 10; i++)
                        {
                            description[i - 1].SetPosition(new Point(0, Cichorium.Scale.Y) - (new Point(0, description[i - 1].Size.Y * i)));
                        }

                        if (Cichorium.InputManager.UsingGamePad)
                        {
                            displayname.SetPosition((Cursor.Position + new Point(5, 5)));
                            costs.SetPosition(displayname.Position + new Point(0, displayname.Size.Y));
                        }
                        return;
                    }
                   
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            

            base.Draw(spriteBatch);

            foreach (List<Line> line in lines.Values)
            {
                foreach (Line lineX in line)
                {
                    lineX.Draw(spriteBatch);
                }
            }

            DrawObjects(spriteBatch);

            if (Cichorium.InputManager.UsingGamePad)
            {
                Cursor.Draw(spriteBatch);
            }

            
            

        }

        public void UpdatePoints()
        {
            points.SetText("FP: " + SaveFileManager.Data.SkillPoints);
        }

        public void UpdateSkillTree()
        {
            RemoveAll();
            int loop = 0;
            foreach (SkillData Skill in SkillManager.Skills)
            {
                Add(SkillManager.GetSkillWithPosition(positions[loop].X * 32, positions[loop].Y * 32, loop));
                loop++;
            }
            
            Add(points);
            Add(displayname);
            Add(costs);

            for (int i = 0; i < 10; i++)
            {
                Add(description[i]);
            }

        }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cichorium.Managers;
using Cichorium.Objects.Screens;

namespace Cichorium.Objects.ScreenObjects
{
    public class Skill : Image
    {

        public SkillData Data;

        private TimeSpan last, now;
        private TimeSpan lastUpdate;
        
        private bool AnimationInit = false;
        double x = 0;

        public bool Hover = false;

        public Skill(ref SkillData Data) : base(Data.GetCalculatedSourcename())
        {
            this.Data = Data;
        }

        public Skill(int x, int y, ref SkillData Data) : base(x, y, Data.GetCalculatedSourcename())
        {
            this.Data = Data;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, ImageArea, Color * Alpha);
            //Color = Color.White;
            if (!Data.Locked && Data.Unlocked)
            {
                //Color = Data.Color;
                Color = Color.Orange;
            }
            if (Container != null)
            {
                spriteBatch.Draw(Cichorium.TextureManager.Sprites["skillbadge"], Container.Position.ToVector2() + Position.ToVector2(), null, Color * Alpha, 0, Cichorium.TextureManager.Sprites["skillbadge"].Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(Cichorium.TextureManager.Sprites["skillbadge"], Position.ToVector2(), null, Color * Alpha, 0, Cichorium.TextureManager.Sprites["skillbadge"].Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
            }
            
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            now = gameTime.TotalGameTime;
            SetSource(Data.GetCalculatedSourcename());

            if (Data.Animation)
            {
                if (!Data.Locked)
                {
                    if (Rotation != 0f)
                    {
                        AnimationInit = false;
                        Rotation = 0f;
                    }
                    if (!AnimationInit)
                    {
                        for (int i = 0; i < Data.Parent.Length; i++)
                        {
                            SkillTreeScreen.lines[Data.ID][i].thickness = 1;
                            SkillTreeScreen.lines[Data.ID][i].Update(gameTime);
                            SkillTreeScreen.lines[Data.ID][i].color = Color.White;
                        }
                        Scale = 1.5f;
                        AnimationInit = true;
                    }

                    if (now - lastUpdate >= new TimeSpan(0, 0, 0, 0, 500))
                    {
                        Data.Animation = false;
                    }
                    else
                    {
                        Scale -= 0.05f;
                        if (Scale < 1f) { Scale = 1f; Data.Animation = false; }
                        return;
                    }
                }
                else
                {
                    if (!AnimationInit)
                    {
                        Rotation = 1.5f;
                        AnimationInit = true;
                    }

                    if (now - lastUpdate >= new TimeSpan(0, 0, 0, 0, 500))
                    {
                        if (Rotation >= -0.5f && Rotation <= 0.5f)
                        {
                            Data.Animation = false;
                            AnimationInit = false;
                            Rotation = 0f;
                            x = 0;
                            for (int i = 0; i < Data.Parent.Length; i++)
                            {
                                SkillTreeScreen.lines[Data.ID][i].thickness = 1;
                                SkillTreeScreen.lines[Data.ID][i].Update(gameTime);
                                SkillTreeScreen.lines[Data.ID][i].color = Color.White;
                            }
                        }
                        else
                        {
                            x += 0.25;
                            Rotation = (float)Math.Sin(x);
                        }
                    }
                    else
                    {

                        for (int i = 0; i < Data.Parent.Length; i++)
                        {
                            if (SkillManager.Skills[Data.Parent[i]].Locked || !SkillManager.Skills[Data.Parent[i]].Unlocked)
                            {
                                //SkillTreeScreen.lines[Data.ID][i].thickness = Math.Max(1, (int)(Math.Sin(x) * 4));
                                if (Data.UnlockCondition == SkillUnlockCondition.MULTIPLE)
                                {
                                    SkillTreeScreen.lines[Data.ID][i].color = Color.Red;
                                }
                                else
                                {
                                    SkillTreeScreen.lines[Data.ID][i].color = Color.Yellow;
                                }
                                SkillTreeScreen.lines[Data.ID][i].Update(gameTime);
                            }
                        }
                        x += 0.25;
                        Rotation = (float)Math.Sin(x);

                        return;
                    }
                }
            }
            else
            {
                lastUpdate = now;
            }

            
            if (!Cichorium.InputManager.UsingGamePad)
            {
                
            }
            else
            {
                
            }

            if (ImageArea.Contains(Container.latestMouseState.Position))
            {
                Hover = true;
                if (Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.LeftClicked) || Cichorium.InputManager.IsActionTriggered((int)Cichorium.Actions.MenuYes))
                {
                    if (!Data.Locked && !Data.Unlocked)
                    {
                        //Scale = 1.25f;
                        if (Data.SkillPointCosts <= SaveFileManager.Data.SkillPoints)
                        {
                            Data.Buy();
                            SkillManager.Update();
                        }
                        else
                        {
                            Data.Denied(false);
                        }
                    }
                    else if (Data.Locked)
                    {
                        Data.Denied(true);
                    }
                }
            }
            else
            {
                Hover = false;
                if (!Data.Locked) Scale = 1;
            }

            last = now;

        }

    }

    public enum SkillUnlockCondition
    {
        MULTIPLE,
        SINGLE
    }

    public enum SkillAction
    {
        INCREASE_HEALTH_SCALE,
        INCREASE_SPEED,
        ALLOW_SKILLING,
        ALLOW_INVENTORY,
        NONE
    }

    public class SkillData
    {

        public int ID;
        public string Displayname;
        public string Sourcename;
        public string[] Description;
        public int DescriptionLines;
        public bool Locked;
        public bool Unlocked;
        public SkillAction Action;
        public SkillUnlockCondition UnlockCondition;
        public Color Color;

        public bool Animation = false;

        public int[] Parent;

        public int SkillPointCosts;

        public SkillData(int ID, string Displayname, string Sourcename, string[] Description, SkillAction? Action, SkillUnlockCondition? UnlockCondition, bool Locked, bool Unlocked, Color Color, int SkillPointCosts, params int[] Parent)
        {
            this.ID = ID;
            this.Displayname = Displayname;
            this.Description = Description;

            this.Action = (Action.HasValue) ? Action.Value : SkillAction.NONE;
            this.UnlockCondition = (UnlockCondition.HasValue) ? UnlockCondition.Value : SkillUnlockCondition.SINGLE;

            this.Sourcename = Sourcename;
            this.Locked = Locked;
            this.Unlocked = Unlocked;
            this.Color = Color;
            this.SkillPointCosts = SkillPointCosts;
            this.Parent = Parent;
            this.DescriptionLines = Description.Length;
        }

        public string GetCalculatedSourcename()
        {
            if (Locked) return "skillicon_locked";
            if (!Unlocked) return "skillicon_" + Sourcename;
            return "skillicon_" + Sourcename;
        }

        public void Update()
        {
            if (UnlockCondition == SkillUnlockCondition.SINGLE) {
                foreach (int x in Parent) {
                    if (SkillManager.Skills[x].Unlocked)
                    {
                        Unlock();
                    }
                }
            }
            else if (UnlockCondition == SkillUnlockCondition.MULTIPLE)
            {
                bool shouldBeUnlocked = true;
                foreach (int x in Parent)
                {
                    if (!SkillManager.Skills[x].Unlocked)
                    {
                        shouldBeUnlocked = false;
                        return;
                    }
                }
                if (shouldBeUnlocked)
                {
                    Unlock();
                }
            }
        }

        public void Denied(bool withAnimation)
        {
            Cichorium.AudioManager.SoundEffects["skill_denied"].Play();
            if (withAnimation && !Animation) Animation = true;
        }

        public void Unlock()
        {
            if (Locked)
            {
                Locked = false;
                Animation = true;
            }
        }

        public void Buy()
        {
            Unlocked = true;
            SaveFileManager.Data.SkillPoints -= SkillPointCosts;

            if (Action == SkillAction.INCREASE_HEALTH_SCALE)
                SaveFileManager.Data.HealthScale += 1;

            if (Action == SkillAction.INCREASE_SPEED)
                SaveFileManager.Data.Speed += 16;

            Cichorium.Skills.UpdatePoints();
            Cichorium.AudioManager.SoundEffects["skill_unlocked"].Play();
        }

    }
}

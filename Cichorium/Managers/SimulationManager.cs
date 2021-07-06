using Cichorium.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using Cichorium.Objects.Screens;

namespace Cichorium.Managers
{
    public class SimulationManager
    {

        public bool Enabled = true;

        public Region CurrentRegion;
        public Region LastRegion;
        public Portal Portal = null;

        public World world { get; set; }
        public static bool init = false;

        public void Update(GameTime gameTime)
        {

            if (!Enabled)
            {
                return;
            }

            List<Body> TempBodies = CurrentRegion.World.BodyList;

            if (LastRegion != CurrentRegion)
            {
                LastRegion = CurrentRegion;
                Console.WriteLine("Updated World!");
                world = CurrentRegion.World;
            }

            CurrentRegion.World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            for (int i = 0; i < TempBodies.Count; i++)
            {

                Body body = TempBodies[i];

                if (body is Sprite)
                {

                    Sprite sprite = body as Sprite;
                    sprite.Update(gameTime);

                    if (sprite is Player)
                    {
                        Player player = sprite as Player;

                        for (int j = 0; j < TempBodies.Count; j++)
                        {

                            if (TempBodies[j] is NPC)
                            {

                                NPC npc = TempBodies[j] as NPC;

                                if (Vector2.Distance(npc.Position, player.Position) < 20f)
                                {

                                    if (npc.Position.Y < player.Position.Y)
                                    {
                                        player.Depth = 10;
                                    }
                                    else
                                    {
                                        player.Depth = 4;
                                    }

                                }
                            }

                            Body portalBody = TempBodies[j];

                            if (portalBody is Portal)
                            {

                                Portal portal = portalBody as Portal;

                                if (portal.BoundingBox.Contains(player.Position + player.Offset.ToVector2()))
                                {

                                    if (Portal != portal || Portal == null)
                                    {
                                        if (CurrentRegion.Title != portal.Destination)
                                        {
                                            Portal = portal;
                                        }
                                    }

                                }

                            }

                            if (TempBodies[j] is ParticleEmitter)
                            {
                                ParticleEmitter emitter = TempBodies[j] as ParticleEmitter;

                                if (emitter.particle is SmokeParticle)
                                {

                                    if (Vector2.Distance(emitter.Position, player.Position) < 20f)
                                    {

                                        if (emitter.Position.Y < player.Position.Y)
                                        {
                                            player.Depth = 10;
                                        }
                                        else
                                        {
                                            player.Depth = 4;
                                        }

                                    }

                                }

                            }

                        }
                    }

                }

            }
        }

    }

}


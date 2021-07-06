using Cichorium.Objects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;

namespace Cichorium.Managers
{
    public static class RegionLoader
    {

        public static Region[] LoadAll()
        {
            // Alle json-Files im Map-Folder suchen
            string mapPath = ".\\Regions";
            var files = Directory.GetFiles(mapPath, "*.json");

            // Alle gefundenen json-Files laden
            Region[] result = new Region[files.Length];
            for (int i = 0; i < files.Length; i++)
                result[i] = LoadFromJson(files[i]);

            return result;
        }

        public static Region LoadFromJson(string file)
        {
            FileInfo info = new FileInfo(file);
            using (Stream stream = File.OpenRead(file))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string json = sr.ReadToEnd();

                    FileRegion result = JsonConvert.DeserializeObject<FileRegion>(json);
                    FileLayer[] tileLayer = result.Layers.Where(l => l.type == "tilelayer").ToArray();
                    FileLayer objectLayer = result.Layers.Where(l => l.type == "objectgroup").FirstOrDefault();

                    Region region = new Region(info.Name.Substring(0, info.Name.Length - 5), result.Width, result.Height, result.TileWidth);
                    foreach (FileLayer fileLayer in tileLayer)
                    {
                        Layer layer = new Layer();
                        layer.Data = new int[result.Width, result.Height];
                        if (result.Width < result.Height)
                        {
                            for (int x = 0; x < result.Width; x++)
                            {
                                for (int y = 0; y < result.Height; y++)
                                {
                                    layer.Data[x, y] = fileLayer.data[(x * result.Height + y)];
                                    layer.IsVisible = fileLayer.visible;
                                }
                            }
                        }
                        else if (result.Width > result.Height)
                        {
                            for (int x = 0; x < result.Width; x++)
                            {
                                for (int y = 0; y < result.Height; y++)
                                {
                                    layer.Data[x, y] = fileLayer.data[(y * result.Width + x)];
                                    layer.IsVisible = fileLayer.visible;
                                }
                            }
                        }
                        else if (result.Width == result.Height)
                        {
                            for (int x = 0; x < result.Width; x++)
                            {
                                for (int y = 0; y < result.Height; y++)
                                {
                                    layer.Data[x, y] = fileLayer.data[(x + result.Height * y)];
                                    layer.IsVisible = fileLayer.visible;
                                }
                            }
                        }

                        region.AddLayer(layer);
                    }

                    // Object Layer analysieren
                    if (objectLayer != null)
                    {
                        foreach (var portal in objectLayer.objects.Where(o => o.type == "Portal"))
                        {
                            Rectangle rectangle = new Rectangle(portal.x, portal.y, portal.width, portal.height);
                            region.AddBody(new Portal(portal.name, rectangle));
                        }
                        foreach (var entity in objectLayer.objects.Where(o => o.type == "Entity"))
                        {
                            if (entity.name == "Frog")
                            {
                                Frog b = new Frog(entity.x, entity.y, entity.width, entity.height);

                                FrictionJoint fj = new FrictionJoint(b, new Body(), b.WorldCenter);
                                fj.MaxForce = 20000f;

                                region.World.Add(fj);
                                region.AddBody(b);
                            }
                            else if (entity.name == "NPC")
                            {
                                NPC b = new NPC("npc", entity.x, entity.y, entity.width, entity.height);

                                FrictionJoint fj = new FrictionJoint(b, new Body(), b.WorldCenter);
                                fj.MaxForce = 9000.81f * b.Mass;

                                region.World.Add(fj);
                                region.AddBody(b);
                            }
                        }
                        foreach (var emitter in objectLayer.objects.Where(o => o.type == "Emitter"))
                        {
                            Console.WriteLine(emitter.ToString());

                            ParticleEmitter particleEmitter = new ParticleEmitter(emitter.x, emitter.y, emitter.width, emitter.height, BodyType.Kinematic);
                            particleEmitter.Area = new Rectangle(new Vector2(emitter.x, emitter.y).ToPoint(), new Vector2(emitter.width, emitter.height).ToPoint());


                            if (emitter.properties != null)
                            {
                                foreach (var prop in emitter.properties)
                                {
                                    //Console.WriteLine(prop.ToString());

                                    if (prop.name == "Amount")
                                    {
                                        if (prop.type == "int")
                                        {

                                            int value;

                                            if (int.TryParse(prop.value, out value))
                                            {
                                                particleEmitter.Max = value;
                                                //Console.WriteLine("Set Emitter-max to " + value);
                                            }
                                        }
                                    }
                                }
                            }
                            //ParticleEmitter particleEmitter = new ParticleEmitter(emitter.x + (emitter.width / 2), emitter.y + (emitter.height / 2));
                            
                            
                            

                            if (emitter.name == "Smoke")
                            {
                                SmokeParticle particle = new SmokeParticle(particleEmitter.Position.X, particleEmitter.Position.Y);
                                particleEmitter.SetParticle(particle);
                            }

                            if (emitter.name == "Fly")
                            {
                                FlyParticle particle = new FlyParticle(particleEmitter.Position.X, particleEmitter.Position.Y);
                                particleEmitter.SetParticle(particle);
                                particleEmitter.Friction = true;
                            }

                            region.AddEmitter(particleEmitter);
                        }
                        foreach (var player in objectLayer.objects.Where(o => o.type == "Player"))
                        {
                            Vector2 position = new Vector2(player.x, player.y);
                            region.AddStartposition(player.name, position);
                        }
                        foreach (var collision in objectLayer.objects.Where(o => o.type == "Collision"))
                        {
                            region.AddBody(new CollisionObject(collision.x, collision.y, collision.width, collision.height, BodyType.Static));
                        }
                        foreach (var collision in objectLayer.objects.Where(o => o.type == "Collision"))
                        {
                            region.AddBody(new CollisionObject(collision.x, collision.y, collision.width, collision.height, BodyType.Static));
                        }
                    }
                    return region;
                }
            }
        }

    }

    public class FileRegion
    {

        public string Name { get; set; }
        public int TileWidth { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public FileLayer[] Layers { get; set; }

    }

    public class FileLayer
    {

        public int[] data { get; set; }
        public FileObject[] objects { get; set; }
        public bool visible { get; set; }
        public string type { get; set; }

    }

    public class FileObject
    {
        /// <summary>
        /// Name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Object-Type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// X-Position
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// Y-Position
        /// </summary>
        public int y { get; set; }

        /// <summary>
        /// Breite
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// Höhe
        /// </summary>
        public int height { get; set; }

        public FileObjectProperty[] properties;

        public override string ToString()
        {
            return "X " + x + ", Y " + y + ", W " + width + ", H " + height;
        }
    }

    public class FileObjectProperty
    {
        public string name;
        public string type;
        public string value;

        public override string ToString()
        {
            return "Name " + name + ", Type " + type + ", Value " + value;
        }

    }

}

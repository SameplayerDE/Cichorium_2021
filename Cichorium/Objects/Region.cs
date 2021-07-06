using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace Cichorium.Objects
{
    public class Region
    {

        public World World { get; }
        public string Title { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int TileSize { get; set; }

        public List<Layer> Layers { get; private set; } = new List<Layer>();
        public Dictionary<string, Vector2> Startposition { get; private set; } = new Dictionary<string, Vector2>();
        public List<Body> Bodies { get; private set; } = new List<Body>();

        public Region()
        {
            World = new World(new Vector2(0, 0));
        }

        public Region(string title, int width, int height, int tileSize)
        {
            World = new World(new Vector2(0, 0));
            Title = title;
            Width = width;
            Height = height;
            TileSize = tileSize;
        }
 
        public void AddLayer(Layer layer)
        {
            Layers.Add(layer);
        }

        public void AddEmitter(ParticleEmitter particleEmitter)
        {
            AddBody(particleEmitter);
            
        }

        public void AddBody(Body body)
        {
            World.Add(body);
        }

        public void AddStartposition(string source, Vector2 position)
        {
            Startposition.Add(source, position);
        }

    }

    public class Layer
    {
        public bool IsVisible { get; set; }
        public int[,] Data { get; set; }

    }
}

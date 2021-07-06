using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Cichorium.Objects;

namespace Cichorium.Data
{
    [Serializable]
    public class PlayerData
    {
        [XmlAttribute]
        public int Health;
        public int HealthScale;
        public int SkillPoints;
        public float Speed;
        public Point Position;
        public string Region;
        public SkillTreeData SkillTree;

    }
}

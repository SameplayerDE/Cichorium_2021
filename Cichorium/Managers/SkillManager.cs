using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cichorium.Objects.ScreenObjects;

namespace Cichorium.Managers
{
    public static class SkillManager
    {
        /**public static List<SkillData> Skills = new List<SkillData>()
        {
            {new SkillData("Elefantenhaut", "DefenseSkill", false, false, 2, 0)},
            {new SkillData("Elefantenherz", "HealthSkill", true, false, 4, 0)},
        };**/

        public static List<SkillData> Skills = SaveFileManager.TreeData.Skills;

        public static Skill GetSkillWithPosition(int x, int y, int index)
        {
           SkillData data = Skills[index];
           return new Skill(x, y, ref data);
        }

        public static void Update()
        {
            foreach(SkillData Skill in Skills)
            {
                Skill.Update();
            }
        }

    }
}

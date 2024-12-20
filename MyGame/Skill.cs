using System;

public class Skill
{
    public int SkillActiveLevel { get; set; }
    public string SkillName { get; set; }
    public int SkillMp { get; set; }
    public bool IsActive { get; set; }

    public Skill(int skillActiveLevel, string skillName, int skillMp, bool isActive)
    {
        SkillActiveLevel = skillActiveLevel;
        SkillName = skillName;
        SkillMp = skillMp;
        IsActive = isActive;
    }
}


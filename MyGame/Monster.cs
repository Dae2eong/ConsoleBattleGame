using System;

public class Monster : Character
{
    public int damage;
    public float maxDamage;
    public float minDamage;
    public Monster(string name, int hp, int mp, int str, int def, int exp, bool isStun, int minGold, int maxGold) 
        : base(name, hp, mp, str, def, exp, isStun, minGold, maxGold)
    {
    }

    public override void ShowInfo()
    {
        Console.WriteLine("=====================");
        Console.WriteLine();
        Console.WriteLine("   ▶ 몬스터 정보 ◀");
        Console.WriteLine($"    ▶ {Name} ◀");
        Console.WriteLine($" ▶ HP : {CurHp} / {Hp} ◀");
        Console.WriteLine($" ▶ MP : {CurMp} / {Mp} ◀");
        Console.WriteLine($"    ▶ STR : {Str} ◀");
        Console.WriteLine($"    ▶ DEF : {Def} ◀");
        Console.WriteLine();
        Console.WriteLine("=====================");
    }

    public override void Attack(Character target)
    {
        Random random = new Random();

        maxDamage = Str * 1.2f;
        minDamage = Str * 0.8f;

        int ranDamage = random.Next((int)minDamage, (int)maxDamage);

        damage = ranDamage;
        target.GetDamage(damage);
    }
}

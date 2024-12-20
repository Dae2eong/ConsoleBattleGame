using System;

public class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int CurHp { get; set; }
    public int Mp { get; set; }
    public int CurMp { get; set; }
    public int Str { get; set; }
    public int Def { get; set; }
    public int Exp { get; set; }
    public bool IsStun {  get; set; }
    public int MinGold { get; set; }
    public int MaxGold { get; set; }

    public bool isAlive = true;

    public Character(string name, int hp, int mp, int str, int def, int exp, bool isStun, int minGold, int maxGold)
    {
        Name = name;
        Hp = hp;
        Mp = mp;
        Str = str;
        Def = def;
        CurHp = Hp;
        CurMp = Mp;
        Exp = exp;
        IsStun = isStun;
        MinGold = minGold;
        MaxGold = maxGold;
    }

    public virtual void ShowInfo() { }
    public virtual void Attack(Character target) { }

    public virtual void GetDamage(int damage) 
    {
        Random random = new Random();
        int damRan = random.Next(0, 2);
        if (Def >= damage)
        {
            if (damRan == 0)
            {
                CurHp -= 1;
            }
            else
            {
                Console.WriteLine("             Miss!");
                CurHp -= 0;
            }   
        }
        else
        {
            CurHp = CurHp - (damage - Def);
            Console.WriteLine($"{damage - Def}만큼 피해를 주었다.");
        }

        if (Hp <= 0)
        {
            Hp = 0;
        }
    }

    public void Die()
    {
        isAlive = false;
    }
}




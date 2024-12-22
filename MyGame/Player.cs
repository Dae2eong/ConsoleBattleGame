using System;

public class Player : Character
{
    public int level = 5;
    public int curExp;
    public int maxExp = 2;
    public int damage;
    public float maxDamage;
    public float minDamage;
    public int gold = 1000;

    private bool rightHand = false;
    private bool leftHand = false;
    private bool bothHands = false;
    
    public Player(string name, int hp, int mp, int str, int def, int exp, bool isStun, int minGold, int maxGold) 
        : base(name, hp, mp, str, def, exp, isStun, minGold, maxGold)
    {
    }

    public override void ShowInfo()
    {
        Console.WriteLine("=====================");
        Console.WriteLine();
        Console.WriteLine("   ▶ 캐릭터 정보 ◀");
        Console.WriteLine($"    ▶ {Name} ◀");
        Console.WriteLine($" ▶ HP : {CurHp} / {Hp} ◀");
        Console.WriteLine($" ▶ MP : {CurMp} / {Mp} ◀");
        Console.WriteLine($"    ▶ STR : {Str} ◀");
        Console.WriteLine($"    ▶ DEF : {Def} ◀");
        Console.WriteLine();
        Console.WriteLine($"    ▶ Level : {level} ◀");
        Console.WriteLine($"   ▶ EXP : {curExp} / {maxExp} ◀");
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

    public void Skills(Character target, string selectSkill)
    {
        Random random = new Random();
        int percent = random.Next(1,101);

        switch(selectSkill)
        {
            case "암살":
                if (percent <= 5)
                {
                    CurMp -= 3;
                    Console.WriteLine("5% 확률로 암살에 성공하였습니다.");
                    Thread.Sleep(1000);
                    target.CurHp = 0;
                    target.isAlive = false;
                }
                else
                {
                    Console.WriteLine("아무런 일이 일어나지 않았다.");
                }
                break;
            case "파워어택":
                CurMp -= 5;
                target.GetDamage(damage * 2);
                break;
            case "메테오":
                CurMp -= 10;
                target.GetDamage((int)(damage * 1.5f));
                target.IsStun = true;
                break;
            case "용의강림":
                CurMp -= 20;
                target.GetDamage((damage * 2) + target.Def);
                break;
            case "천벌":
                CurMp -= 30;
                if (percent <= 50)
                {
                    target.GetDamage(damage * 3);
                }
                else
                {
                    target.GetDamage(damage * 2);
                }
                break;
            case "자폭":
                CurMp -= 50;
                CurHp /= 2;
                target.GetDamage((int)(target.CurHp * 0.7f));
                break;
        }
    }

    public void LevelUp()
    {
        if (curExp >= maxExp)
        {
            curExp = curExp % maxExp;
            level++;
            maxExp += 10;
        }

        Hp += 10;
        Mp += 5;
        Str += 3;
        Def += 2;

        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
        Thread.Sleep(200);
        Console.WriteLine("{0, -19}■", "■");
        Thread.Sleep(200);
        Console.WriteLine("■    ★☆레벨업☆★    ■");
        Thread.Sleep(200);
        Console.WriteLine("{0, -19}■", "■");
        Thread.Sleep(200);
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
        Thread.Sleep(2000);

        Console.WriteLine($"HP {Hp}으로 증가");
        Console.WriteLine($"MP {Mp}으로 증가");
        Console.WriteLine($"STR {Str}으로 증가");
        Console.WriteLine($"DEF {Def}으로 증가");
        Thread.Sleep(2000);
    }

    public void Inventory(List<Weapon> inven)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=====================");
            Console.WriteLine();
            Console.WriteLine($"    보유골드 : {gold}");
            Console.WriteLine();
            
            for (int i = 0; i < inven.Count; i++)
            {
                if (inven[i].WpEquip)
                {
                    Console.WriteLine($"{inven[i].Code} {inven[i].WpName,8} [E]");
                }
                else
                {
                    Console.WriteLine($"{inven[i].Code} {inven[i].WpName,8}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("=====================");
            Console.WriteLine("=====================");
            Console.WriteLine();

            bool rightEquip = false;
            bool leftEquip = false;

            foreach (var item in inven)
            {
                if (item.WpEquip)
                {
                    if (item.WpType == "TwoHand")
                    {
                        Console.WriteLine($"  오른손 : {item.WpName} 장착중");
                        Console.WriteLine($"   왼손  : {item.WpName} 장착중");
                        rightEquip = true;
                        leftEquip = true;
                        break;
                    }
                    else if (item.WpType == "OneHand")
                    {
                        if (!rightEquip)
                        {
                            Console.WriteLine($"  오른손 : {item.WpName} 장착중");
                            rightEquip = true;
                        }

                        else if (!leftEquip)
                        {
                            Console.WriteLine($"   왼손  : {item.WpName} 장착중");
                            leftEquip = true;
                        }
                    }
                }
            }

            if (!rightEquip)
            {
                Console.WriteLine($"  오른손 : 빈슬롯");
            }

            if (!leftEquip)
            {
                Console.WriteLine($"   왼손  : 빈슬롯");
            }

            Console.WriteLine();
            Console.WriteLine("=====================");
            Console.WriteLine();
            Console.WriteLine("1.돌아가기 2.장착하기 3.해제하기");
            Console.WriteLine();
            Console.Write("▶ ");

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (result == 1)
                {
                    Console.Clear();
                    break;
                }
                else if (result == 2)
                {
                    EquipItem(inven);
                }
                else if (result == 3)
                {
                    UnequipItem(inven);
                }
            }
        }
    }

    private void EquipItem(List<Weapon> inven)
    {
        Console.WriteLine("어떤 장비를 장착하시겠습니까?");
        Console.Write("▶ ");
        if (int.TryParse(Console.ReadLine(), out int number))
        {
            foreach (var item in inven)
            {
                if (item.Code == number)
                {
                    if (item.WpEquip)
                    {
                        Console.WriteLine($"{item.WpName}은(는) 이미 장착중입니다.");
                        Thread.Sleep(2000);
                        return;
                    }

                    if (item.WpType == "TwoHand")
                    {
                        if (rightHand || leftHand)
                        {
                            Console.WriteLine("더이상 장착할 수 없습니다.");
                            Thread.Sleep(2000);
                            return;
                        }
                        bothHands = true;
                        rightHand = leftHand = item.WpEquip = true;
                        Console.WriteLine($"{item.WpName}을(를) 장착하였습니다.");

                        {
                            Str += item.WpStr;

                        }

                        Thread.Sleep(2000);
                    }
                    else if (item.WpType == "OneHand")
                    {
                        if (!rightHand)
                        {
                            rightHand = item.WpEquip = true;
                            Console.WriteLine($"{item.WpName}을(를) 장착하였습니다.");
                            Thread.Sleep(2000);

                            {
                                Str += item.WpStr;
                                Def += item.WpDef;
                            }
                        }
                        else if (!leftHand)
                        {
                            leftHand = item.WpEquip = true;
                            Console.WriteLine($"{item.WpName}을(를) 장착하였습니다.");
                            Thread.Sleep(2000);

                            {
                                Str += item.WpStr;
                                Def += item.WpDef;
                            }
                        }
                        else
                        {
                            Console.WriteLine("더이상 장착할 수 없습니다.");
                            Thread.Sleep(2000);
                        }
                    }
                }
            }
        }
    }

    private void UnequipItem(List<Weapon> inven)
    {
        Console.WriteLine("어떤 장비를 해제하시겠습니까?");
        Console.Write("▶ ");
        if (int.TryParse(Console.ReadLine(), out int number))
        {
            foreach (var item in inven)
            {
                if (item.Code == number && item.WpEquip)
                {
                    if (item.WpType == "TwoHand")
                    {
                        bothHands = rightHand = leftHand = item.WpEquip = false;
                        Console.WriteLine($"{item.WpName}을(를) 해제하였습니다.");
                        Thread.Sleep(2000);

                        {
                            Str -= item.WpStr;
                            Def -= item.WpDef;
                        }

                        return;
                    }
                    else if (item.WpType == "OneHand")
                    {
                        if (rightHand && item.WpEquip)
                        {
                            rightHand = item.WpEquip = false;
                            Console.WriteLine($"{item.WpName}을(를) 해제하였습니다.");
                            Thread.Sleep(2000);

                            {
                                Str -= item.WpStr;
                                Def -= item.WpDef;
                            }

                            return;
                        }
                        else if (leftHand && item.WpEquip)
                        {
                            leftHand = item.WpEquip = false;
                            Console.WriteLine($"{item.WpName}을(를) 해제하였습니다.");
                            Thread.Sleep(2000);

                            {
                                Str -= item.WpStr;
                                Def -= item.WpDef;
                            }

                            return;
                        }
                    }
                }
            }
        }
    }
}

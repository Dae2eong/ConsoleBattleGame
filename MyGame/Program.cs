using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace MyGame
{
    internal class Program
    {
        static string playerName = "대성대성";
        static string selectSkill;
        static bool cancelSkill = false;

        static void Main(string[] args)
        {
            //Start();
            //Create();
            Player player = new Player(playerName, 100, 50, 20, 10, 0, false, 0, 0);

            List<Character> monsterList = new List<Character>();
            StreamReader monsterTable = new StreamReader("MonsterTable.json");
            string monsterInfo = monsterTable.ReadToEnd();
            monsterList = JsonSerializer.Deserialize<List<Character>>(monsterInfo);
            List<Monster> monsters = new List<Monster>();

            List<Skill> skillList = new List<Skill>();
            StreamReader skillTable = new StreamReader("SkillTable.json");
            string skillInfo = skillTable.ReadToEnd();
            skillList = JsonSerializer.Deserialize<List<Skill>>(skillInfo);
            List<Skill> skills = new List<Skill>();

            List<Weapon> weaponList = new List<Weapon>();
            StreamReader weaponTable = new StreamReader("EquipmentTable.json");
            string weaponInfo = weaponTable.ReadToEnd();
            weaponList = JsonSerializer.Deserialize<List<Weapon>>(weaponInfo);
            List<Weapon> weapons = new List<Weapon>();

            List<Weapon> inven = new List<Weapon>();
            inven.Add(new Weapon(weaponList[0].Code, weaponList[0].WpName, weaponList[0].WpStr, weaponList[0].WpDef,
                    weaponList[0].WpMaxDur, weaponList[0].WpMinDur, weaponList[0].WpLevel, weaponList[0].WpGold,
                    weaponList[0].WpType, weaponList[0].WpEnhanceCheck, weaponList[0].WpEnhanceLevel, weaponList[0].WpEquip));

            for (int i = 0; i < skillList.Count; i++)
            {
                skills.Add(new Skill(skillList[i].SkillActiveLevel, skillList[i].SkillName, 
                    skillList[i].SkillMp, skillList[i].IsActive));
            }

            for (int i = 0; i < weaponList.Count; i++)
            {
                weapons.Add(new Weapon(weaponList[i].Code, weaponList[i].WpName, weaponList[i].WpStr, weaponList[i].WpDef,
                    weaponList[i].WpMaxDur, weaponList[i].WpMinDur, weaponList[i].WpLevel, weaponList[i].WpGold,
                    weaponList[i].WpType, weaponList[i].WpEnhanceCheck, weaponList[i].WpEnhanceLevel, weaponList[i].WpEquip));
            }

            Dictionary<int, Weapon> weaponDictionary = new Dictionary<int, Weapon>();
            foreach (var weapon in weapons)
            {
                weaponDictionary[weapon.Code] = weapon;
            }

            while (true)
            {
                UpdateSkillState(player, skills);
                monsters.Clear();
                player.ShowInfo();
                Lobby();
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    Random random = new Random();

                    switch (result)
                    {
                        case 1:
                            Console.Clear();

                            int mobRandom = random.Next(5, 6);
                            monsters.Add(new Monster(monsterList[mobRandom].Name, monsterList[mobRandom].Hp, monsterList[mobRandom].Mp, 
                                monsterList[mobRandom].Str, monsterList[mobRandom].Def, monsterList[mobRandom].Exp, monsterList[mobRandom].IsStun,
                                monsterList[mobRandom].MinGold, monsterList[mobRandom].MaxGold));
                            monsters[0].isAlive = true;

                            if (!player.isAlive)
                            {
                                player.ShowInfo();
                                Console.WriteLine("=====================");
                                Console.WriteLine();
                                Console.WriteLine("죽었습니다, 부활하세요");
                                Console.WriteLine();
                                Console.WriteLine("=====================");
                                Thread.Sleep(1000);
                                Console.Clear();
                                break;
                            }

                            Battle(player, monsters, skills);

                            if (player.curExp >= player.maxExp)
                            {
                                player.LevelUp();
                            }

                            Console.Clear();
                            break;
                        case 2:
                            Console.Clear();
                            Sauna(player);
                            continue;
                        case 3:
                            ShowSkills(skills, player);
                            Console.Clear();
                            continue;
                        case 4:
                            Console.Clear();
                            player.Inventory(inven);
                            continue;
                        case 5:
                            Console.Clear();
                            Store(player, weaponDictionary, inven);
                            break;
                        default:
                            Console.Clear();
                            continue;
                    }
                }
            }
        }

        #region 시작화면
        static void Start()
        {
            while (true)
            {
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
                Console.WriteLine("{0, -19}■", "■");
                Console.WriteLine("■ 아직 제목 안정함 ■");
                Console.WriteLine("{0, -19}■", "■");
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");

                Console.WriteLine("====================");
                Console.WriteLine("    1. 게임시작");
                Console.WriteLine("    2. 게임종료");
                Console.WriteLine("====================");

                if (int.TryParse(Console.ReadLine(), out int result) && result == 2)
                {
                    Console.WriteLine("게임을 종료합니다.");
                    break;
                }
                else if (result == 1)
                {
                    Console.Write("게임을 시작합니다.");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("1번과 2번만 입력하세요.");
                    Thread.Sleep(500);
                    Console.Clear();
                }
            }
        }
        #endregion

        #region 생성
        static void Create()
        {
            while (true)
            {
                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.WriteLine("   플레이어 이름을");
                Console.WriteLine("     입력하세요");
                Console.WriteLine();
                Console.WriteLine("=====================");
                Console.Write("    ▶ ");
                playerName = Console.ReadLine();

                Thread.Sleep(1000);
                Console.Clear();

                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.WriteLine("    ▶ 생성완료 ◀");
                Console.WriteLine($"    ▶ {playerName} ◀");
                Console.WriteLine("    ▶ HP : 100 ◀");
                Console.WriteLine("    ▶ MP :  50 ◀");
                Console.WriteLine("    ▶ STR : 20 ◀");
                Console.WriteLine("    ▶ DEF : 10 ◀");
                Console.WriteLine();
                Console.WriteLine("=====================");

                Thread.Sleep(2000);
                Console.Clear();
                break;
            }
        }
        #endregion

        #region 로비
        static void Lobby()
        {
            Console.WriteLine("=====================");
            Console.WriteLine();
            Console.WriteLine(" 1.사냥진행 2.찜질방");
            Console.WriteLine(" 3.스킬목록 4.인벤토리");
            Console.WriteLine(" 5.  상점");
            Console.WriteLine();
            Console.WriteLine("=====================");
        }
        #endregion

        #region 전투
        static void Battle(Player player, List<Monster> monsters, List<Skill> skills)
        {
            Random random = new Random();

            while (player.isAlive && monsters[0].isAlive)
            {
                player.ShowInfo();
                if (player.CurHp <= 0) 
                {
                    player.CurHp = 0;
                    player.Die(); 
                }

                Console.WriteLine("          VS");
                monsters[0].ShowInfo();

                if (!player.isAlive && monsters[0].isAlive)
                {
                    Console.WriteLine("전투 종료!");
                    break;
                }
                
                Console.WriteLine();
                Console.WriteLine($"▶{playerName}님이 공격 할 차례입니다◀");
                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.WriteLine("1.기본공격 2.스킬쓰기");
                Console.WriteLine("           4.도망가기");
                Console.WriteLine();
                Console.WriteLine("=====================");

                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    switch (result)
                    {
                        case 1:
                            Console.Clear();
                            player.ShowInfo();
                            monsters[0].ShowInfo();
                            Console.WriteLine($"    {monsters[0].Name}에게 기본공격!");
                            player.Attack(monsters[0]);
                            Thread.Sleep(1000);
                            break;
                        case 2:
                            SkillsInfo(player, skills);
                            if (cancelSkill == true)
                            {
                                cancelSkill = false;
                                Console.Clear();
                                continue;
                            }
                            player.Skills(monsters[0], selectSkill);
                            Console.Clear();
                            player.ShowInfo();
                            monsters[0].ShowInfo();
                            break;
                        case 4:
                            Console.WriteLine("성공적으로 도망가셨습니다.");
                            Thread.Sleep(1000);
                            break;
                        default:
                            Console.Clear();
                            continue;
                    }
                    if (result == 4)
                        break;
                }

                foreach (Monster monster in monsters)
                {
                    if (monster.CurHp <= 0)
                    {
                        monsters[0].CurHp = 0;
                        monster.Die();
                    }
                }
                if (player.isAlive && !monsters[0].isAlive)
                {
                    player.curExp += monsters[0].Exp;
                    int gold = random.Next(monsters[0].MinGold, monsters[0].MaxGold + 1);
                    player.gold += gold;

                    Console.WriteLine("         전투 종료!");
                    Thread.Sleep(1000);
                    Console.WriteLine($"     {monsters[0].Exp}의 경험치 획득!");
                    Thread.Sleep(1000);
                    Console.WriteLine($"    {gold}만큼의 골드 획득!");
                    Thread.Sleep(1000);
                    break;
                }

                {
                    Console.Clear();
                    player.ShowInfo();
                    monsters[0].ShowInfo();
                    Console.WriteLine();

                    if (monsters[0].IsStun == true)
                    {
                        Console.WriteLine($"{monsters[0].Name}이(가) 기절이므로 공격할 수 없습니다.");
                        Thread.Sleep(1000);
                        monsters[0].IsStun = false;
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"▶{monsters[0].Name}이(가) 공격 할 차례입니다◀");

                        Thread.Sleep(1000);
                        Console.Clear();

                        player.ShowInfo();
                        monsters[0].ShowInfo();
                        Console.WriteLine($"    {player.Name}에게 기본공격!");
                        monsters[0].Attack(player);
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                    
                }   
            }
        }
        #endregion

        #region 스킬목록
        static void ShowSkills(List<Skill> skills, Player player)
        {
            bool returnMenu = false;
            Console.Clear();
            while (returnMenu == false)
            {
                Console.WriteLine("=================스킬목록=================");
                Console.WriteLine();
                Console.WriteLine("\t\t 습득스킬");
                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].SkillActiveLevel <= player.level)
                    {
                        Console.WriteLine($"이름: {skills[i].SkillName}\t 레벨: {skills[i].SkillActiveLevel}\t 마나: {skills[i].SkillMp}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\t        미습득스킬");
                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].SkillActiveLevel > player.level)
                    {
                        Console.WriteLine($"이름: {skills[i].SkillName}\t 레벨: {skills[i].SkillActiveLevel}\t 마나: {skills[i].SkillMp}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("==========================================");
                Console.WriteLine();
                Console.WriteLine("1. 돌아가기");
                Console.WriteLine("2. 스킬설명 [스킬이름을 입력하세요]");
                Console.WriteLine("3. 스킬설명으로 돌아가기");

                Console.WriteLine();
                Console.Write("▶ ");

                while (true)
                {
                    string input = Console.ReadLine();
                    Console.WriteLine();

                    if (int.TryParse(input, out int result))
                    {
                        if (result == 1)
                        {
                            Console.Clear();
                            returnMenu = true;
                            break;
                        }
                        else if (result == 3)
                        {
                            Console.Clear();
                            break;
                        }
                    }
                    else
                    {
                        switch (input)
                        {
                            case "암살":
                                Console.WriteLine("[ 암살 ] 5% 확률로 대상을 즉사시킵니다.");
                                break;
                            case "파워어택":
                                Console.WriteLine("[ 파워어택 ] 기본공격의 두 배의 피해를 입힙니다.");
                                break;
                            case "메테오":
                                Console.WriteLine("[ 메테오 ] 기본공격의 1.5배의 피해를 입히고 공격한 대상을 기절시킵니다.");
                                break;
                            case "용의강림":
                                Console.WriteLine("[ 용의강림 ] 기본공격의 두 배의 피해를 입히고 대상의 방어력을 무시합니다.");
                                break;
                            case "천벌":
                                Console.WriteLine("[ 천벌] 기본공격의 두 배의 피해를 입힙니다. 50% 확률로 세 배의 피해를 입힙니다.");
                                break;
                            case "자폭":
                                Console.WriteLine("[ 자폭] 자폭하여 자신의 HP를 절반 잃지만, 대상의 현재체력의 70%의 데미지를 줍니다.");
                                break;
                        }
                    }

                    //Console.WriteLine("1. 돌아가기");
                    //if (int.TryParse(Console.ReadLine(), out int number))
                    //{
                    //    if (number == 1)
                    //    {
                    //        Console.Clear();
                    //        break;
                    //    }
                    //}
                }
                
            }
        }
        #endregion

        #region 스킬정보
        static void SkillsInfo(Player player, List<Skill> skills)
        {
            bool isBool = true;

            while (isBool)
            {
                Console.Clear();
                Console.WriteLine("=================스킬목록=================");
                Console.WriteLine();
                Console.WriteLine("\t\t 습득스킬");
                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].SkillActiveLevel <= player.level)
                    {
                        Console.WriteLine($"이름: {skills[i].SkillName}\t 레벨: {skills[i].SkillActiveLevel}\t 마나: {skills[i].SkillMp}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\t        미습득스킬");
                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].SkillActiveLevel > player.level)
                    {
                        Console.WriteLine($"이름: {skills[i].SkillName}\t 레벨: {skills[i].SkillActiveLevel}\t 마나: {skills[i].SkillMp}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("==========================================");
                Console.WriteLine();
                Console.WriteLine("1.돌아가기");
                Console.WriteLine();
                Console.Write("사용할 스킬 입력 ▶ ");
                
                selectSkill = Console.ReadLine();

                if (int.TryParse(selectSkill, out int result))
                {
                    if (result == 1)
                    {
                        isBool = false;
                        cancelSkill = true;
                        break;
                    }
                }
                else
                {
                    for (int i = 0; i < skills.Count; i++)
                    {
                        if (skills[i].IsActive == true && selectSkill == skills[i].SkillName && player.CurMp >= skills[i].SkillMp)
                        {
                            Console.WriteLine($"{skills[i].SkillName} 사용!");
                            Thread.Sleep(2000);
                            isBool = false;
                            break;
                        }

                        if (player.CurMp <  skills[i].SkillMp)
                        {
                            Console.WriteLine("마나가 부족합니다.");
                            Thread.Sleep(1000);
                            break;
                        }

                        if (skills[i].IsActive == false)
                        {
                            Console.WriteLine($"사용불가 스킬입니다!");
                            Thread.Sleep(2000);
                            break;
                        }     
                    }
                }
            }
        }
        #endregion

        #region 찜질방
        static void Sauna(Player player)
        {
            if (player.CurHp >= player.Hp)
            {
                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.WriteLine(" 이미 최대체력입니다!");
                Console.WriteLine();
                Console.WriteLine("=====================");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else if (player.CurHp <= 0)
            {
                player.CurHp = player.Hp;
                player.CurMp = player.Mp;
                player.isAlive = true;
                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.WriteLine("    캐릭터 부활!");
                Console.WriteLine();
                Console.WriteLine("=====================");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else
            {
                player.CurHp = player.Hp;
                player.CurMp = player.Mp;
                Console.WriteLine("=====================");
                Console.WriteLine();
                Console.WriteLine("   HP 최대로 회복!");
                Console.WriteLine("   MP 최대로 회복!");
                Console.WriteLine();
                Console.WriteLine("=====================");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }
        #endregion

        static void UpdateSkillState(Player player, List<Skill> skills)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (player.level >= skills[i].SkillActiveLevel)
                {
                    skills[i].IsActive = true;
                }
            }
        }

        #region 상점
        static void Store(Player player, Dictionary<int, Weapon> weaponDictionary, List<Weapon> inven)
        {
            while (true)
            {
                Console.WriteLine("===============================상점===============================");
                foreach (var weapon in weaponDictionary.Values)
                {
                    if (weapon.Code <= 10)
                    {
                        Console.WriteLine($"{weapon.Code,-2} {weapon.WpName,-10}Str: {weapon.WpStr,-5}Lv: {weapon.WpLevel,-5}" +
                            $"Gold: {weapon.WpGold,-8}Type: {weapon.WpType}");
                    }
                }
                Console.WriteLine();
                foreach (var weapon in weaponDictionary.Values)
                {
                    if (weapon.Code > 10)
                    {
                        Console.WriteLine($"{weapon.Code,-2} {weapon.WpName,-10}Def: {weapon.WpDef,-5}Lv: {weapon.WpLevel,-5}" +
                            $"Gold: {weapon.WpGold,-8}Type: {weapon.WpType}");
                    }
                }
                Console.WriteLine("==================================================================");
                Console.WriteLine();
                Console.WriteLine("0.돌아가기");
                Console.WriteLine("아이템사기(아이템코드입력)");
                Console.Write("▶ ");

                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (result == 0)
                    {
                        Console.Clear();
                        break;
                    }

                    if (weaponDictionary.TryGetValue(result, out Weapon selectedWeapon))
                    {
                        if (player.level >= selectedWeapon.WpLevel)
                        {
                            if (player.gold >= selectedWeapon.WpGold)
                            {
                                Console.WriteLine($"{selectedWeapon.WpName}을(를) 구매했습니다.");
                                Console.WriteLine($"{selectedWeapon.WpGold}골드를 지불, 남은골드: {player.gold - selectedWeapon.WpGold}골드");
                                player.gold -= selectedWeapon.WpGold;
                                inven.Add(selectedWeapon);
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족합니다.");
                                Thread.Sleep(1000);
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine("레벨이 낮아 구매할 수 없습니다.");
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("아이템을 찾을 수 없습니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }
        }
        #endregion
    }
}

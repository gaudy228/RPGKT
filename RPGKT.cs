namespace RPG
{
    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.Write("введите имя героя: ");
            string heroName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("выберите класс героя:");
            Console.WriteLine("1. воин");
            Console.WriteLine("2. маг");
            Console.WriteLine("3. лучник");
            int classChoice = Convert.ToInt32(Console.ReadLine());
            Hero hero = null;

            switch (classChoice)
            {
                case 1:
                    hero = new Hero(heroName, "Воин", 100, 35, 5);
                    break;
                case 2:
                    hero = new Hero(heroName, "Маг", 90, 25, 25);
                    break;
                case 3:
                    hero = new Hero(heroName, "Лучник", 80, 30, 15);
                    break;
            }

            hero.VievStat();

            while (hero.Wins < 15 && hero.Health > 0)
            {
                Console.WriteLine();
                Monster monster = CreationMonster();
                Console.WriteLine($"на вас напал {monster.Name}, его здоровье: {monster.Health}, урон: {monster.AttackPower})");

                Battle(hero, monster);
              
                if (hero.Health <= 0)
                {
                    break;
                }
                else
                {
                    hero.Wins++;
                    int expGained = rnd.Next(1, 100);
                    hero.Experience += expGained;
                    Console.WriteLine($"вы получили {expGained} опыта, всего опыта: {hero.Experience}");

                    Console.WriteLine("хотите восстановить часть здоровья? (да/нет)");
                    string resp = Console.ReadLine();
                    if (resp == "да")
                    {
                        hero.RestoreHealth();
                        hero.VievStat();
                    }
                    CheckLvlUp(hero);
                    Console.WriteLine();
                }
            }
            if (hero.Health <= 0)
            {
                Console.WriteLine("вас убили");
            }
            else if (hero.Wins >= 15)
            {
                Console.WriteLine("вы прошли игру");
            }
        }
        static Monster CreationMonster()
        {
            string[] monsterNames = {"зомби", "паук", "скелет"};
            string name = monsterNames[rnd.Next(monsterNames.Length)];
            int health = rnd.Next(50, 100);
            int attack = rnd.Next(10, 30);
            return new Monster(name, health, attack);
        }
        static void Battle(Hero hero, Monster monster)
        {
            while (hero.Health > 0 && monster.Health > 0)
            {
                Console.WriteLine();
                int damage = hero.AttackPower;

                double critRoll = rnd.NextDouble() * 100;
                if (critRoll <= hero.CritChance)
                {
                    damage *= 2;
                    Console.WriteLine("крит");
                }

                monster.Health -= damage;
                Console.WriteLine($"вы атакуете {monster.Name} и наносите {damage} урона, здоровье монстра: {monster.Health}");

                if (monster.Health <= 0)
                {
                    Console.WriteLine($"{monster.Name} убит");
                    break;
                }

                hero.Health -= monster.AttackPower;
                Console.WriteLine($"{monster.Name} атакует и наносит {monster.AttackPower} урона, здоровье героя: {hero.Health}");
            }
            Console.WriteLine();
            hero.VievStat();
        }

        static void CheckLvlUp(Hero hero)
        {
            int[] experienceForLvlUp = {0, 100, 250, 322, 500, 1000};
            int newLevel = hero.Level;
            for (int i = experienceForLvlUp.Length - 1; i > 0; i--)
            {
                if (hero.Experience >= experienceForLvlUp[i])
                {
                    newLevel = i + 1;
                    break;
                }
            }
            if (newLevel > hero.Level)
            {
                Console.WriteLine();
                Console.WriteLine($"вы достигли уровня {newLevel}");
                hero.Level = newLevel;

                Console.WriteLine("улучшите одну характеристику: ");
                Console.WriteLine("1. Здоровье");
                Console.WriteLine("2. Сила атаки");
                Console.WriteLine("3. Шанс критического удара");
                int choice = Convert.ToInt32(Console.ReadLine());
                hero.LevelUp(choice);
                hero.VievStat();
            }
        }
        class Hero
        {
            public string Name;
            public string ClassName;
            public int MaxHealth;
            public int Health;
            public int AttackPower;
            public double CritChance;
            public int Experience;
            public int Level;
            public int Wins;
            public int RestoresLeft;

            public Hero(string name, string className, int maxHealth, int attackPower, double critChance)
            {
                Name = name;
                ClassName = className;
                MaxHealth = maxHealth;
                Health = MaxHealth;
                AttackPower = attackPower;
                CritChance = critChance;
                Experience = 0;
                Level = 1;
                Wins = 0;
                RestoresLeft = 5;
            }
            public void LevelUp(int stat)
            {
                if (stat == 1)
                {
                    MaxHealth += 30;
                }
                else if (stat == 2)
                {
                    AttackPower += 10;
                }
                else if (stat == 3)
                {
                    CritChance += 5;
                }
            }
            public void RestoreHealth()
            {
                if (RestoresLeft > 0)
                {
                    Console.WriteLine();
                    Health = MaxHealth;
                    RestoresLeft--;
                    Console.WriteLine($"восстановлено здоровье");
                }
            }
            public void VievStat()
            {
                Console.WriteLine();
                Console.WriteLine("ваши статы: ");
                Console.WriteLine($"здоровье: {Health}/{MaxHealth}");
                Console.WriteLine($"атака: {AttackPower}");
                Console.WriteLine($"крит шанс: {CritChance}");
            }
        }
        class Monster
        {
            public string Name;
            public int Health;
            public int AttackPower;
            public Monster(string name, int health, int attackPower)
            {
                Name = name;
                Health = health;
                AttackPower = attackPower;
            }
        }
    }
}

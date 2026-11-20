using System.Net.Http.Json;

internal class Program
{
    #region Klasser
    public class Character
    {
        public string Name { get; set; }
        public int Health { get; set; } = 100;
        public HeavyWeapon HeavyWeapon { get; set; }
        public LightWeapon LightWeapon { get; set; }
        public Armor Armor { get; set; }
        public Shield Shield { get; set; }
    }
    public class HeavyWeapon
    {
        public string Name { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int HitChance { get; set; }
    }
    public class LightWeapon
    {
        public string Name { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int HitChance { get; set; }
    }
    public class Armor
    {
        public string Name { get; set; }
        public double MaxArmorMultiplier { get; set; }
        public double MinArmorMultiplier { get; set; }
    }
    public class Shield
    {
        public string Name { get; set; }
        public int BlockChance { get; set; }
    }
    public class WeatherResponse
    {
        public CurrentWeather Current { get; set; }
    }
    public class CurrentWeather
    {
        public double Rain { get; set; }
        public double Snowfall { get; set; }
    }
    #endregion

    #region Weapon / stats
    static List<HeavyWeapon> heavyWeapons = new List<HeavyWeapon>
    {
        new HeavyWeapon { Name = "Great sword", MinDamage = 25, MaxDamage = 30, HitChance = 45 },
        new HeavyWeapon { Name = "Sword", MinDamage = 18, MaxDamage = 21, HitChance = 50 },
        new HeavyWeapon { Name = "Mace", MinDamage = 15, MaxDamage = 30, HitChance = 60 },
        new HeavyWeapon { Name = "Javelin", MinDamage = 17, MaxDamage = 22, HitChance = 50 },
        new HeavyWeapon { Name = "Spear", MinDamage = 16, MaxDamage = 21, HitChance = 55 }
    };

    static List<LightWeapon> lightWeapons = new List<LightWeapon>
    {
        new LightWeapon { Name = "Dirk", MinDamage = 8, MaxDamage = 9, HitChance = 100 },
        new LightWeapon { Name = "Tomahawk", MinDamage = 6, MaxDamage = 11, HitChance = 90 },
        new LightWeapon { Name = "Jambiya", MinDamage = 9, MaxDamage = 9, HitChance = 95 }
    };

    static List<Shield> shields = new List<Shield>
    {
        new Shield { Name = "Parma", BlockChance = 25 },
        new Shield { Name = "Scutum", BlockChance = 25 }
    };

    static Dictionary<string, Armor> armors = new Dictionary<string, Armor>
    {
        { "Centurion", new Armor { Name = "Centurion"} },
        { "Dimachaeri", new Armor { Name = "Dimachaeri"} }
    };

    static Dictionary<string, int> weatherPenalties = new Dictionary<string, int>
    {
        { "Clear", 0 },
        { "Rain", 10 },
        { "Snow", 15 }
    };
    #endregion

    private static void Main(string[] args)
    {
        bool startGameScreen = true;
        Dictionary<string, Action> gameModes = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
        {
            { "PVP", StartPvP },
        };

        #region Start
        Intro();

        while (startGameScreen)
        {
            Console.WriteLine("The heavy iron portcullis groans upward, flooding the dark stone tunnel with blinding, merciless sunlight. As you step \nonto the scorched earth of the Crimson Pit, the sheer wall of sound from ten thousand spectators hits you like a \nphysical blow. Dust hangs thick in the air, carrying the sharp, metallic tang of dry ash, sweat, and sun-baked clay.\r\n\r\nAcross the expanse of trampled sand, the opposing gate slams shut with a finality that echoes in your chest. Your rival emerges from the shadows—a silhouette of scarred muscle and cold, unblinking focus, already pacing the perimeter like a caged hound. There are no banners flown in honor here, no armor to hide behind, and no forged steel to carry the burden of survival. There are only two warriors, bound by the ancient decree of Vespera, circling each other as the crowd’s \nanticipation reaches a fever pitch.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nWrite pvp to continue...");
            string gameMode = Console.ReadLine() ?? "";
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Clear();
            #endregion

            if (gameModes.ContainsKey(gameMode))
            {
                gameModes[gameMode]();
                startGameScreen = false;
            }
            else
            {
                Console.Clear();
            }
        }
    }

    #region PvP
    static void StartPvP()
    {
        Console.Write("What is the name of the first player? : ");
        string player1Name = Console.ReadLine() ?? "";
        Console.Write("What is the name of the second player? : ");
        string player2Name = Console.ReadLine() ?? "";

        Character player1 = new Character { Name = player1Name, Health = 100 };
        Character player2 = new Character { Name = player2Name, Health = 100 };

        foreach (var player in new[] { player1, player2 })
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"=== {player.Name}'s turn to choose equipment (HP: {player.Health}) ===\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            // Choose Heavy Weapon
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Heavy slot:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < heavyWeapons.Count; i++)
            {
                var w = heavyWeapons[i];
                string dmg = w.MinDamage == w.MaxDamage ? $"{w.MinDamage}" : $"{w.MinDamage} - {w.MaxDamage}";
                Console.WriteLine($"  {i + 1}. {w.Name} -- {dmg} damage ({w.HitChance}% hit chance)");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Choose a heavy weapon (1-5): ");
            int heavyChoice = Choice(1, heavyWeapons.Count) - 1;
            player.HeavyWeapon = heavyWeapons[heavyChoice];
            Console.ForegroundColor = ConsoleColor.DarkGray;

            // Choose Light Weapon
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nLight slot:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < lightWeapons.Count; i++)
            {
                var w = lightWeapons[i];
                string dmg = w.MinDamage == w.MaxDamage ? $"{w.MinDamage}" : $"{w.MinDamage} - {w.MaxDamage}";
                Console.WriteLine($"  {i + 1}. {w.Name} -- {dmg} damage ({w.HitChance}% hit chance)");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Choose a light weapon (1-3): ");
            int lightChoice = Choice(1, lightWeapons.Count) - 1;
            player.LightWeapon = lightWeapons[lightChoice];
            Console.ForegroundColor = ConsoleColor.DarkGray;

            // Choose Shield
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nShield slot:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < shields.Count; i++)
            {
                var s = shields[i];
                Console.WriteLine($"  {i + 1}. {s.Name} ({s.BlockChance}% block chance)");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Choose a shield (1-2): ");
            int shieldChoice = Choice(1, shields.Count) - 1;
            player.Shield = shields[shieldChoice];
            Console.ForegroundColor = ConsoleColor.DarkGray;

            // Choose Armor via coin flip med Dictionary
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nThe armor you got was chosen by a random generation with a coin flip");
            int randomArmorGeneration = Random.Shared.Next(0, 2);
            if (randomArmorGeneration == 0)
            {
                Console.WriteLine("The coin landed on tails!");
                Console.WriteLine("The armor you got is Centurion armor\nYour armor is only a cosmetic and wont affect your gameplay.");
                player.Armor = armors["Centurion"];
            }
            else
            {
                Console.WriteLine("The coin landed on heads!");
                Console.WriteLine("The armor you got is Dimachaeri armor\nYour armor is only a cosmetic and wont affect your gameplay.");
                player.Armor = armors["Dimachaeri"];
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        Queue<Character> playerTurnQueue = new Queue<Character>();
        playerTurnQueue.Enqueue(player1);
        playerTurnQueue.Enqueue(player2);

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{player1.Name} will be going up against {player2.Name}!\n");
        Console.ForegroundColor = ConsoleColor.DarkGray;

        string currentWeather = "Clear";
        try
        {
            using var client = new HttpClient();
            string url = "https://api.open-meteo.com/v1/forecast?latitude=52.512604&longitude=13.419517&current=rain,snowfall";
            var weatherData = client.GetFromJsonAsync<WeatherResponse>(url).GetAwaiter().GetResult();

            if (weatherData?.Current != null)
            {
                if (weatherData.Current.Rain > 0) currentWeather = "Rain";
                else if (weatherData.Current.Snowfall > 0) currentWeather = "Snow";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No weather data was found ({ex.Message}). The game will continue with the sun out.");
        }

        int weatherPenalty = weatherPenalties[currentWeather];

        if (currentWeather == "Rain")
        {
            Console.WriteLine($"Its raining in the arena! The weather makes it so that hit chance is {weatherPenalty}%units lower.");
        }
        else if (currentWeather == "Snow")
        {
            Console.WriteLine($"Its snowing in the arena! The weather makes it so that hit chance is {weatherPenalty}%units lower.");
        }
        else
        {
            Console.WriteLine("The sun is out in the arena! NO weather penalties.");
        }

        Console.WriteLine("Press Enter to start fight...");
        Console.ReadLine();

        // Battle Loop
        while (player1.Health > 0 && player2.Health > 0)
        {
            Console.Clear();
            Character attacker = playerTurnQueue.Dequeue();
            Character defender = playerTurnQueue.Peek();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"=== It is {attacker.Name}'s turn to attack! ===");
            Console.WriteLine($"{attacker.Name} HP: {attacker.Health} | {defender.Name} HP: {defender.Health}\n");
            Console.WriteLine("Choose weapon to attack with:");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"1. Heavy: {attacker.HeavyWeapon.Name} ({attacker.HeavyWeapon.MinDamage}-{attacker.HeavyWeapon.MaxDamage} dmg) ({attacker.HeavyWeapon.HitChance}% hit chance)");
            Console.WriteLine($"2. Light: {attacker.LightWeapon.Name} ({attacker.LightWeapon.MinDamage}-{attacker.LightWeapon.MaxDamage} dmg) ({attacker.HeavyWeapon.HitChance}% hit chance)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nYour choice (1-2): ");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            int weaponChoice = Choice(1, 2);
            int damageDealt = 0;

            if (weaponChoice == 1)
            {
                damageDealt = CalculateDamage(
                    attacker.HeavyWeapon.MinDamage,
                    attacker.HeavyWeapon.MaxDamage,
                    attacker.HeavyWeapon.HitChance - weatherPenalty,
                    attacker.HeavyWeapon.Name,
                    defender.Shield.BlockChance);
            }
            else
            {
                damageDealt = CalculateDamage(
                    attacker.LightWeapon.MinDamage,
                    attacker.LightWeapon.MaxDamage,
                    attacker.LightWeapon.HitChance - weatherPenalty,
                    attacker.LightWeapon.Name,
                    defender.Shield.BlockChance);
            }

            defender.Health -= damageDealt;
            if (defender.Health < 0) defender.Health = 0;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{defender.Name} has {defender.Health} HP remaining.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            playerTurnQueue.Enqueue(attacker);

            if (defender.Health <= 0)
            {
                Console.Clear();
                Console.WriteLine($"As the final blow echoes across the arena and one warrior collapses motionless into the dark, \ntrampled dirt, the deafening roar of the amphitheater swells to a crescendo—then abruptly dies. \nTen thousand heads turn upward toward the high marble gallery, where the Emperor \nrises slowly from his gilded throne, his heavy crimson robes catching the dry arena wind.\r\n\r\nHe steps to the stone balustrade, resting ring-laden hands \nupon the parapet as his cold, unreadable gaze sweeps over the Crimson Pit. \nHe looks first at the broken fighter sprawled in the dust, weighing \nthe courage of their struggle, before locking eyes with the lone survivor \nstanding chest-heaving in the center of the ring. To the Emperor, \nsweat and bruised bone are merely the currency that buys the peace of Vespera.\r\n\r\nWith agonizing, deliberate slowness, he raises his right hand over the edge of the balcony. \nA suffocating silence falls over the coliseum, broken only by \nragged breathing in the sand below. Then, with a sharp, unmistakable turn of his wrist, \nthe Emperor delivers the imperial verdict. Bronze trumpets blare in harsh unison, \nthe iron gates grind open once more, and the {attacker.Name} name is roared \nto the heavens—while the loser is dragged back into the dark.");
                Console.ReadKey();
                break;
            }

            Console.WriteLine("\nPress Enter for next round...");
            Console.ReadLine();
        }
    }
    #endregion

    #region Combat Logic
    static int CalculateDamage(int minDmg, int maxDmg, int hitChance, string weaponName, int BlockChance)
    {
        int sheildBlockchance = 30;
        int roll = Random.Shared.Next(1, 101);
        if (roll <= hitChance)
        {
            if (Random.Shared.Next(100) < sheildBlockchance)
            {
                Console.WriteLine($"\nAttack with {weaponName} was BLOCKED by the shield!");
                return 0;
            }

            int damage = Random.Shared.Next(minDmg, maxDmg + 1);
            Console.WriteLine($"\nAttack with {weaponName} HIT and dealt {damage} damage!");
            return damage;
        }
        else
        {
            Console.WriteLine($"\nAttack with {weaponName} MISSED!");
            return 0;
        }
    }
    #endregion

    #region CenteredText
    static void CentreraText(string text)
    {
        int fonsterBredd = Console.WindowWidth;
        int blanksteg = (fonsterBredd - text.Length) / 2;
        if (blanksteg < 0) blanksteg = 0;
        Console.WriteLine(new string(' ', blanksteg) + text);
    }
    #endregion

    #region Weapon Choice
    static int Choice(int min, int max)
    {
        int val;
        while (!int.TryParse(Console.ReadLine(), out val) || val < min || val > max)
        {
            Console.Write($"PLease type a number within the range ({min}-{max}): ");
        }
        return val;
    }
    #endregion
    static void Intro ()
    {
        Console.ForegroundColor = ConsoleColor.White;
        string text = "<===== Gladiator =====>";
        CentreraText(text);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nBeneath the towering stone arches of Vespera, forged steel is strictly forbidden. Long ago, the Arena Magistrates \ndecreed that blades end a bout too quickly, robbing the roaring crowds of true spectacle. Instead, warriors are cast \ninto the Crimson Pit armed with nothing but scarred knuckles, iron discipline, and sheer willpower. \nHere, the law of the arena is absolute: two gladiators step onto the sun-baked dirt, and the heavy gates remain bolted \nuntil only one is left standing.");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\npress any button to continue...");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.ReadLine();
        Console.Clear();
    }
}
//ALl lore är skriven av gemini
//jhvj
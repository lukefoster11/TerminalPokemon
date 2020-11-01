using System;
using System.Collections.Generic;

namespace Pokemon
{
    // deal with all Battle situations
    public class Battle
    {
        // carry out a pokemon battle given set pokemonInstances
        public static void DoBattle(List<PokemonInstance> playerParty, List<PokemonInstance> opponentParty)
        {
            // make Random rnd
            Random rnd = new Random();

            // create empty list for status effects for each pokemon in player party
            foreach (PokemonInstance pokemon in playerParty)
            {
                pokemon.StatusEffects = new List<string>();
            }

            // create empty list for status effects for each pokemon in opponent party
            foreach (PokemonInstance pokemon in opponentParty)
            {
                pokemon.StatusEffects = new List<string>();
            }

            // create accuracy and evasion stats for each pokemon (Stats[6] and Stats[7])
            foreach (PokemonInstance pokemon in playerParty)
            {
                pokemon.Stats.Add(100);
                pokemon.Stats.Add(100);
            }
            foreach (PokemonInstance pokemon in opponentParty)
            {
                pokemon.Stats.Add(100);
                pokemon.Stats.Add(100);
            }

            // make copies of default stats for player's pokemon
            List<List<int>> playerPartyDefaultStats = new List<List<int>>();
            foreach (PokemonInstance pokemon in playerParty)
            {
                List<int> stats = new List<int>();
                foreach (int stat in pokemon.Stats)
                {
                    stats.Add(stat);
                }
                playerPartyDefaultStats.Add(stats);
            }

            // make copies of default stats for opponents's pokemon
            List<List<int>> opponentPartyDefaultStats = new List<List<int>>();
            foreach (PokemonInstance pokemon in opponentParty)
            {
                List<int> stats = new List<int>();
                foreach (int stat in pokemon.Stats)
                {
                    stats.Add(stat);
                }
                opponentPartyDefaultStats.Add(stats);
            }
            
            // create stat stages that affect calculated stats
            foreach (PokemonInstance pokemon in playerParty)
            {
                pokemon.StatStages = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            foreach (PokemonInstance pokemon in opponentParty)
            {
                pokemon.StatStages = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            
            // keep track of which pokemon of the parties are in play
            int playerActivePokemon = 0;
            int opponentActivePokemon = 0;

            // display stats for each pokemon
            DisplayStats(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerPartyDefaultStats[playerActivePokemon][0], opponentPartyDefaultStats[opponentActivePokemon][0]);

            // create a loop that continues as long as battle is going
            while (true)
            {
                // display moves for player's pokemon
                DisplayMoves(playerParty[playerActivePokemon]);

                // create a bool that becomes true when user inputs a correct move
                bool match = false;

                // create a string userInput that userInput will be stored into
                string userInput = "";

                // TODO: recharge

                // prompt user for input until they input a valid action
                    // TODO: other actions (switch pokemon, use item, etc.)
                    // TODO: RECHARGE STATUS EFFECT (FROM HYPER BEAM)
                while (!match)
                {
                    userInput = Console.ReadLine().ToUpper();
                    foreach (Move move in playerParty[playerActivePokemon].Moves)
                    {
                        if (move.MoveName.ToUpper() == userInput && move.PP > 0)
                        {
                            move.PP -= 1;
                            match = true;
                        }
                    }
                }

                // store the selected move into Move playerMove
                Move playerMove = playerParty[playerActivePokemon].Moves.Find(x => x.MoveName.ToUpper() == userInput);

                // randomly select a move for the opponent to use and store in Move opponentMove
                    // TODO: add some sort of AI
                Move opponentMove = opponentParty[opponentActivePokemon].Moves[rnd.Next(opponentParty[opponentActivePokemon].Moves.Count)];

                // display stats again, no longer displaying player moves
                DisplayStats(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerPartyDefaultStats[playerActivePokemon][0], opponentPartyDefaultStats[opponentActivePokemon][0]);

                // call DoMoves method, carrying out the rest of the turn
                    // TODO: this only works with 2 moves
                        // TODO: create new method for other turn actions (switch pokemon, etc.)
                DoMoves(playerParty, playerActivePokemon, opponentParty, opponentActivePokemon, playerPartyDefaultStats, opponentPartyDefaultStats, playerMove, opponentMove);
                
                // check if either pokemon fainted
                if (opponentParty[opponentActivePokemon].Stats[0] <= 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"The opposing {opponentParty[opponentActivePokemon].PokemonValue.PokemonName} fainted.");
                }
                if (playerParty[playerActivePokemon].Stats[0] <= 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"Your {playerParty[playerActivePokemon].PokemonValue.PokemonName} fainted.");
                }
                if (opponentParty[opponentActivePokemon].Stats[0] <= 0 || playerParty[playerActivePokemon].Stats[0] <= 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
            }

            // return all pokemon stats to default values
            for (int i = 0; i < playerParty.Count; i++)
            {
                playerParty[i].Stats = playerPartyDefaultStats[i];
                opponentParty[i].Stats = opponentPartyDefaultStats[i];
            }

            // clear any status effects
            foreach (PokemonInstance pokemon in playerParty)
            {
                pokemon.StatusEffects.Clear();
            }
        }
    
        // generate a PokemonInstance given a specific pokemon, level, and movelist
        public static PokemonInstance GeneratePokemonInstance(Pokemon pokemon, List<Move> moves, int level)
        {
            // create a Random rnd
            Random rnd = new Random();

            // create list that will store calculated stats
            List<int> stats = new List<int>();

            // TODO: IV things
            List<int> ivs = new List<int> { rnd.Next(1, 11), rnd.Next(1, 11), rnd.Next(1, 11), rnd.Next(1, 11), rnd.Next(1, 11), rnd.Next(1, 11) };

            // determine and store HP based on BaseStats and level
            stats.Add(Convert.ToInt32(Math.Ceiling((((pokemon.BaseStats[0] + ivs[0]) * 2 + 2.5) * level) / 100 + level + 10)));

            // determine other pokemon stats based on BaseStats and level
            for (int i = 1; i < 6; i++)
            {
                stats.Add(Convert.ToInt32(Math.Ceiling((((pokemon.BaseStats[i] + ivs[i]) * 2 + 2.5) * level) / 100 + 5)));
            }

            // create and return PokemonInstance using info
            PokemonInstance pokemonInstance = new PokemonInstance(pokemon, level, ivs, stats, moves);
            return pokemonInstance;
        }
        
        // display player pokemon and enemy pokemon info
        public static void DisplayStats(PokemonInstance player, PokemonInstance opponent, int playerPokemonMaxHP, int opponentPokemonMaxHP)
        {
            // display player pokemon and enemy pokemon info
            Console.Clear();
            Console.WriteLine("\n");
            Console.WriteLine($"\t\t\t\t\t{opponent.PokemonValue.PokemonName} - Level {opponent.Level}");
            Console.WriteLine($"\t\t\t\t\tHP: {opponent.Stats[0]} / {opponentPokemonMaxHP}");
            Console.WriteLine($"\t\t\t\t\t{opponent.Stats[1]} {opponent.Stats[2]} {opponent.Stats[3]} {opponent.Stats[4]} {opponent.Stats[5]} {opponent.Stats[6]} {opponent.Stats[7]}");
            Console.Write($"\t\t\t\t\t");
            foreach (string status in opponent.StatusEffects)
            {
                Console.Write($"{status.ToUpper()} ");
            }

            Console.WriteLine("");
            Console.WriteLine($"\t{player.PokemonValue.PokemonName} - Level {player.Level}");
            Console.WriteLine($"\tHP: {player.Stats[0]} / {playerPokemonMaxHP}");
            Console.WriteLine($"\t{player.Stats[1]} {player.Stats[2]} {player.Stats[3]} {player.Stats[4]} {player.Stats[5]} {player.Stats[6]} {player.Stats[7]}");
            Console.Write($"\t");
            foreach (string status in player.StatusEffects)
            {
                Console.Write($"{status.ToUpper()} ");
            }
            Console.WriteLine("\n");
        }

        // display player pokemon's moves
        public static void DisplayMoves(PokemonInstance player)
        {
            // display player pokemon's moves
            foreach (Move move in player.Moves)
            {
                Console.WriteLine($"{move.MoveName} - {move.MoveDescription}");
                Console.WriteLine($"Type: {move.MoveType.TypeName}, Power: {move.Power}, Accuracy: {move.Accuracy}, PP: {move.PP}");
                Console.WriteLine("");
            }
        }
        
        // carry out the rest of the turn
        public static void DoMoves(List<PokemonInstance> playerParty, int playerActivePokemon, List<PokemonInstance> opponentParty, int opponentActivePokemon, List<List<int>> playerPartyDefaultStats, List<List<int>> opponentPartyDefaultStats, Move playerMove, Move opponentMove)
        {
            // recalculate stats before each move using StatModifiers
            playerParty[playerActivePokemon].Stats = CalculateStats(playerParty[playerActivePokemon], playerPartyDefaultStats[playerActivePokemon]);
            opponentParty[opponentActivePokemon].Stats = CalculateStats(opponentParty[opponentActivePokemon], opponentPartyDefaultStats[opponentActivePokemon]);

            // determine which move goes first
            OrderMoves(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerMove, opponentMove, playerPartyDefaultStats[playerActivePokemon], opponentPartyDefaultStats[opponentActivePokemon], out PokemonInstance firstPokemon, out PokemonInstance secondPokemon, out Move firstMove, out Move secondMove, out List<int> firstPokemonDefaultStats, out List<int> secondPokemonDefaultStats);
            
            // FIRST MOVE:
                // TODO: move effects
            
            // create empty lists that will be filled with move effects
            List<string> firstEffects = new List<string>();

            // check for move tags
                // check if condition is met
            foreach (KeyValuePair<string, string> tag in firstMove.Tags)
            {
                // if condition is numerical, check if it will occur
                if (Int32.TryParse(tag.Value, out int chance) && new Random().Next(1, 101) <= chance)
                {
                    // if it occurs, save it to the list of effects
                    firstEffects.Add(tag.Key);
                }
                // TODO: other conditions
            }

            // by default, the move doesn't miss
            bool miss = false;
            
            // by default, accuracy move accuracy is 100
            int moveAccuracy = 100;
            
            // check if the move is missable, and if so, store move accuracy in int accuracy
            bool isMissable = Int32.TryParse(firstMove.Accuracy, out moveAccuracy);

            // calculate final accuracy given pokemon stats
            double accuracy = Convert.ToDouble(firstPokemon.Stats[6]) / 100.0;
            double evasion = Convert.ToDouble(secondPokemon.Stats[7]) / 100.0;
            int finalAccuracy = Convert.ToInt32(Math.Round(moveAccuracy * accuracy / evasion));

            if (finalAccuracy > 100)
            {
                finalAccuracy = 100;
            }

            // determine whether attack misses
            if (new Random().Next(1, 101) > finalAccuracy)
            {
                miss = true;
            }

            // display that the move has been performed
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"{firstPokemon.PokemonValue.PokemonName} used {firstMove.MoveName}.");

            // display move missing
            if (miss)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine($"{firstPokemon.PokemonValue.PokemonName} missed.");
            }

            // finish move if it doesn't miss
            if (!miss)
            {
                // calculate first move damage
                    // TODO: atypical attack damage
                int firstMoveDamage = CalculateDamage(firstPokemon, secondPokemon, firstMove, out double firstEffectiveness);
                    if (firstMoveDamage > secondPokemon.Stats[0])
                    {
                        firstMoveDamage = secondPokemon.Stats[0];
                    }

                // change pokemon HP
                secondPokemon.Stats[0] -= firstMoveDamage;

                // display amount of damage done
                if (firstMoveDamage != 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"{secondPokemon.PokemonValue.PokemonName} took {firstMoveDamage} damage.");
                }

                // TODO: enact move effect
                DoEffect(firstPokemon, secondPokemon, firstEffects, firstMoveDamage, firstPokemonDefaultStats);

                // display first move effectiveness
                DisplayEffectiveness(firstEffectiveness, firstMoveDamage);
            }

            // recalculate stats before each move using StatModifiers
            playerParty[playerActivePokemon].Stats = CalculateStats(playerParty[playerActivePokemon], playerPartyDefaultStats[playerActivePokemon]);
            opponentParty[opponentActivePokemon].Stats = CalculateStats(opponentParty[opponentActivePokemon], opponentPartyDefaultStats[opponentActivePokemon]);

            // reload stats with updated stats
            System.Threading.Thread.Sleep(1000);
            DisplayStats(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerPartyDefaultStats[playerActivePokemon][0], opponentPartyDefaultStats[opponentActivePokemon][0]);

            // if a pokemon has fainted, the turn is over
            if (firstPokemon.Stats[0] <= 0 || secondPokemon.Stats[0] <= 0)
            {
                return;
            }

            // SECOND MOVE:
                // TODO: move effects

            // create empty lists that will be filled with move effects
            List<string> secondEffects = new List<string>();

            // check for move tags
                // check if condition is met
            foreach (KeyValuePair<string, string> tag in secondMove.Tags)
            {
                // if condition is numerical, check if it will occur
                if (Int32.TryParse(tag.Value, out int chance) && new Random().Next(1, 101) <= chance)
                {
                    // if it occurs, save it to the list of effects
                    secondEffects.Add(tag.Key);
                }
                // TODO: other conditions
            }

            // by default, the move doesn't miss
            miss = false;
            
            // by default, accuracy move accuracy is 100
            moveAccuracy = 100;
            
            // check if the move is missable, and if so, store move accuracy in int accuracy
            isMissable = Int32.TryParse(secondMove.Accuracy, out moveAccuracy);

            // calculate final accuracy given pokemon stats
            accuracy = Convert.ToDouble(secondPokemon.Stats[6]) / 100.0;
            evasion = Convert.ToDouble(firstPokemon.Stats[7]) / 100.0;
            finalAccuracy = Convert.ToInt32(Math.Round(moveAccuracy * accuracy / evasion));

            if (finalAccuracy > 100)
            {
                finalAccuracy = 100;
            }

            // determine whether attack misses
            if (new Random().Next(1, 101) > finalAccuracy)
            {
                miss = true;
            }

            // display that the move has been performed
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"{secondPokemon.PokemonValue.PokemonName} used {secondMove.MoveName}.");

            // display move missing
            if (miss)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine($"{secondPokemon.PokemonValue.PokemonName} missed.");
            }

            // finish move if it doesn't miss
            if (!miss)
            {
                // calculate second move damage
                    // TODO: atypical attack damage
                int secondMoveDamage = CalculateDamage(secondPokemon, firstPokemon, secondMove, out double secondEffectiveness);
                if (secondMoveDamage > firstPokemon.Stats[0])
                    {
                        secondMoveDamage = firstPokemon.Stats[0];
                    }

                // change pokemon HP
                firstPokemon.Stats[0] -= secondMoveDamage;

                // display amount of damage done
                if (secondMoveDamage != 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"{firstPokemon.PokemonValue.PokemonName} took {secondMoveDamage} damage.");
                }

                // TODO: enact move effect
                DoEffect(secondPokemon, firstPokemon, secondEffects, secondMoveDamage, secondPokemonDefaultStats);

                // display second move effectiveness
                DisplayEffectiveness(secondEffectiveness, secondMoveDamage);
            }

            // recalculate stats before each move using StatModifiers
            playerParty[playerActivePokemon].Stats = CalculateStats(playerParty[playerActivePokemon], playerPartyDefaultStats[playerActivePokemon]);
            opponentParty[opponentActivePokemon].Stats = CalculateStats(opponentParty[opponentActivePokemon], opponentPartyDefaultStats[opponentActivePokemon]);

            // display stats with updated stats
            System.Threading.Thread.Sleep(1000);
            DisplayStats(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerPartyDefaultStats[playerActivePokemon][0], opponentPartyDefaultStats[opponentActivePokemon][0]);

            // if a pokemon has fainted, the turn is over
            if (firstPokemon.Stats[0] <= 0 || secondPokemon.Stats[0] <= 0)
            {
                return;
            }

            // do poison status effect for opponent
            foreach (string status in opponentParty[opponentActivePokemon].StatusEffects)
            {
                if (status == "poison")
                {
                    int damage = opponentPartyDefaultStats[opponentActivePokemon][0] / 16;
                    opponentParty[opponentActivePokemon].Stats[0] -= damage;
                    if (opponentParty[opponentActivePokemon].Stats[0] < 0)
                    {
                        opponentParty[opponentActivePokemon].Stats[0] = 0;
                    }
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"{opponentParty[opponentActivePokemon].PokemonValue.PokemonName} was hurt by poison.");
                    System.Threading.Thread.Sleep(1000);
                    DisplayStats(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerPartyDefaultStats[playerActivePokemon][0], opponentPartyDefaultStats[opponentActivePokemon][0]);
                }
            }

            // do poison status effect for player
            foreach (string status in playerParty[playerActivePokemon].StatusEffects)
            {
                if (status == "poison")
                {
                    int damage = playerPartyDefaultStats[playerActivePokemon][0] / 16;
                    playerParty[playerActivePokemon].Stats[0] -= damage;
                    if (playerParty[playerActivePokemon].Stats[0] < 0)
                    {
                        playerParty[playerActivePokemon].Stats[0] = 0;
                    }
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"{playerParty[playerActivePokemon].PokemonValue.PokemonName} was hurt by poison.");
                    System.Threading.Thread.Sleep(1000);
                    DisplayStats(playerParty[playerActivePokemon], opponentParty[opponentActivePokemon], playerPartyDefaultStats[playerActivePokemon][0], opponentPartyDefaultStats[opponentActivePokemon][0]);
                }
            }
        }

        // display a move's effectiveness
        public static void DisplayEffectiveness(double effectiveness, int damage)
        {
            if (effectiveness > 1.0 && damage != 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Super effective!");
                }
                if (effectiveness < 1.0 && damage != 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Not very effective.");
                }
                if (effectiveness == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"No effect.");
                }
        }

        // calculate how much damage a move will typically do
        public static int CalculateDamage(PokemonInstance attacker, PokemonInstance defender, Move attackerMove, out double effectiveness)
        {
            // take effectiveness into account
            effectiveness = 1.0;

            // create an empty List<string> to be filled by types
            List<string> defenderTypes = new List<string>();

            // fill list with types of the pokemon getting attacked
            for (int i = 0; i < defender.PokemonValue.PokemonTypes.Count; i++)
            {
                string currentType = defender.PokemonValue.PokemonTypes[i].TypeName;
                defenderTypes.Add(currentType);
            }
            
            // search for a match between pokemon type and attack type effectiveness
            foreach (string type in defenderTypes)
            {
                // multiply effectiveness by 2 if matches a super effective type
                if (attackerMove.MoveType.Strengths.Contains(type))
                {
                    effectiveness *= 2;
                }

                // multiply effectiveness by 0.5 if matches a not very effective type
                if (attackerMove.MoveType.Weaknesses.Contains(type))
                {
                    effectiveness *= 0.5;
                }

                // multiply effectiveness by 0 if matches a noEffect type
                if (attackerMove.MoveType.NoEffects.Contains(type))
                {
                    effectiveness *= 0.0;
                }
            }

            // return 0 if power is atypical
            if (attackerMove.Power == "--")
            {
                return 0;
            }

            // create a damage variable
            double damage = 0;

            // determine attack / defense values based on attackType
            double attack = Convert.ToDouble(attacker.Stats[1]);
            double defense = Convert.ToDouble(defender.Stats[2]);
            if (attackerMove.AttackType == "special")
            {
                attack = Convert.ToDouble(attacker.Stats[3]);
                defense = Convert.ToDouble(defender.Stats[4]);
            }

            // convert Power to a double
            double power = Convert.ToDouble(Convert.ToInt32(attackerMove.Power));

            // create Random
            Random rnd = new Random();

            // TODO: weather
            double weather = 1.0;

            // TODO: critical

            // create a random component to the modifier
            double random = Convert.ToDouble(rnd.Next(85, 101)) / 100;

            // create a bonus if the attack type matches the attacker's type
            double STAB = 1.0;
            if (attacker.PokemonValue.PokemonTypes.Contains(attackerMove.MoveType))
            {
                STAB = 1.5;
            }

            // calculate modifier
            double modifier = 1.0 * weather * random * STAB * effectiveness;

            // calculate and return damage based on info
            damage = ((((((2 * attacker.Level) + 2) / 5) * power * (attack / defense)) / 50) + 2) * modifier;
            return Convert.ToInt32(Math.Round(damage));
        }
    
        // check the order that moves go in
        public static void OrderMoves(PokemonInstance player, PokemonInstance opponent, Move playerMove, Move opponentMove, List<int> playerDefaultStats, List<int> opponentDefaultStats, out PokemonInstance firstPokemon, out PokemonInstance secondPokemon, out Move firstMove, out Move secondMove, out List<int> firstPokemonDefaultStats, out List<int> secondPokemonDefaultStats)
        {
            // TODO: exceptions (quick attack, etc.)
            if (player.Stats[5] < opponent.Stats[5])
            {
                firstPokemon = opponent;
                secondPokemon = player;
                firstMove = opponentMove;
                secondMove = playerMove;
                firstPokemonDefaultStats = opponentDefaultStats;
                secondPokemonDefaultStats = playerDefaultStats;
            }
            else
            {
                firstPokemon = player;
                secondPokemon = opponent;
                firstMove = playerMove;
                secondMove = opponentMove;
                firstPokemonDefaultStats = playerDefaultStats;
                secondPokemonDefaultStats = opponentDefaultStats;
            }
        }
    
        // check for move tags
        public static void DoEffect(PokemonInstance attacker, PokemonInstance defender, List<string> effects, int damage, List<int> attackerDefaultStats)
        {
            // stealhalfdamage
            foreach (string effect in effects)
            {
                if (effect == "stealhalfdamage")
                {
                    int heal = damage / 2;
                    if (heal > attackerDefaultStats[0] - attacker.Stats[0])
                    {
                        heal = attackerDefaultStats[0] - attacker.Stats[0];
                    }
                    attacker.Stats[0] += heal;
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"{attacker.PokemonValue.PokemonName} stole some of {defender.PokemonValue.PokemonName}'s HP.");
                }
            }

            // opponentpoison
            foreach (string effect in effects)
            {
                if (effect == "opponentpoison")
                {
                    // check if opponent is already poisoned
                    bool alreadyPoisoned = false;
                    foreach (string status in defender.StatusEffects)
                    {
                        if (status == "poison")
                        alreadyPoisoned = true;
                    }
                    if (alreadyPoisoned)
                    {
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine($"{defender.PokemonValue.PokemonName} is already poisoned.");
                    }
                    else
                    {
                        defender.StatusEffects.Add("poison");
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine($"{defender.PokemonValue.PokemonName} was poisoned.");
                    }
                }
            }

            // opponentaccuracy-
            foreach (string effect in effects)
            {
                if (effect == "opponentaccuracy-")
                {
                    if (defender.StatStages[6] > -6)
                    {
                        defender.StatStages[6] -= 1;
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine($"{defender.PokemonValue.PokemonName}'s accuracy fell!");
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine($"{defender.PokemonValue.PokemonName}'s accuracy won't go any lower!");
                    }
                }
            }

            // selfrecharge
            foreach (string effect in effects)
            {
                if (effect == "selfrecharge")
                {
                    attacker.StatusEffects.Add("recharge");
                }
            }
            
            // TODO: Other tags
        }
    
        public static List<int> CalculateStats(PokemonInstance pokemon, List<int> pokemonDefaultStats)
        {
            // create list populated only with health stat to be filled later
            List<int> calculatedStats = new List<int> { pokemon.Stats[0] };

            double multiplier;

            // add calculated attack, defense, spattack, spdefense, and, speed
            for (int i = 1; i < 6; i++)
            {
                // determine stat multiplier based on stat stage
                // deal with stages below 0
                if (pokemon.StatStages[i] < 0)
                {
                    multiplier = 2.0 / (Convert.ToDouble(Math.Abs(pokemon.StatStages[i]) + 2.0));
                }
                
                // deal with stages 0 and above
                else
                {
                    multiplier = Convert.ToDouble(pokemon.StatStages[i] + 2.0) / 2.0;
                }

                // add calculated stat to list
                calculatedStats.Add(Convert.ToInt32(pokemonDefaultStats[i] * multiplier));
            }

            // add calculated accuracy and evasion
            for (int i = 6; i < 8; i++)
            {
                // determine stat multiplier based on stat stage
                // deal with stages below 0
                if (pokemon.StatStages[i] < 0)
                {
                    multiplier = 3.0 / (Convert.ToDouble(Math.Abs(pokemon.StatStages[i]) + 3.0));
                }

                // deal with stages 0 and above
                else
                {
                    multiplier = Convert.ToDouble(pokemon.StatStages[i] + 3.0) / 3.0;
                }

                // add calculated stat to list
                calculatedStats.Add(Convert.ToInt32(pokemonDefaultStats[i] * multiplier));
            }
            return calculatedStats;
        }
    }
}
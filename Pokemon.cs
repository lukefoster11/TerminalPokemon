using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    // Type class, where each one has a name, strengths, weaknesses, and noEffects
    public class Type
    {
        public string TypeName
        { get; set; }
        public List<string> Strengths
        { get; set; }
        public List<string> Weaknesses
        { get; set; }
        public List<string> NoEffects
        { get; set; }

        // establish Type constructor, requiring a name, strength, weakness, and noEffect
        private Type(string name, List<string> strengths, List<string> weaknesses, List<string> noEffects)
        {
            this.TypeName = name;
            this.Strengths = strengths;
            this.Weaknesses = weaknesses;
            this.NoEffects = noEffects;
        }

        // go through types.dex, saving the name, strengths, weaknesses, and noeffects for each type
        public static List<Type> Initialize()
        {
            // create List<Type> that gets filled as each Type is read
            List<Type> typeList = new List<Type>();

            // types.txt format: Type;#ofStrengths:strength,strength,;#ofWeaknesses:weakness,weakness,;#ofNoEffects:noEffect,
            string line;
            StreamReader file = new StreamReader("gamedata/types.dex");
            while((line = file.ReadLine()) != null)
            {
                // find where the first semicolon is and save to int breakIndex
                int breakIndex = line.IndexOf(";");

                //  save the name of the type (line from start to breakIndex) to typeName
                string typeName = line.Substring(0, breakIndex);

                // find the number of strengths the type has and save to int strengthNumber
                line = line.Substring(breakIndex+1);
                string tempStrengthNumber = Convert.ToString(line[0]);
                int strengthNumber = Convert.ToInt32(tempStrengthNumber);

                // move the line along 2 spaces to start at the beginning of the strengths
                line = line.Substring(2);

                // loop through and save the strengths of each type to a list strengthList, using the int strengthNumber
                List<string> strengthList = new List<string>();
                for (int i = 0; i < strengthNumber; i++)
                {
                    if (strengthNumber > 0)
                    {
                        // find the index of the comma (where the strength ends)
                        breakIndex = line.IndexOf(",");

                        // save the strength (line from start to comma index) to string tempStrength
                        string tempStrength = line.Substring(0, breakIndex);

                        // save the string to the strengthList
                        strengthList.Add(tempStrength);

                        // move the line along to the next strength
                        line = line.Substring(breakIndex+1);
                    }
                }
                
                // find the number of weaknesses the type has and save to int weaknessNumber
                string tempWeaknessNumber = Convert.ToString(line[1]);
                int weaknessNumber = Convert.ToInt32(tempWeaknessNumber);

                // move the line along 3 places to start at the beginning of weaknesses
                line = line.Substring(3);

                // loop through and save the weaknesses of each type to a list weaknessList, using the int weaknessNumber
                List<string> weaknessList = new List<string>();
                for (int i = 0; i < weaknessNumber; i++)
                {
                    if (weaknessNumber > 0)
                    {
                        // find the index of the comma (where the weakness ends)
                        breakIndex = line.IndexOf(",");

                        // save the weakness (line from start to comma index) to string tempWeakness
                        string tempWeakness = line.Substring(0, breakIndex);

                        // save the string to the weaknessList
                        weaknessList.Add(tempWeakness);

                        // move the line along to the next weakness
                        line = line.Substring(breakIndex+1);
                    }
                }

                // find the number of noEffects the type has and save to int noEffectNumber
                string tempNoEffectNumber = Convert.ToString(line[1]);
                int noEffectNumber = Convert.ToInt32(tempNoEffectNumber);

                // move the line along 3 places to start at the beginning of noEffects
                line = line.Substring(3);

                // loop through and save the noEffects of each type to a list noEffectList, using the int noEffectNumber
                List<string> noEffectList = new List<string>();
                for (int i = 0; i < noEffectNumber; i++)
                {
                    if (noEffectNumber > 0)
                    {
                        // find the index of the comma (where the noEffect ends)
                        breakIndex = line.IndexOf(",");

                        // save the noEffect (line from start to comma index) to string tempNoEffect
                        string tempNoEffect = line.Substring(0, breakIndex);

                        // save the string to the tempNoEffect list
                        noEffectList.Add(tempNoEffect);
                    }
                }

                // store all type data into Type currentType that gets added to typeList
                Type currentType = new Type(typeName, strengthList, weaknessList, noEffectList);
                typeList.Add(currentType);

                // display on console that the type has been initialized (just for fun)
                Console.WriteLine($"{typeName} type initialized....");
            }

            // close types.dex
            file.Close();

            // display on console that all types have been initialized (just for fun)
            Console.WriteLine("** All types initialized **");

            return typeList;
        }
    }

    // Pokemon class, where each pokemon has a specific name, description, evolution requirement, types, and stats
    public class Pokemon
    {
        public int PokemonID
        { get; set; }
        public string PokemonName
        { get; set; }
        public string PokemonDescription
        { get; set; }

        // "item" if by item, "level" if by level, "none" if doesn't evolve
        public string EvolutionMethod
        { get; set; }

        // itemName if by item, level if by level, "none" if doesn't evolve
        public string EvolutionSpecifics
        { get; set; }
        public List<Type> PokemonTypes
        { get; set; }

        // HP, Attack, Defense, Special Attack, Special Defense, Speed
        public List<int> BaseStats
        { get; set; }
        public List<Move> MoveSet
        { get; set; }

        // establish Pokemon constructor, requiring a name, id, description, evolutionMethod, evolutionSpecifics, types, stats, and moveSet
        private Pokemon(int id, string name, string description, string evolutionMethod, string evolutionSpecifics, List<Type> types, List<int> stats, List<Move> moveSet)
        {
            this.PokemonID = id;
            this.PokemonName = name;
            this.PokemonDescription = description;
            this.EvolutionMethod = evolutionMethod;
            this.EvolutionSpecifics = evolutionSpecifics;
            this.PokemonTypes = types;
            this.BaseStats = stats;
            this.MoveSet = moveSet;
        }

        // go through pokedex.dex, saving the id, name, description, evolution info, types, stats, and moveSet for each pokemon
        public static List<Pokemon> Initialize()
        {
            // create pokemonList that gets filled as pokedex.dex is read
            List<Pokemon> pokemonList = new List<Pokemon>();

            // create empty tempStats = List<int> instance to store as Stats
            List<int> tempStats = new List<int>();

            // create empty tempMoveSet = List<Move> instance to store as MoveSet
            List<Move> tempMoveSet = new List<Move>();

            //pokedex.txt format: id;name;description;evolutionMethod:specifics;#ofTypes:type,(type,)
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("gamedata/pokedex.dex");
            while((line = file.ReadLine()) != null)
            {

                // find where the end of int pokemonID is (index of first ";")
                int breakIndex = line.IndexOf(";");

                // get PokemonID and store in int pokemonID
                string tempPokemonID = line.Substring(0, breakIndex);
                int pokemonID = Convert.ToInt32(tempPokemonID);

                // move file along to start of PokemonName
                line = line.Substring(breakIndex + 1);

                // find where the end of string pokemonName is (index of next ";")
                breakIndex = line.IndexOf(";");

                // get PokemonName and store in string pokemonName
                string pokemonName = line.Substring(0, breakIndex);

                // move file along to start of PokemonDescription
                line = line.Substring(breakIndex + 1);

                // find where the end of string pokemonDescription is (index of next ";")
                breakIndex = line.IndexOf(";");

                // get PokemonDescription and store in string pokemonDescription
                string pokemonDescription = line.Substring(0, breakIndex);

                // move file along to start of EvolutionMethod
                line = line.Substring(breakIndex + 1);

                // find where the end of string evolutionMethod is (index of ":")
                breakIndex = line.IndexOf(":");

                // get EvolutionMethod and store in string evolutionMethod
                string evolutionMethod = line.Substring(0, breakIndex);

                // move file along to start of EvolutionSpecifics
                line = line.Substring(breakIndex + 1);

                // find where the end of string evolutionSpecifics is (index of ";")
                breakIndex = line.IndexOf(";");

                // get EvolutionSpecifics and store in string evolutionSpecifics
                string evolutionSpecifics = line.Substring(0, breakIndex);

                // move file along to start of type information
                line = line.Substring(breakIndex + 1);

                // get number of types and store in int numberOfTypes
                int numberOfTypes = Convert.ToInt32(Convert.ToString(line[0]));

                // move file along one position to beginning of first type
                line = line.Substring(2);

                // create empty typeList that will be filled later
                List<Type> typeList = new List<Type>();

                // add each type to typeList
                    // loop through types, using numberOfTypes to determine how many times to loop through
                for (int i = 0; i < numberOfTypes; i++)
                {
                    // find where the end of the first type is (index of ",")
                    breakIndex = line.IndexOf(",");

                    // get typeName and store in string typeName
                    string typeName = line.Substring(0, breakIndex);

                    // search through typeDex for a type that has a name match and store in Type pokemonType
                    Type pokemonType = Dex.TypeDex.Find(x => x.TypeName.Contains(typeName));

                    // add pokemonType to typeList
                    typeList.Add(pokemonType);

                    // move file along to the beginning of the next type
                    line = line.Substring(breakIndex + 1);
                }

                // store all pokemon data into Pokemon currentPokemon that gets added to pokemonList
                Pokemon currentPokemon = new Pokemon(pokemonID, pokemonName, pokemonDescription, evolutionMethod, evolutionSpecifics, typeList, tempStats, tempMoveSet);
                pokemonList.Add(currentPokemon);
            }

            // close pokedex.dex
            file.Close();

            // open basestats.csv and save stats to List<int> stats
            var path = @"gamedata/basestats.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                // data is broken up by ","s
                csvParser.SetDelimiters(new string[] { "," });

                // set up a counter to go through each row(pokemon)
                int counter = 0;

                while (!csvParser.EndOfData)
                {
                    // read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    // save the info from each position to an int
                    int HP = Convert.ToInt32(Convert.ToString(fields[0]));
                    int Attack = Convert.ToInt32(Convert.ToString(fields[1]));
                    int Defense = Convert.ToInt32(Convert.ToString(fields[2]));
                    int SpAttack = Convert.ToInt32(Convert.ToString(fields[3]));
                    int SpDefense = Convert.ToInt32(Convert.ToString(fields[4]));
                    int Speed = Convert.ToInt32(Convert.ToString(fields[5]));

                    // create List<int> stats that gets filled with stats for each pokemon
                    List<int> stats = new List<int> { HP, Attack, Defense, SpAttack, SpDefense, Speed };

                    // store stats into the Stats field for specific pokemon (determined by counter)
                    pokemonList[counter].BaseStats = stats;

                    // increase the counter by 1 to move onto the next pokemon
                    counter++;
                }
            }

            // open movesets.csv and save movesets to List<Move> moveSet
            path = @"gamedata/movesets.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                // data is broken up by ","s
                csvParser.SetDelimiters(new string[] { "," });

                // set up a counter to go through each pokemon
                int counter = 0;

                while(!csvParser.EndOfData)
                {
                    // read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    // create a new List<Move> moveSet to store all moves into
                    List<Move> MoveSet = new List<Move>();

                    // loop through each move on the current row
                    foreach (string move in fields)
                    {
                        // find a move within moveDex that has the same name as the pokemon's move and store in Move currentMove
                        Move currentMove = Dex.MoveDex.Find(x => x.MoveName.Contains(move));

                        // add current move to the pokemon's MoveSet
                        MoveSet.Add(currentMove);
                    }

                    // store MoveSet into the MoveSet field for specific pokemon (determined by counter)
                    pokemonList[counter].MoveSet = MoveSet;

                    // display on console that the specific pokemon has been initialized (just for fun)
                    Console.WriteLine($"{pokemonList[counter].PokemonName} initialized....");

                    // increase the counter by 1
                    counter++;
                }
            }

            // display on console that all pokemon have been initialized (just for fun)
            Console.WriteLine("** All Pokemon initialized **");

            return pokemonList;
        }
    }

    // Move class, where each move has a specific name, type, power, accuracy, pp, description, attackType, and tags
    public class Move
    {
        public string MoveName
        { get; set; }
        public Type MoveType
        { get; set; }
        public string Power
        { get; set; }
        public int PP
        { get; set; }
        public string Accuracy
        { get; set; }
        public string MoveDescription
        { get; set; }
        public string AttackType
        { get; set; }
        public Dictionary<string, string> Tags
        { get; set; }

        // establish a Move constructor
        private Move(string name, Type type, string power, int PP, string accuracy, string description, string attackType)
        {
            this.MoveName = name;
            this.MoveType = type;
            this.Power = power;
            this.PP = PP;
            this.Accuracy = accuracy;
            this.MoveDescription = description;
            this.AttackType = attackType;
        }

        // create and return a List<Move> containing every move from the database
        public static List<Move> Initialize()
        {
            // create a new empty List<Move> that will be filled with moves
            List<Move> moveList = new List<Move>();

            // open movedex.csv, going through each line and saving the data to a new move
                // format: moveName,type,PP,attack,accuracy,description
            var path = @"gamedata/movedex.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                // data is broken up by ","s
                csvParser.SetDelimiters(new string[] { "," });

                // loop through each line of movedex.csv
                while (!csvParser.EndOfData)
                {
                    // read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    // save the info from each position into variables
                    string moveName = fields[0];
                    Type moveType = Dex.TypeDex.Find(x => x.TypeName.Contains(fields[1]));
                    int movePP = Convert.ToInt32(Convert.ToString(fields[2]));
                    string movePower = fields[3];
                    string moveAccuracy = fields[4];
                    string MoveDescription = fields[5];

                    // save attackType (physical or special) for each move
                    string attackType = "physical";
                    if (moveType.TypeName == "Fire" || moveType.TypeName == "Water" || moveType.TypeName == "Grass" || moveType.TypeName == "Electric" || moveType.TypeName == "Psychic" || moveType.TypeName == "Ice" || moveType.TypeName == "Dragon" || moveType.TypeName == "Dark")
                    {
                        attackType = "special";
                    }

                    // create a new Move using the info from movedex.csv
                    Move currentMove = new Move(moveName, moveType, movePower, movePP, moveAccuracy, MoveDescription, attackType);

                    // add currentMove to moveList
                    moveList.Add(currentMove);

                    // display on console that move has been initialized (just for fun)
                    Console.WriteLine($"{currentMove.MoveName} initialized....");
                }
            }
            
            // call AddTags method to add tags to specific moves
            AddTags(moveList);
            
            // disply on console that all moves have been initialized (just for fun)
            Console.WriteLine("** All moves initialized **");

            return moveList;
        }

        // add tags to moves (can be "selfattack+", "opponentdefense--", "selfconfuse", etc.)
        public static void AddTags(List<Move> moveList)
        {
            // go through each move, adding tags
            foreach (Move move in moveList)
            {
                // create an empty dictionary for tags to be added into
                move.Tags = new Dictionary<string, string>();
            }
            // hard code in tags and conditions for each move (not ideal)
                // TODO: finish hard coding
            {
            moveList[0].Tags.Add("movepower*2", "successive");
            moveList[0].Tags.Add("movepowerreset", "nonsuccessive");
            moveList[1].Tags.Add("stealhalfdamage", "100");
            moveList[3].Tags.Add("repeat2-5", "100");
            moveList[4].Tags.Add("opponentconfuse", "10");
            moveList[5].Tags.Add("selfattack+, selfdefense+, selfspattack+, selfspdefense+, selfspeed+", "10");
            moveList[6].Tags.Add("opponentstuck", "100");
            moveList[7].Tags.Add("opponentspeed-", "100");
            moveList[8].Tags.Add("selfspattack++", "100");
            moveList[9].Tags.Add("repeat2", "100");
            moveList[10].Tags.Add("opponentflinch", "30");
            moveList[11].Tags.Add("opponentspdefense-", "20");
            moveList[13].Tags.Add("opponentspdefense--", "100");
            moveList[14].Tags.Add("opponentconfuse", "100");
            moveList[14].Tags.Add("opponentspattack+", "100");
            moveList[15].Tags.Add("opponentnoitem", "100");
            moveList[16].Tags.Add("selffaint", "100");
            moveList[16].Tags.Add("opponentattack--", "100");
            moveList[16].Tags.Add("opponentspattack--", "100");
            moveList[17].Tags.Add("movepower*2", "opponentswitch");
            moveList[17].Tags.Add("firstturn", "opponentswitch");
            moveList[18].Tags.Add("stealtag", "100");
            moveList[19].Tags.Add("opponentonlyattack", "100");
            moveList[20].Tags.Add("stealitem", "100");
            moveList[21].Tags.Add("opponentnonsuccessive", "100");
            moveList[22].Tags.Add("damage*2", "nextmoveelectric");
            moveList[24].Tags.Add("opponentparalyze", "30");
            moveList[25].Tags.Add("opponentparalyze", "30");
            moveList[26].Tags.Add("opponentparalyze", "100");
            moveList[27].Tags.Add("opponentparalyze", "10");
            moveList[28].Tags.Add("opponentparalyze", "10");
            moveList[29].Tags.Add("opponentparalyze", "10");
            moveList[30].Tags.Add("selfthirddamage", "100");
            moveList[31].Tags.Add("opponentparalyze", "100");
            moveList[33].Tags.Add("selfattack+", "100");
            moveList[33].Tags.Add("selfspeed+", "100");
            moveList[34].Tags.Add("damage40", "100");
            moveList[36].Tags.Add("selfspattack--", "100");
            moveList[37].Tags.Add("successive2-3", "100");
            moveList[37].Tags.Add("selfconfuse", "100aftersuccessive");
            moveList[39].Tags.Add("repeat2-5", "100");
            moveList[40].Tags.Add("destroybarrier", "100");
            moveList[41].Tags.Add("selfattack+", "100");
            moveList[41].Tags.Add("selfdefense+", "100");
            moveList[42].Tags.Add("returndamage*2", "100");
            moveList[42].Tags.Add("goeslast", "100");
            moveList[43].Tags.Add("criticalchance*4", "100");
            moveList[44].Tags.Add("goesfirst", "100");
            moveList[44].Tags.Add("barrier", "100");
            moveList[44].Tags.Add("miss", "successive");
            moveList[45].Tags.Add("repeat2", "100");
            moveList[46].Tags.Add("opponentconfuse", "100");

            // giga drain
            moveList[104].Tags.Add("stealhalfdamage", "100");

            // sand-attack
            moveList[131].Tags.Add("opponentaccuracy-", "100");

            // poisonpowder
            moveList[289].Tags.Add("opponentpoison", "100");



            }

        }
    }

    // PokemonInstance class, where actual existing pokemon are stored
        // each one has a pokemonValue, level, stats, IVs, and a list of moves
    public class PokemonInstance
    {
        public Pokemon PokemonValue
        { get; set; }
        public int Level
        { get; set; }
        public List<int> IVs
        { get; set; }

        // HP, Attack, Defense, Special Attack, Special Defense, Speed
        public List<int> Stats
        { get; set; }
        public List<Move> Moves
        { get; set; }
        public List<string> StatusEffects
        { get; set; }

        // ex: { 0, 1, -2, 0, +3, -1, 1, 2 }
            // HP (not used), attack, defense, spattack, spdefense, speed, accuracy, evasion
        public List<int> StatStages
        { get; set; }

        // establish a PokemonInstance contructor
        public PokemonInstance(Pokemon pokemon, int level, List<int> ivs, List<int> stats, List<Move> moves)
        {
            this.PokemonValue = pokemon;
            this.Level = level;
            this.IVs = ivs;
            this.Stats = stats;
            this.Moves = moves;
        }
    }

    // Dex class, where important constant data are held
    public static class Dex
    {
        public static List<Type> TypeDex = Type.Initialize();
        public static List<Move> MoveDex = Move.Initialize();
        public static List<Pokemon> PokemonDex = Pokemon.Initialize();
    }
}
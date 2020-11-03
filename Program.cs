using System;
using System.Collections.Generic;

namespace Pokemon
{
    public class Program
    {
        static void Main(string[] args)
        {
            // run DoBattle method, feeding it 2 pokemon (a pre-made Venusaur and Pidgeot)
            PokemonInstance venusaur = Battle.GeneratePokemonInstance(Dex.PokemonDex[2], new List<Move> { Dex.MoveDex[119], Dex.MoveDex[199], Dex.MoveDex[104], Dex.MoveDex[289] }, 42);
            PokemonInstance pidgeot = Battle.GeneratePokemonInstance(Dex.PokemonDex[17], new List<Move> { Dex.MoveDex[131], Dex.MoveDex[88], Dex.MoveDex[83], Dex.MoveDex[167] }, 48);
            Battle.DoBattle(new List<PokemonInstance> {pidgeot}, new List<PokemonInstance> {venusaur});

            
            return;
        }
    }
}

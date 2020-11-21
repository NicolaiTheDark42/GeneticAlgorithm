using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm_Phrase
{

    public class Individual
    {
        // Definição dos cromossomos
        public char[] chromosome;

        // Definição do fitness
        public int fitness;

        public Individual(int chromosomeLength)
        {
            chromosome = new char[chromosomeLength];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random(); 

            List<Individual> population = new List<Individual>(); // Guarda a População de Individuals
            int populationSize = 2000; // Define o tamanho da população
            float mutatationRate = 0.001f; // Taxa de mutação
            int generation = 0; // Geração atual

            int bestFitness = 0; // Melhor fitness até agora
            string bestPhrase = ""; // Melhor frase até agora

            string targetPhrase = "Sou apenas um rapaz latino-americano"; // Frase que queremos que o algoritmo descubra

            population = GenerateFirstGeneration(populationSize, targetPhrase.Length, rand);
            
            while (bestFitness < targetPhrase.Length)
            {
                int populationBestScore = 0;

                // Gera o fitness para cada membro da população
                for (int i = 0; i < population.Count; i++)
                {
                    population[i].fitness = GetFitness(targetPhrase, population[i].chromosome);

                    if (population[i].fitness >= populationBestScore)
                    {
                        populationBestScore = population[i].fitness;
                        bestFitness = populationBestScore;
                        bestPhrase = new string(population[i].chromosome);
                    }
                }

                Console.WriteLine("Generation # {0} - Best Fitness {1}/{2}", generation, bestFitness, targetPhrase.Length);
                Console.WriteLine("Current Best phrase: {0}\n", bestPhrase);


                // Variável que vai guardar a nova população
                List<Individual> newPopulation = new List<Individual>();

                // Cria a nova população
                for (int i = 0; i < populationSize; i++)
                {
                    // Pega os pais
                    Individual[] parents = GetParents(population, rand);

                    // Cria um novo indivíduo
                    newPopulation.Add(GenerateNewIndividual(parents, rand, mutatationRate));
                }

                // Substitui a velha população com a nova
                population = newPopulation;

                generation++;
            }
        }


        // Retorna uma letra ou símbolo
        public static char GetCharacter(Random rand)
        {
            char[] charList = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'y', 'x', 'z', ' ', '.', ',', '-' };
            return charList[rand.Next(charList.Length)];
        }

        // Cria a primeira geração de indivíduos
        public static List<Individual> GenerateFirstGeneration(int populationSize, int targetPhraseSize, Random rand)
        {
            List<Individual> tempPopulation = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                tempPopulation.Add(new Individual(targetPhraseSize));

                for (int j = 0; j < targetPhraseSize; j++)
                {
                    tempPopulation[i].chromosome[j] = GetCharacter(rand);
                }
            }

            return tempPopulation;
        }

        // Calcula o fitness
        public static int GetFitness(string targetPhrase, char[] chromosome)
        {
            int score = 0;

            for (int i = 0; i < chromosome.Length; i++)
            {
                if (chromosome[i] == targetPhrase[i])
                {
                    score += 1;
                }
            }

            return score;
        }

        public static Individual[] GetParents(List<Individual> oldPopulation, Random rand)
        {
            Individual[] parents = new Individual[2];

            for (int i = 0; i < 2; i++)
            {
                int chosenIndex = 0;
                int currentCut = 0;
                int scoreSum = 0;

                for (int j = 0; j < oldPopulation.Count; j++)
                {
                    currentCut = rand.Next(scoreSum + oldPopulation[j].fitness);

                    if (currentCut > scoreSum)
                    {
                        chosenIndex = j;
                    }

                    scoreSum += oldPopulation[j].fitness;
                }

                parents[i] = oldPopulation[chosenIndex];
            }

            return parents;
        }

        public static Individual GenerateNewIndividual(Individual[] parents, Random rand, float mutationRate)
        {
            Individual child = new Individual(parents[0].chromosome.Length);

            int split = rand.Next(0, parents[0].chromosome.Length);

            for (int i = 0; i < parents[0].chromosome.Length; i++)
            {
                if (i <= split)
                {
                    child.chromosome[i] = parents[0].chromosome[i];
                }
                else
                {
                    child.chromosome[i] = parents[1].chromosome[i];
                }

                if (rand.NextDouble() < mutationRate)
                {
                    child.chromosome[i] = GetCharacter(rand);
                }

            }

            return child;
        }

    }
}

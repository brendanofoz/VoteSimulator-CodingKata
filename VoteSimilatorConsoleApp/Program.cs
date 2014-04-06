using System;
using ClassLibrary1;
using VoteSimulatorEngine;

namespace VoteSimilatorConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to Vladimur Putin's voting simulator!");
                Console.WriteLine();
                Console.WriteLine("Please type the statement to vote upon, then push Enter:");
                Console.WriteLine();

                int numberOfVoters;
                int numberOfPossibleResponses;
                var proposition = InputUserData(out numberOfVoters, out numberOfPossibleResponses);

                var votingSimulator = new VoteSimulator(new RandomNumberGenerator());
                var votingResult = votingSimulator.SimulateVote(proposition, numberOfVoters);

                OutputVotingResults(proposition, votingResult, numberOfPossibleResponses);

                Console.WriteLine("Hit Enter to continue");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error was encountered, details:");
                Console.WriteLine(e.Message);
            }            
        }

        private static Proposition InputUserData(out int numberOfVoters, out int numberOfPossibleResponses)
        {
            var proposition = new Proposition();
            string userInput = Console.ReadLine();
            proposition.Statement = userInput;

            Console.WriteLine("How many valid responses will there be?");
            userInput = Console.ReadLine();
            numberOfPossibleResponses = Int32.Parse(userInput);

            // request each of the valid responses for the vote
            proposition.ValidResponses = new string[numberOfPossibleResponses];
            Console.WriteLine(
                "Now enter the possible responses, pushing Enter after each one (push Enter on beginning of line to finish):");
            int currentResponse = 0;
            while (currentResponse < numberOfPossibleResponses)
            {
                Console.WriteLine(string.Format("Response {0}:", currentResponse + 1));
                userInput = Console.ReadLine();
                proposition.ValidResponses[currentResponse] = userInput;
                currentResponse++;
            }

            Console.WriteLine("How many voters are there?");
            userInput = Console.ReadLine();
            numberOfVoters = Int32.Parse(userInput);
            return proposition;
        }

        private static void OutputVotingResults(Proposition proposition, VoteResult votingResult, int numberOfPossibleResponses)
        {
            Console.WriteLine();
            Console.WriteLine(string.Format("Voting results for the proposition that {0}", proposition.Statement));
            int currentVote = 0;
            while (currentVote < numberOfPossibleResponses)
            {
                int numberOfVotes = votingResult.GetNumberOfVotesForResponse(currentVote);
                Console.WriteLine(string.Format("{0} votes for response '{1}': ", numberOfVotes,
                                                proposition.ValidResponses[currentVote]));

                currentVote++;
            }
        }
    }
}

using System;
using ClassLibrary1;

namespace VoteSimulatorEngine
{
    public class VoteSimulator
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public VoteSimulator(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public VoteResult SimulateVote(Proposition proposition, int populationSize)
        {
            ThrowIfVoteIsNotValid(proposition, populationSize);

            var result = new VoteResult(proposition);
            int maxResponseIndex = proposition.ValidResponses.Length;
            var currentVoter = 0;
            while (currentVoter < populationSize)
            {
                var randomResponseIndex = _randomNumberGenerator.GenerateRandomNumber(maxResponseIndex);
                result.AddVote(randomResponseIndex);

                currentVoter++;
            }

            return result;
        }

        private static void ThrowIfVoteIsNotValid(Proposition proposition, int populationSize)
        {
            if (proposition == null)
                throw new ArgumentNullException("proposition");

            if (String.IsNullOrEmpty(proposition.Statement))
                throw new ApplicationException("Proposition must have a statement");

            if (proposition.ValidResponses == null || proposition.ValidResponses.Length == 0)
                throw new ApplicationException("Proposition must have at least one valid response");

            if (populationSize == 0)
                throw new ApplicationException("Must be an least one voter in the population");
        }
    }
 }

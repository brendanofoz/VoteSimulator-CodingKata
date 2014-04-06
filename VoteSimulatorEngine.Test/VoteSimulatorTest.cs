using System;
using ClassLibrary1;
using NUnit.Framework;
using Rhino.Mocks;
using VoteSimulatorEngine;

namespace VoteSimilatorEngine.Test
{
    [TestFixture]
    public class VoteSimulatorTest
    {
        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void SimulateVote_NullProposition_Throws()
        {
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 0,
                                                                             alwaysGenerateValueOf: 0);
            simulator.SimulateVote(null, 0);
        }

        [Test]
        public void SimulateVote_PopulationOf1And2ValidResponsesAndVoteForSecondResponse_VoteResultIsExpected()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 1,
                                                                             alwaysGenerateValueOf: 1);
            int responseId = 0;
            var validResponses = CreateTwoValidResponses();
            var proposition = new Proposition {Statement = "valid", ValidResponses = validResponses};
            
            // act
            VoteResult voteResult = simulator.SimulateVote(proposition, 1);

            // assert
            Assert.That(voteResult.GetNumberOfVotesForResponse(responseId), Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof (ApplicationException), ExpectedMessage = "Proposition must have a statement")]
        public void SimulateVote_PropositionWithEmptyStatement_ThrowsExpectedException()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 0,
                                                                             alwaysGenerateValueOf: 0);
            var proposition = new Proposition {Statement = ""};

            // act/assert
            simulator.SimulateVote(proposition, 0);
        }

        [Test, ExpectedException(typeof (ApplicationException), ExpectedMessage = "Proposition must have a statement")]
        public void SimulateVote_PropositionWithNullStatement_ThrowsExpectedException()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 0,
                                                                             alwaysGenerateValueOf: 0);
            var proposition = new Proposition {Statement = null};

            // act/assert
            simulator.SimulateVote(proposition, 0);
        }

        [Test, ExpectedException(typeof (ApplicationException), ExpectedMessage = "Proposition must have at least one valid response")]
        public void SimulateVote_PropositionWithNullValidResponses_ThrowsExpectedException()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 1,
                                                                             alwaysGenerateValueOf: 0);
            var proposition = new Proposition {Statement = "valid", ValidResponses = null};

            // act/assert
            simulator.SimulateVote(proposition, 0);
        }


        [Test, ExpectedException(typeof(ApplicationException), ExpectedMessage = "Must be an least one voter in the population")]
        public void SimulateVote_1ValidResponseWithPopulationSize0_ThrowsExpectedException()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 1,
                                                                             alwaysGenerateValueOf: 1);

            var validResponses = CreateOneValidResponse();
            var proposition = new Proposition { Statement = "valid", ValidResponses = validResponses };

            // act/assert
            simulator.SimulateVote(proposition, populationSize: 0);
        }

        [Test]
        public void SimulateVote_1ValidResponseAndPopulationOf1_CallsMockRandomNumberGenerator()
        {
            // arrange
            var mockGenerater = MockRepository.GenerateMock<IRandomNumberGenerator>();
            mockGenerater.Expect(x => x.GenerateRandomNumber(1)).Return(0);
            var simulator = new VoteSimulator(mockGenerater);

            var validResponses = CreateOneValidResponse();
            var proposition = new Proposition { Statement = "valid", ValidResponses = validResponses };

            // act
            simulator.SimulateVote(proposition, 1);

            // assert
            mockGenerater.VerifyAllExpectations();
        }

        [Test, ExpectedException(typeof(ApplicationException), ExpectedMessage = "Proposition must have at least one valid response")]
        public void SimulateVote_0ValidResponsesAndPopulationOf1_1VoteForFirstReponse()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 1,
                                                                             alwaysGenerateValueOf: 0);
            var validResponses = new string[0];
            var proposition = new Proposition { Statement = "valid", ValidResponses = validResponses };

            // act
            simulator.SimulateVote(proposition, 1);
        }

        [Test]
        public void SimulateVote_1ValidResponseAndPopulationOf1_1VoteForFirstReponse()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 1,
                                                                             alwaysGenerateValueOf: 0);
            var validResponses = CreateOneValidResponse();
            var proposition = new Proposition {Statement = "valid", ValidResponses = validResponses};

            // act
            VoteResult voteResult = simulator.SimulateVote(proposition, 1);

            Assert.That(voteResult.GetNumberOfVotesForResponse(0), Is.EqualTo(1));
        }

        [Test]
        public void SimulateVote_1ValidResponseAndPopulationOf2_2VotesForOnlyReponse()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 1,
                                                                             alwaysGenerateValueOf: 0);
            var validResponses = CreateOneValidResponse();
            var proposition = new Proposition { Statement = "valid", ValidResponses = validResponses };

            // act
            VoteResult voteResult = simulator.SimulateVote(proposition, 2);

            Assert.That(voteResult.GetNumberOfVotesForResponse(0), Is.EqualTo(2));
        }

        [Test]
        public void SimulateVote_2ValidResponseAndPopulationOf2AndBothVoteForFirstResponse_2VotesForFirstReponse()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 2,
                                                                             alwaysGenerateValueOf: 0);
            var validResponses = CreateTwoValidResponses();
            var proposition = new Proposition { Statement = "valid", ValidResponses = validResponses };

            // act
            VoteResult voteResult = simulator.SimulateVote(proposition, 2);

            Assert.That(voteResult.GetNumberOfVotesForResponse(responseId:0), Is.EqualTo(2));
        }

        [Test]
        public void SimulateVote_2ValidResponseAndPopulationOf2AndBothVoteForSecondResponse_2VotesForSecondReponse()
        {
            // arrange
            VoteSimulator simulator = CreateSimulatorWithStubRandomGenerator(maxGeneratedValue: 2,
                                                                             alwaysGenerateValueOf: 1);
            var validResponses = CreateTwoValidResponses();
            var proposition = new Proposition { Statement = "valid", ValidResponses = validResponses };

            // act
            VoteResult voteResult = simulator.SimulateVote(proposition, 2);

            Assert.That(voteResult.GetNumberOfVotesForResponse(responseId: 1), Is.EqualTo(2));
        }

        private static VoteSimulator CreateSimulatorWithStubRandomGenerator(int maxGeneratedValue,
                                                                    int alwaysGenerateValueOf)
        {
            var generatorStub = MockRepository.GenerateStub<IRandomNumberGenerator>();
            generatorStub.Stub(x => x.GenerateRandomNumber(maxGeneratedValue)).Return(alwaysGenerateValueOf);
            return new VoteSimulator(generatorStub);
        }

        private static string[] CreateOneValidResponse()
        {
            return new[] { "option1" };
        }

        private static string[] CreateTwoValidResponses()
        {
            return new[] { "option1", "option2" };
        }
    }
}
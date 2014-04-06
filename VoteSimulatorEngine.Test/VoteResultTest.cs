using System;
using NUnit.Framework;

namespace VoteSimilatorEngine.Test
{
    [TestFixture]
    public class VoteResultTest
    {
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void VoteResult_NullPropositionPassedToConstructor_Throws()
        {
            new VoteResult(null);
        }

        [Test]
        public void VoteResult_ValidPropositionPassedToConstructor_PropositionSetOnProperty()
        {
            var proposition = new Proposition {ValidResponses = new[] {"test"}};
            var result = new VoteResult(proposition);
        
            Assert.That(proposition, Is.EqualTo(result.Proposition));
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void VoteResult_PropositionWithNullResponseList_Throws()
        {
            new VoteResult(new Proposition());
        }

        [Test, ExpectedException(typeof(ApplicationException), ExpectedMessage = "Proposition must have at least one valid response")]
        public void VoteResult_PropositionWithEmptyResponseList_Throws()
        {
            var proposition = new Proposition {ValidResponses = new string[] {}};
            new VoteResult(proposition);
        }

        [Test, ExpectedException(typeof(ApplicationException), ExpectedMessage = "Attempt to read invalid response id from VoteResult")]
        public void GetNumberOfVotes_InvalidResponseId_Throws()
        {
            var proposition = new Proposition { ValidResponses = new[] { "test" } };
            var result = new VoteResult(proposition);

            result.GetNumberOfVotesForResponse(999);
        }

        [Test]
        public void GetNumberOfVotes_ValidPropositionWith1ResponseAddVote_VoteIsRegistered()
        {
            // arrange
            var proposition = new Proposition { ValidResponses = new[] { "test" } };
            var result = new VoteResult(proposition);
            const int responseId = 0;

            // act
            result.AddVote(responseId);
            var votes = result.GetNumberOfVotesForResponse(responseId);
            
            Assert.That(votes, Is.EqualTo(1));
        }

        [Test]
        public void GetNumberOfVotes_ValidPropositionWith2ResponseAddVoteToSecond_VoteIsRegistered()
        {
            // arrange
            var proposition = new Proposition { ValidResponses = new[] { "test1", "test2" } };
            var result = new VoteResult(proposition);
            const int responseId = 1;

            // act
            result.AddVote(responseId);
            var votes = result.GetNumberOfVotesForResponse(responseId);
            
            Assert.That(votes, Is.EqualTo(1));
        }
    }
}

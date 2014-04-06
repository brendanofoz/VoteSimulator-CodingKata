using System;
using System.Collections.Generic;

public class VoteResult
{
    private readonly Proposition _proposition;
    private readonly Dictionary<int, int> votes = new Dictionary<int, int>();

    public VoteResult(Proposition proposition)
    {
        if (proposition == null)
            throw new ArgumentNullException("proposition");

        if (proposition.ValidResponses == null || proposition.ValidResponses.Length == 0)
            throw new ApplicationException("Proposition must have at least one valid response");

        _proposition = proposition;

        // initialise the vote for each response to be 0
        int numResponses = proposition.ValidResponses.Length;
        int currentResponse = 0;
        while (currentResponse < numResponses)
        {
            votes.Add(currentResponse, 0);
            currentResponse++;
        }
    }

    public Proposition Proposition
    {
        get { return _proposition; }
    }

    public void AddVote(int responseId)
    {
        votes[responseId]++;
    }

    public int GetNumberOfVotesForResponse(int responseId)
    {
        int numberOfVotes;
        if (!votes.TryGetValue(responseId, out numberOfVotes))
            throw new ApplicationException("Attempt to read invalid response id from VoteResult");

        return numberOfVotes;
    }
}
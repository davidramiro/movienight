namespace MovieNight.Models.Exceptions;

[Serializable]
public class VotingNotFinishedException : Exception
{
    public VotingNotFinishedException ()
    {}

    public VotingNotFinishedException (string message) 
        : base(message)
    {}

    public VotingNotFinishedException (string message, Exception innerException)
        : base (message, innerException)
    {}   
}
using System;

namespace Challenge.Core.Application.Exceptions
{
    public class ChallengeException: Exception
    {
        public ChallengeException()
        {

        }

        public ChallengeException(string message): base(message)
        {

        }
    }
}

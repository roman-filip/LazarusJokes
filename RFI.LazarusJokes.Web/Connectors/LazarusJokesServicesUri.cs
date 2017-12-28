namespace RFI.LazarusJokes.Web.Connectors
{
    public static class LazarusJokesServicesUri
    {
        private const string _commonPrefix = "LazarusJokes/api/jokes";

        public const string LoadJokes = _commonPrefix;

        public const string AddJoke = _commonPrefix;

        public const string VoteForJoke = _commonPrefix;
    }
}
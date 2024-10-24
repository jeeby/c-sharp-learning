namespace Movies.Api;

public static class ApiEndpoints
{
    public const string ApiBase = "api";

    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";
        public const string Create = Base;
        //public const string Get = $"{Base}/{{id:guid}}"; // :guid is a constraint, so it'll only accept Guids

        // Changed to get Id or Slug:
        public const string Get = $"{Base}/{{idOrSlug}}"; 
        public const string GetAll = Base;
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string Update = $"{Base}/{{id:guid}}";
    }
}

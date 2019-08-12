namespace uSlack.Services.Models
{
    public class InteractiveRoute
    {
        public string Controller { get; set; }
        public string Method { get; set; }

        //Todo: support complex objects
        public string Value { get; set; }
    }
}

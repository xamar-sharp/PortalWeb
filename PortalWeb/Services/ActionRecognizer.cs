namespace PortalWeb.Services
{
    public sealed class ActionRecognizer:IActionRecognizer
    {
        public string Recognize(string intent)
        {
            switch (System.Enum.Parse<ServiceType>(intent))
            {
                case ServiceType.Http:
                    return "HttpInvoke";
                default:
                    return "ServiceNotFound";
            }
        }
    }
}

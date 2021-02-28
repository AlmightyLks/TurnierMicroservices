using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes
{
    public static class Microservices
    {
        public const string GatewayPage = "https://localhost:44338/Views/Gateway";

        public const string LoginServicePage = "https://localhost:44315/Views/Login";
        public const string LoginServiceApi = "https://localhost:44315/api/Message";

        public const string MitgliederServicePage = "https://localhost:44366/Views/Verwaltung";
        public const string MitgliederServiceApi = "https://localhost:44366/api/Message";

        public const string MannschaftsServicePage = "https://localhost:44335/Views/Verwaltung";
        public const string MannschaftsServiceApi = "https://localhost:44335/api/Message";
    }
}

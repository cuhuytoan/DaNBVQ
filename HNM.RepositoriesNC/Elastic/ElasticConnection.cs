using Nest;
using System;

namespace HNM.RepositoriesNC.Elastic
{
    public class ElasticConnection
    {
        public static ElasticClient _elaticClient;
        public ElasticConnection()
        {
            var elasticUri = "http://202.134.18.185:9200";
            var settings = new ConnectionSettings(new Uri(elasticUri));

            _elaticClient = new ElasticClient(settings);
        }
    }
}

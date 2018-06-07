using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace AAR_Bot.Helper.webscraping
{
    public class CosmosDBService
    {
        private static readonly string DatabaseId = "ToDoList";//ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = "Items";// ConfigurationManager.AppSettings["collection"];
        private static readonly string endPoint = "https://mjukje.documents.azure.com/";// ConfigurationManager.AppSettings["endpoint"];
        private static readonly string authKey = "5MfRHpG8g1x534IXnilPh4DzhZOk94S440mTZ4rCSTiUFnyzq1Fr4lnOi9IscugVUNNFVFWahEQs4Yo06AqQDA==";// ConfigurationManager.AppSettings["authKey"]
        DocumentClient documentClient;

        public CosmosDBService()
        {

            documentClient = new DocumentClient(new Uri(endPoint), authKey);
        }


        public async Task<IEnumerable<ConversationInfo>> readDataFromDocument(ConversationInfo info)
        {

            //await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), info);
            IDocumentQuery<ConversationInfo> query = documentClient.CreateDocumentQuery<ConversationInfo>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(d => d.id == info.id)
                .AsDocumentQuery();

            List<ConversationInfo> results = new List<ConversationInfo>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<ConversationInfo>());
            }

            return results;
        }
        public async Task SetInfoAsync(ConversationInfo info, string id)
        {
            try
            {
                await documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), info);
            }
            catch(Exception e)
            {
                 await documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), info);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using fn_save_sql.Model;

namespace AzureSQL.ToDo
{
    public static class PostToDo
    {
        [Function(nameof(PostToDo))]
        public static async Task<OutputType> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "PostFunction")] 
            HttpRequestData req, FuctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PostToDo");
            logger.LogInformation("C# HTTP trigger function processed a request");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ToDoItem toDoItem = JsonConvert.DeserializeObject<toDoItem>(requestBody);

            toDoItem.Id = Guid.NewGuid();
            toDoItem.url = "google.com/" + "?id=" + toDoItem.Id.ToString();

            if(toDoItem.completed == null)
            {
                toDoItem.completed = false;
            }

            return new OutputType()
            {
                ToDoItem = toDoItem,
                HttpResponse = req.CreateResponse(System.Net.HttpStatusCode.Created)
            };
        }
    }

    public class OutputType
    {
        [SqlOutput("dbo.ToDo", connectionStringSetting: "SqlConnectionString")]
        public ToDoItem ToDoItem { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}
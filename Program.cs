using System.Collections;
using System.Net.Http.Headers;
using System.Text.Json;
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

await ProcessJsonAsync(client);
//


static async Task ProcessJsonAsync(HttpClient client)
{
    var json = await client.GetStringAsync("https://api.sampleapis.com/codingresources/codingResources");

    HashSet<string> uniqueTopics = new HashSet<string>(); 
    List<Model>? lstModels;
    if(json != null)
    {
        lstModels = JsonSerializer.Deserialize<List<Model>>(json);
        uniqueTopics = GetUniqueTopicsFromList(lstModels);
    }

    if (uniqueTopics != null){
        foreach(string uniqueTopic in uniqueTopics) {
            Console.WriteLine(uniqueTopic);
        }
    }
}


static HashSet<string> GetUniqueTopicsFromList(List<Model>? lstModels)
{
    HashSet<string> uniqueTopics = new HashSet<string>();
    if (lstModels != null)
    {
        foreach (var item in lstModels)
        {
            if (item.topics != null)
            {
                foreach (string topic in item.topics)
                {
                    /* I opted for not parsing topics written as whole strings

                    examples:
                    "mobile app development Kotlin machine learning java android"
                    "Xcode vue AdobeXD Sketch mobile app development React swift iOS"

                    Although "Xcode" and "Vue" look like they should be each one topic, terms like "mobile app development" should be kept as multi-string

                    */
                    uniqueTopics.Add(topic);
                }
            }
        }
    }
    return uniqueTopics;
}


public class Model {
    public int id {get; set;}
    public string? description {get; set;}
    public string? url {get; set;}
    public string[]? types {get; set;}
    public string[]? topics {get; set;}
    public string[]? levels {get; set;}
}

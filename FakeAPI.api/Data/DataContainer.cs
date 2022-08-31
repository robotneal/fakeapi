using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace FakeAPI.Api;

public interface IDataContainer
{
    Task<T> Get<T>(IdString id, UserName userName);
    Task<T> Upsert<T>(T item);
    Task Delete<T>(IdString id, UserName userName);
    Task<IEnumerable<T>> GetAll<T>(UserName userName);
}

public class DataContainer : IDataContainer
{
    private readonly Container _cosmosContainer;
    public DataContainer(Container cosmosContainer)
    {
        _cosmosContainer = cosmosContainer;
    }

    public async Task<T> Get<T>(IdString id, UserName userName)
    {
        var app = await _cosmosContainer.ReadItemAsync<T>(id, new PartitionKey(userName));
        return app;
    }

    public async Task<T> Upsert<T>(T item)
    {
        var response = await _cosmosContainer.UpsertItemAsync(item);
        return response.Resource;
    }

    public async Task Delete<T>(IdString id, UserName userName)
    {
        await _cosmosContainer.DeleteItemAsync<T>(id, new(userName));
    }

    public async Task<IEnumerable<T>> GetAll<T>(UserName userName)
    {
        using var setIterator = _cosmosContainer.GetItemQueryIterator<T>(
            "SELECT * FROM c",
            requestOptions: new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey(userName)
            });

        var items = new List<T>();
        while (setIterator.HasMoreResults)
        {
            FeedResponse<T> response = await setIterator.ReadNextAsync();
            items.AddRange(response);
        }
        return items;
    }
}
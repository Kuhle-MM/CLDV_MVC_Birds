using Azure;
using Azure.Data.Tables;
using CLDV_MVC_Birds.Models;

namespace CLDV_MVC_Birds.Services
{
    public class TableStorageService
    {
        private readonly TableClient _tableClient; // for the bird table
        private readonly TableClient _birdTableClient; // for the birder table
        private readonly TableClient _sightingTableClient; // used for sighting

        public TableStorageService(string connectionString)
        {
            _tableClient = new TableClient(connectionString, "Bird");
            _birdTableClient = new TableClient(connectionString, "Birder");
            _sightingTableClient = new TableClient(connectionString, "Sightings");

        }

        public async Task<List<Bird>> GetAllBirdsAsync()
        {
            var birds = new List<Bird>();
            await foreach (var bird in _tableClient.QueryAsync<Bird>())
            {
                birds.Add(bird);
            }
            return birds;
        }

        public async Task addBirdAsync(Bird bird)
        {
            if (string.IsNullOrEmpty(bird.PartitionKey) || string.IsNullOrEmpty(bird.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set");

            }

            try
            {
                await _tableClient.AddEntityAsync(bird);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to table storage", ex);
            }
        }

        public async Task DeleteBirdAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<Bird?> GetBirdAsync(string partitionKey, string rowkey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<Bird>(partitionKey, rowkey);
                return response.Value;
            } catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///BIRDER
        public async Task<List<Birder>> GetAllBirdersAsync()
        {
            var birders = new List<Birder>();
            await foreach (var birder in _birdTableClient.QueryAsync<Birder>())
            {
                birders.Add(birder);
            }
            return birders;
        }

        public async Task addBirderAsync(Birder birder)
        {
            if (string.IsNullOrEmpty(birder.PartitionKey) || string.IsNullOrEmpty(birder.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set");

            }

            try
            {
                await _birdTableClient.AddEntityAsync(birder);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to table storage", ex);
            }
        }

        public async Task DeleteBirderAsync(string partitionKey, string rowKey)
        {
            await _birdTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<Birder?> GetbirderAsync(string partitionKey, string rowkey)
        {
            try
            {
                var response = await _birdTableClient.GetEntityAsync<Birder>(partitionKey, rowkey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        #region Sightings
        public async Task AddSightingAsync(Sighting sighting)
        {
            if (string.IsNullOrEmpty(sighting.PartitionKey) || string.IsNullOrEmpty(sighting.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set");
            }
            try
            {
                await _sightingTableClient.AddEntityAsync(sighting);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding sighting to table storage", ex);
            }

        }
        public async Task<List<Sighting>> GetAllSightingAsync()
        {
            var sightings = new List<Sighting>();
            await foreach(var sighting in _sightingTableClient.QueryAsync<Sighting>())
            {
                sightings.Add(sighting);
            }
            return sightings;
        }
    }
    #endregion
}


﻿using Avanade.AzureWorkshop.WebApp.Models.TableStorageModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class TeamsRepository
    {
        private CloudTableClient GetClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["storageConnectionString"]);
            return storageAccount.CreateCloudTableClient();
        }

        public async Task StoreTeams(IEnumerable<TeamEntity> teams)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("teams");

            await table.CreateIfNotExistsAsync();

            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach(var team in teams)
            {
                batchOperation.Insert(team);
            }

            table.ExecuteBatch(batchOperation);
        }

        public async Task StorePlayers(IEnumerable<PlayerEntity> players)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("players");

            await table.CreateIfNotExistsAsync();            

            foreach (var group in players.GroupBy(p => p.PartitionKey))
            {
                foreach(var player in group)
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();
                    batchOperation.Insert(player);
                    table.ExecuteBatch(batchOperation);
                }                
            }            
        }

        public IEnumerable<TeamEntity> FetchTeams()
        {
            return Enumerable.Empty<TeamEntity>();
        }

        public IEnumerable<PlayerEntity> FetchPlayers(string teamId)
        {
            return Enumerable.Empty<PlayerEntity>();
        }
    }
}
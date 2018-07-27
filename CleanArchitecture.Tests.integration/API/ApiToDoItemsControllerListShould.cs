﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.Tests.Integration.API
{

    public class ApiToDoItemsControllerList : BaseWebTest
    {
        [Fact]
        public async Task ReturnsTwoItems()
        {
            var response = await _client.GetAsync("/api/todoitems");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ToDoItem>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.Count(a => a.Title == "Test Item 1"));
            Assert.Equal(1, result.Count(a => a.Title == "Test Item 2"));
        }
    }
}
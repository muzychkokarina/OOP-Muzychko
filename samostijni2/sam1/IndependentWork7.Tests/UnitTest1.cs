using System;
using System.Collections.Generic;
using Xunit;
using IndependentWork7;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_And_Get_ShouldStoreAndRetrieveItem()
        {
            var cache = new Cache<string, int>(3);

            cache.Add("A", 100);

            Assert.Equal(100, cache.Get("A"));
            Assert.Equal(1, cache.Count);
        }

        [Fact]
        public void Add_WhenCapacityExceeded_ShouldEvictOldestItem_FIFO()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "First");
            cache.Add(2, "Second");

            cache.Add(3, "Third");

            Assert.Equal(2, cache.Count);
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
            Assert.Equal("Second", cache.Get(2));
            Assert.Equal("Third", cache.Get(3));
        }

        [Fact]
        public void Get_NonExistentKey_ShouldThrowKeyNotFoundException()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("key1", "value1");

            Assert.Throws<KeyNotFoundException>(() => cache.Get("missing_key"));
        }

        [Fact]
        public void ContainsKey_ShouldReturnTrueForExisting_AndFalseForMissing()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("existing", "data");

            Assert.True(cache.ContainsKey("existing"));
            Assert.False(cache.ContainsKey("non_existing"));
        }

        [Fact]
        public void Count_ShouldReflectCurrentNumberOfElements()
        {
            var cache = new Cache<int, int>(3);
            Assert.Equal(0, cache.Count);

            cache.Add(1, 10);
            Assert.Equal(1, cache.Count);

            cache.Add(2, 20);
            Assert.Equal(2, cache.Count);

            
            cache.Add(1, 99);
            Assert.Equal(2, cache.Count);
            Assert.Equal(99, cache.Get(1));
        }

        [Fact]
        public void Constructor_WithInvalidCapacity_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<string, string>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<string, string>(-5));
        }
    }
}
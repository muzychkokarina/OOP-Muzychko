using System.Collections.Generic;
using System.Linq;
using lab7v13;
using Xunit;

namespace lab7v13.Tests
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void Add_ShouldAddEntityToRepository()
        {
            var repo = new InMemoryRepository<City>();
            var city = new City { Id = 1, Name = "Lviv", Country = "Ukraine", Population = 720000 };

            repo.Add(city);

            var result = repo.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("Lviv", result.Name);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectEntity_WhenExists()
        {
            var repo = new InMemoryRepository<City>();
            repo.Add(new City { Id = 10, Name = "Odesa", Country = "Ukraine", Population = 1000000 });

            var result = repo.GetById(10);

            Assert.NotNull(result);
            Assert.Equal("Odesa", result.Name);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            var repo = new InMemoryRepository<City>();

            var result = repo.GetById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEntities()
        {
            var repo = new InMemoryRepository<City>();
            repo.Add(new City { Id = 1, Name = "City1", Country = "Country1", Population = 100 });
            repo.Add(new City { Id = 2, Name = "City2", Country = "Country2", Population = 200 });

            var allCities = repo.GetAll();

            Assert.Equal(2, allCities.Count());
        }

        [Fact]
        public void Update_ShouldUpdateExistingEntity()
        {
            var repo = new InMemoryRepository<City>();
            repo.Add(new City { Id = 1, Name = "OldName", Country = "Ukraine", Population = 1000 });

            var updatedCity = new City { Id = 1, Name = "NewName", Country = "Ukraine", Population = 2000 };
            repo.Update(updatedCity);

            var result = repo.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("NewName", result.Name);
            Assert.Equal(2000, result.Population);
        }

        [Fact]
        public void Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            var repo = new InMemoryRepository<City>();
            var nonExistingCity = new City { Id = 999, Name = "GhostCity", Country = "Unknown", Population = 0 };

            Assert.Throws<KeyNotFoundException>(() => repo.Update(nonExistingCity));
        }

        [Fact]
        public void Delete_ShouldRemoveExistingEntity()
        {
            var repo = new InMemoryRepository<City>();
            repo.Add(new City { Id = 5, Name = "Kharkiv", Country = "Ukraine", Population = 1400000 });

            repo.Delete(5);

            var result = repo.GetById(5);
            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
        {
            var repo = new InMemoryRepository<City>();

            Assert.Throws<KeyNotFoundException>(() => repo.Delete(999));
        }
    }
}
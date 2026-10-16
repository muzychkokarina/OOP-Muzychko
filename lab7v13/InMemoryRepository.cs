using System;
using System.Collections.Generic;
using System.Linq;

namespace lab7v13
{
    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _items.Add(item);
        }

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var existingIndex = _items.FindIndex(x => x.Id == item.Id);
            if (existingIndex == -1)
            {
                throw new KeyNotFoundException($"Елемент з Id = {item.Id} не знайдено.");
            }

            _items[existingIndex] = item;
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
            }

            _items.Remove(item);
        }
    }
}
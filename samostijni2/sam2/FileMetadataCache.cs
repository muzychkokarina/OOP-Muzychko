using System;
using System.Collections.Generic;

namespace sam2
{
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{FileName} ({Size} байт, змінено: {LastModified:yyyy-MM-dd HH:mm}, Hash: {Hash})";
        }
    }

    public class FileMetadataCache
    {
        private readonly Dictionary<string, FileMetadata> _cache;

        public FileMetadataCache()
        {
            _cache = new Dictionary<string, FileMetadata>(StringComparer.OrdinalIgnoreCase);
        }

        public int Count => _cache.Count;

        public void Add(string filePath, FileMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Шлях до файлу не може бути порожнім.", nameof(filePath));

            _cache[filePath] = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public FileMetadata? Get(string filePath)
        {
            _cache.TryGetValue(filePath, out var metadata);
            return metadata;
        }

        public bool Remove(string filePath)
        {
            return _cache.Remove(filePath);
        }

        public bool Contains(string filePath)
        {
            return _cache.ContainsKey(filePath);
        }
    }
}
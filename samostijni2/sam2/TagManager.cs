using System;
using System.Collections.Generic;

namespace sam2
{
    public class TagManager
    {
        private readonly Dictionary<string, HashSet<string>> _articleTags;

        public TagManager()
        {
            _articleTags = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.TryGetValue(articleId, out var tags))
            {
                tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _articleTags[articleId] = tags;
            }

            tags.Add(tag);
        }

        public bool RemoveTag(string articleId, string tag)
        {
            if (_articleTags.TryGetValue(articleId, out var tags))
            {
                return tags.Remove(tag);
            }

            return false;
        }

        public HashSet<string> GetAllUniqueTags()
        {
            var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tags in _articleTags.Values)
            {
                allTags.UnionWith(tags);
            }

            return allTags;
        }

        public HashSet<string> FindCommonTags(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1) ||
                !_articleTags.TryGetValue(articleId2, out var tags2))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            var common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            common.IntersectWith(tags2);
            return common;
        }

        public HashSet<string> GetExclusiveTags(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            var exclusive = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            if (_articleTags.TryGetValue(articleId2, out var tags2))
            {
                exclusive.ExceptWith(tags2);
            }

            return exclusive;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CachingDynamicDecorator.Infrastructure
{
    public class CachedInvocationResponseCollection
    {
        private List<CachedItem<InvocationResponsePair>> _cachedItems = new List<CachedItem<InvocationResponsePair>>();
        private readonly TimeSpan _itemCacheDuration;

        public CachedInvocationResponseCollection(TimeSpan itemCacheDuration)
        {
            _itemCacheDuration = itemCacheDuration;
        }

        public void Add(InvocationResponsePair invocationResponsePair)
        {
            RemoveById(invocationResponsePair.MethodId);
            _cachedItems.Add(new CachedItem<InvocationResponsePair>(invocationResponsePair, _itemCacheDuration));
        }

        public object GetValueById(MethodIdentifier methodId)
        {
            return _cachedItems.Single(x => x.Item.MatchesId(methodId)).Item.Response;
        }

        public bool ContainsById(MethodIdentifier methodId)
        {
            UpdateCache();
            return _cachedItems.Any(x => x.Item.MatchesId(methodId));
        }

        private void RemoveById(MethodIdentifier methodId)
        {
            UpdateCache();
            if (ContainsById(methodId))
                _cachedItems.RemoveAll(x => x.Item.MatchesId(methodId));
        }

        private void UpdateCache()
        {
            _cachedItems = _cachedItems.Where(x => !x.IsExpired).ToList();
        }
    }
}
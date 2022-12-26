using System;
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using System.Text;
using Models;

namespace GroupsTags
{
    class GroupsTagsNamedTags
    {
        private static ICache _cache;
        public static void Run()
        {
            InitializeCache();
            assignnamedtags();
        }

        private static void InitializeCache()
        {
            _cache = CacheManager.GetCache("ClusteredCacheZainTest");
            Console.WriteLine("Cache Initialized");
        }

        private static void AddGroups()
        {
            for (int i = 21; i <= 40; i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                cacheitem.Group = "East Coast";
                _cache.Insert(key, cacheitem);
            }

            for (int i = 41; i <= 60; i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                cacheitem.Group = "North Coast";
                _cache.Insert(key, cacheitem);
            }
        }

        private static void assignnamedtags()
        {

            //Named Tags can only be searched using SQL search
            for (int i = 1; i <= 20; i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                var namedtags = new NamedTagsDictionary();
                char? c = null;
                namedtags.Add("Region", c);
                namedtags.Add("Ability", "Mind Reading");
                namedtags.Add("Age", 26);
                cacheitem.NamedTags = namedtags;
                _cache.Insert(key, cacheitem);
            }   
        }

        private static void assigntags()
        {
            Tag[] tags = new Tag[3];
            tags[0] = new Tag("New Emp");
            tags[1] = new Tag("Most Payed");
            tags[2] = new Tag("VIP Emp");

            for(int i=61;i<=80;i++)
            {
                string key = "ALPHAK" + i;
                Customer cust = _cache.Get<Customer>(key);
                var cacheitem = new CacheItem(cust);
                cacheitem.Tags = tags;
                _cache.Insert(key, cacheitem);
            }
        }

        private static void retreivegroups()
        {
            //to retreive the data along with the keys use IDictionary<string, objecy> instead of ICollection.
            string groupname= "East Coast";
            ICollection<string> keys = _cache.SearchService.GetGroupKeys(groupname);
            if(keys!=null && keys.Count>0)
            {
                foreach(var key in keys)
                {
                    Console.WriteLine("key associated with the group '{key}'");
                }
            }
        }
        private static void retreivetags()
        {
            //Can also pass array of tags. for that use GetKeysByTags Overload
            //TagSearchOption can be used as any and all to get records that have all the tags,
            //or the records associated with any tags.
            //To get the data with the record as well. Use GetByTag/Tags Api, can be used with the search options as well.
            Tag tag = new Tag("Most Payed");
            ICollection<string> keys = _cache.SearchService.GetKeysByTag(tag);

            if(keys!=null && keys.Count>0)
            {
                foreach(var key in keys)
                {
                    Console.WriteLine("key associated with the tag '{tag}': key '{key}'");
                }
            }
        }

        private static void retrievewildcard()
        {
            string wild = "* Payed";
            ICollection<string> keys = _cache.SearchService.GetKeysByTag(wild);
            if (keys != null && keys.Count > 0)
            {
                foreach (var key in keys)
                {
                    Console.WriteLine("key associated with the suffix Payed: key '{key}'");
                }
            }
        }


    }
}

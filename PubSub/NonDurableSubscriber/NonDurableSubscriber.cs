// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Caching.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonDurableSubscriberTest
{
    class NonDurableSubscriber
    {
        private static ICache _cache;

        static public void Run()
        {
            try
            {
                InitializeCache();

                ITopicSubscription electronicsSubscription = RunSubscriber("ElectronicsOrders");
                ITopicSubscription garmentsSubscription = RunSubscriber("GarmentsOrders");

                //Following patterns can be used;
                //* -	 represents zero or more characters. For example, ‘bl*’ finds TOPICs ‘black’, ‘blue’, and ‘blob’ etc.
                //? -	 represents one character. For example, ‘h?t’ finds ‘hot’, ‘hat’, and ‘hit’ etc.
                //[]-	 represents any single character within the brackets. For example, ‘h[oa]t’ finds hot and hat, but not hit.

                ITopicSubscription allOrdersSubscription = RunPatternBasedSubscriber("*Orders");

                Console.WriteLine("Press any key to stop subscribers...");
                Console.ReadLine();
                electronicsSubscription.UnSubscribe();
                garmentsSubscription.UnSubscribe();
                allOrdersSubscription.UnSubscribe();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static ITopicSubscription RunSubscriber(string topicName)
        {
            // Initialize cache
            InitializeCache();

            ITopic _topic = _cache.MessagingService.GetTopic(topicName);
            if (_topic == null)
                _topic = _cache.MessagingService.CreateTopic(topicName);

            // Subscribes to the topic.
            return _topic.CreateSubscription(MessageReceivedCallback);
        }

        public static ITopicSubscription RunPatternBasedSubscriber(string pattern)
        {
            //Create Subscription on all the topics that matches the pattern.
            ITopic _patternTopic = _cache.MessagingService.GetTopic(pattern, TopicSearchOptions.ByPattern);

            // Subscribes to the topic.
            if (_patternTopic != null)
                return _patternTopic.CreateSubscription(PatternMessageReceivedCallback);

            return null;
        }

        public static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }


        /// <summary>
        /// This method will get invoked if a message is recieved by the subscriber.
        /// </summary>
        static void MessageReceivedCallback(object sender, MessageEventArgs args)
        {
            Console.WriteLine("Message Recieved for "+args.TopicName);
        }

        /// <summary>
        /// This method will get invoked if a message is recieved by the subscriber.
        /// </summary>
        static void PatternMessageReceivedCallback(object sender, MessageEventArgs args)
        {
            Console.WriteLine("Message Recieved on pattern based subscription for " + args.TopicName);
        }
    }
}

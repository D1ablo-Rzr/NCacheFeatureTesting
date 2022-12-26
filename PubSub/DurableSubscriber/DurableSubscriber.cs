using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Runtime.Exceptions;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;


namespace DurableSubscriberTest
{
    class DurableSubscriber
    {
        private static  ICache _cache;

        public static void Run()
        {
            try
            {
                InitializeCache();
                ITopic _topic = _cache.MessagingService.GetTopic("ElectronicsOrders");
                Console.WriteLine(_topic.MessageCount);
                //IDurableTopicSubscription electronicsSharedSubs = RunSubscriber("ElectronicsOrders", "ElectronicsSubscription", SubscriptionPolicy.Shared);
                //Console.ReadLine();
                //IDurableTopicSubscription electronicsSharedSubs1 = RunSubscriber("ElectronicsOrders", "ElectronicsSubscription", SubscriptionPolicy.Shared);
                //Console.ReadLine();
                //IDurableTopicSubscription electronicsSharedSubs2 = RunSubscriber("ElectronicsOrders", "ElectronicsSubscription", SubscriptionPolicy.Shared);
                //IDurableTopicSubscription electronicsSharedSubs3 = RunSubscriber("ElectronicsOrders", "ElectronicsSubscription", SubscriptionPolicy.Shared);
                //IDurableTopicSubscription electronicsExclusiveSubs = RunSubscriber("ElectronicsOrders", "ElectronicsSubscription1", SubscriptionPolicy.Exclusive);

                ITopicSubscription topicSubscription = RunSubscriber("ElectronicsOrders");

                //IDurableTopicSubscription allOrdersSubscription = RunPatternBasedSubscriber("*Orders", "AllOrdersSubscription");
                Console.WriteLine("Unsubscribing");
                Console.ReadLine();
                //electronicsSharedSubs.UnSubscribe();
                //electronicsSharedSubs1.UnSubscribe();
                //electronicsSharedSubs2.UnSubscribe();
                //electronicsSharedSubs3.UnSubscribe();
                //electronicsExclusiveSubs.UnSubscribe();

                topicSubscription.UnSubscribe();

            }
            catch (CacheException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.SUBSCRIPTION_EXISTS)
                {
                    Console.WriteLine("Active Subscription with this name already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static ITopicSubscription RunSubscriber(string topicName)
        {
            // Initialize cache

            ITopic _topic = _cache.MessagingService.GetTopic(topicName);
            if (_topic == null)
                _topic = _cache.MessagingService.CreateTopic(topicName);

            // Subscribes to the topic.
            Console.WriteLine("Non Durable Subscriber Running");
            return _topic.CreateSubscription(MessageReceivedCallback);
        }

        public static IDurableTopicSubscription RunSubscriber(string topicName,string subscriptionName, SubscriptionPolicy policy)
        {
            Console.WriteLine("Subscriber Started");
            ITopic topic = _cache.MessagingService.GetTopic(topicName);

            if (topic == null)
                topic = _cache.MessagingService.CreateTopic(topicName);

            //Creates durable subscription 
            //subscriptionName: User defined name for subscription
            //Subscription Policy: If subscription policy is shared , it means subscription can hold multiple active subscribers 
            //                     If subscription policy is exclusive , it means subscription can hold one active client only.
            Console.WriteLine(policy.ToString());
            return topic.CreateDurableSubscription(subscriptionName, policy, MessageReceivedCallback, new TimeSpan(0, 50, 0));
        }

        public static void InitializeCache()
        {
            _cache = CacheManager.GetCache("PubSubCache");

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", "PubSubCache"));
        }

        public static IDurableTopicSubscription RunPatternBasedSubscriber(string topicPattern,string subscriptionName)
        {
            ITopic topic = _cache.MessagingService.GetTopic(topicPattern, Alachisoft.NCache.Runtime.Caching.Messaging.TopicSearchOptions.ByPattern);

            //Create durable subscriptions with an expiry of 1 hour on all the topics matching the specified pattern
            if (topic != null)
                return topic.CreateDurableSubscription(subscriptionName, SubscriptionPolicy.Shared, MessageReceivedCallbackPatternBased, new TimeSpan(0, 15, 0));
            return null;
        }

        /// <summary>
        /// This method will get invoked if a message is recieved by the subscriber.
        /// </summary>
        static void MessageReceivedCallback(object sender, MessageEventArgs args)
        {
            
            Console.WriteLine("Message Received from" + args.Message.Payload);
            Console.WriteLine("Message Recieved for " + args.TopicName);
        }
        /// <summary>
        /// This method will get invoked if a message is recieved by the subscriber.
        /// </summary>
        static void MessageReceivedCallbackPatternBased(object sender, MessageEventArgs args)
        {
            Console.WriteLine("Message Recieved on Pattern Based subscription for " + args.TopicName);
            //Console.WriteLine(args.Message.Payload);
        }
    }
}

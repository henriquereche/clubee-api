using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Clubee.API.Services
{
    public class RelevanceEmitter
    {
        private readonly IMongoRepository Repository;
        private readonly IDictionary<string, RelevanceEntry> Index;
        private readonly Timer Timer;

        private static object Lock = new object();

        /// <summary>
        /// RelevanceEmitter singleton instance.
        /// </summary>
        public static RelevanceEmitter Instance;

        ~RelevanceEmitter()
        {
            this.Timer.Dispose();
        }

        private RelevanceEmitter(IMongoRepository repository)
        {
            this.Repository = repository;
            this.Index = new Dictionary<string, RelevanceEntry>();
            this.Timer = new Timer(
                this.Register, 
                null, 
                TimeSpan.FromSeconds(3), 
                TimeSpan.FromMinutes(1)
            );
        }

        /// <summary>
        /// Initialize application relevance emitter.
        /// </summary>
        /// <param name="repository"></param>
        public static void Initialize(IMongoRepository repository)
        {
            RelevanceEmitter.Instance = new RelevanceEmitter(repository);
        }

        /// <summary>
        /// Check if emitter has pending entry based on its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            lock (RelevanceEmitter.Lock)
            {
                if (this.Index.ContainsKey(key))
                {
                    RelevanceEntry entry = this.Index[key];

                    entry.ResetExpiration();
                    this.Index[key] = entry;

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Add new entry to emitter.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        public void Add(string key, RelevanceCount count)
        {
            if (!this.HasKey(key))
                lock (RelevanceEmitter.Lock)
                    this.Index.Add(key, new RelevanceEntry(count));
        }

        private void Register(object state)
        {
            lock(RelevanceEmitter.Lock)
            {
                foreach (KeyValuePair<string, RelevanceEntry> entry 
                    in this.Index.Where(entry => entry.Value.Expiration <= DateTime.Now).ToList())
                {
                    this.Repository.Insert(entry.Value.Entry);
                    this.Index.Remove(entry.Key);
                }
            }
        }

        protected class RelevanceEntry
        {
            public RelevanceEntry(RelevanceCount entry)
            {
                this.Entry = entry;
                this.ResetExpiration();
            }

            public DateTime Expiration { get; private set; }
            public RelevanceCount Entry { get; private set; }

            public void ResetExpiration()
            {
                this.Expiration = DateTime.Now.AddMinutes(2);
            }
        }
    }
}

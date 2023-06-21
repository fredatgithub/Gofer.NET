﻿using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Gofer.NET
{
  public static class RedisExtensions
  {
    /// <summary>
    /// Blocks for blockingTimeout amount of time, and tries to take the lock at an interval of tryLockInterval.
    /// If the lock cannot be obtained before the timeout, throws an exception.
    /// </summary>
    /// <exception cref="ApplicationException">
    /// Thrown if the attempt to lock the resource takes longer than blockingTimeout.
    /// </exception>
    public static async Task<RedisLock> LockBlockingAsync(this IConnectionMultiplexer redis,
        string key,
        TimeSpan? duration = null,
        TimeSpan? blockingTimeout = null,
        TimeSpan? tryLockInterval = null,
        string token = null)
    {
      duration = duration ?? TimeSpan.FromMinutes(5);
      blockingTimeout = blockingTimeout ?? TimeSpan.FromSeconds(60);
      tryLockInterval = tryLockInterval ?? TimeSpan.FromMilliseconds(200);

      RedisValue actualToken = token ?? Guid.NewGuid().ToString();
      var db = redis.GetDatabase();

      var startTime = DateTime.Now;
      while (true)
      {
        if (await db.LockTakeAsync(key, actualToken, duration.Value))
        {
          return new RedisLock(db, key, actualToken);
        }

        await Task.Delay(tryLockInterval.Value);

        if ((DateTime.Now - startTime) >= blockingTimeout)
        {
          throw new Exception("Unable to lock resource.");
        }
      }
    }

    /// <summary>
    /// Tries to take a lock, then returns null immediately if it's not successful.
    /// </summary>
    /// <param name="redis"></param>
    /// <param name="key"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static async Task<RedisLock> LockNonBlockingAsync(this IConnectionMultiplexer redis,
        string key,
        TimeSpan? duration = null,
        string token = null)
    {
      duration = duration ?? TimeSpan.FromMinutes(5);

      RedisValue actualToken = token ?? Guid.NewGuid().ToString();
      var db = redis.GetDatabase();

      if (await db.LockTakeAsync(key, actualToken, duration.Value))
      {
        return new RedisLock(db, key, actualToken);
      }

      return null;
    }

  }
}
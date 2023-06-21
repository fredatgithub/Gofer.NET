using Gofer.NET.Utils;
using Newtonsoft.Json;
using System;

namespace Gofer.NET
{
  [JsonObject(MemberSerialization.OptIn)]
  public class ScheduledTask
  {
    public string LockKey => $"{nameof(ScheduledTask)}::{TaskKey}::ScheduleLock";

    [JsonProperty]
    public string TaskKey { get; private set; }

    [JsonProperty]
    public long ScheduledUnixTimeMilliseconds { get; private set; }

    [JsonProperty]
    public TaskInfo TaskInfo { get; private set; }

    public ScheduledTask() { }

    public ScheduledTask(
        TaskInfo taskInfo,
        TimeSpan offset,
        string taskKey) : this(taskInfo, new DateTimeOffset(DateTime.UtcNow + offset), taskKey)
    {
    }

    public ScheduledTask(
        TaskInfo taskInfo,
        DateTime scheduledTime,
        TaskQueue taskQueue,
        string taskKey) : this(taskInfo, new DateTimeOffset(scheduledTime), taskKey)
    {
    }

    public ScheduledTask(
        TaskInfo taskInfo,
        DateTimeOffset scheduledDateTimeOffset,
        string taskKey)
    {
      TaskKey = taskKey;
      TaskInfo = taskInfo;
      ScheduledUnixTimeMilliseconds = scheduledDateTimeOffset.ToUnixTimeMilliseconds();
    }
  }
}
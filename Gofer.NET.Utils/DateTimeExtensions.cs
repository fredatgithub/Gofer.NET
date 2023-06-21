using System;

namespace Gofer.NET.Utils
{
  public static class DateTimeExtensions
  {
    public static long ToUnixTimeMilliseconds(this DateTime source)
    {
      return new DateTimeOffset(source).ToUnixTimeMilliseconds();
    }

    public static long ToUnixTimeSeconds(this DateTime source)
    {
      return new DateTimeOffset(source).ToUnixTimeSeconds();
    }
  }
}
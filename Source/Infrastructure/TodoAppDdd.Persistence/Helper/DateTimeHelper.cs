using System;

namespace TodoAppDdd.Persistence
{
	public static class DateTimeHelper
	{
		public static string ToIso8601(this DateTime dateTime)
		{
			return DateTime.UtcNow.ToString("o");
		}

		public static DateTime FromIso8601ToDateTime(this string iso8601String)
		{
			return DateTime.Parse(iso8601String);
		}

		public static bool IsOlderThan(this DateTime dateTime, TimeSpan span)
		{
			if (DateTime.Now.Subtract(dateTime) > span)
			{
				return true;
			}

			return false;
		}
	}
}
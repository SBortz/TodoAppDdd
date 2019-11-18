namespace TodoAppDdd.Api.Filter
{
	public class ErrorResult
	{
		public string ExceptionType { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
	}
}
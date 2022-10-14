namespace TechBlog.Web.Models;

public class AlertMessage
{
	public string Title { get; set; }

	public string Message { get; set; }

	public string Type { get; set; }

	public AlertMessage()
	{
		Type = AlertType.Info;
	}

	public AlertMessage(string title, string message, string type)
	{
		Title = title;
		Message = message;
		Type = type;
	}
	
	public static AlertMessage Info(string message, string title = "Information")
	{
		return new AlertMessage(title, message, AlertType.Info);
	}

	public static AlertMessage Success(string message, string title = "Success")
	{
		return new AlertMessage(title, message, AlertType.Success);
	}

	public static AlertMessage Warn(string message, string title = "Warning")
	{
		return new AlertMessage(title, message, AlertType.Warning);
	}

	public static AlertMessage Error(string message, string title = "Error")
	{
		return new AlertMessage(title, message, AlertType.Error);
	}

	public class AlertType
	{
		public const string Info = "info";
		public const string Success = "success";
		public const string Warning = "warning";
		public const string Error = "danger";
	}
}
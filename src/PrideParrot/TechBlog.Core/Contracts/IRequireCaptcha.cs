namespace TechBlog.Core.Contracts;

public interface IRequireCaptcha
{
	public string CaptchaToken { get; set; }
}
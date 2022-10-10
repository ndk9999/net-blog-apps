using TechBlog.Core.Contracts;

namespace TechBlog.Web.Providers;

public interface ICaptchaProvider
{
	Task<bool> VerifyAsync(IRequireCaptcha captcha);
}
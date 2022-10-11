using TechBlog.Core.Contracts;

namespace TechBlog.Services.Security;

public interface ICaptchaProvider
{
	Task<bool> VerifyAsync(IRequireCaptcha captcha);
}
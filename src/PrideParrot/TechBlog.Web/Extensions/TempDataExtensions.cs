using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using TechBlog.Web.Models;

namespace TechBlog.Web.Extensions;

public static class TempDataExtensions
{
	private static readonly string AlertKey = "Alert";

	public static ITempDataDictionary Set<T>(
		this ITempDataDictionary tempData, string key, T data)
	{
		var jsonData = JsonConvert.SerializeObject(data);

		tempData[key] = jsonData;

		return tempData;
	}

	public static T Get<T>(this ITempDataDictionary tempData, 
		string key, T defaultValue = default)
	{
		if (!tempData.TryGetValue(key, out var data) || data is not string jsonData)
		{
			return defaultValue;
		}

		if (string.IsNullOrWhiteSpace(jsonData))
		{
			return defaultValue;
		}

		try
		{
			return JsonConvert.DeserializeObject<T>(jsonData);
		}
		catch
		{
			return defaultValue;
		}
	}

	public static ITempDataDictionary SetAlertMessage(
		this ITempDataDictionary tempData,
		string message,
		string type = AlertMessage.AlertType.Success,
		string title = "Success")
	{
		return SetAlertMessage(tempData, new AlertMessage(title, message, type));
	}

	public static ITempDataDictionary SetAlertMessage(
		this ITempDataDictionary tempData, AlertMessage alertMessage)
	{
		return tempData.Set(AlertKey, alertMessage);
	}

	public static AlertMessage GetAlertMessage(this ITempDataDictionary tempData)
	{
		return tempData.Get<AlertMessage>(AlertKey);
	}
}
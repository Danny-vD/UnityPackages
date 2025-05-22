using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VDFramework.Extensions;

namespace UtilityPackage.Networking
{
	/// <summary>
	/// GET a dad joke from https://icanhazdadjoke.com/ <br/>
	/// A simple class that shows how to use a <see cref="httpClient"/> to send a <see cref="HttpMethod.Get"/> request with an accept header
	/// </summary>
	/// <remarks>
	/// Mostly for fun and for future reference<br/>
	/// But could be used for example for loading screen messages
	/// </remarks>
	public static class DadJokeGetter
	{
		private static readonly string[] failJoke = new string[]
		{
			"Why did the HTTP client fail to get a joke? Because it couldn't fetch one.",
			"There's supposed to be a joke here... but the HTTP client didn't GET it.",
		};

		// No need to manually dispose, it will automatically be disposed when the application ends
		private static readonly HttpClient httpClient = new HttpClient()
		{
			BaseAddress = new Uri("https://icanhazdadjoke.com/"),
		};

		public static async Task<string> GetDadJokeStringAsync()
		{
			try
			{
				// No need for a further URI, the website uses the base URI and differs output based on the Accept header
				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, string.Empty);
				requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

				HttpResponseMessage responseMessage = await SendHttpRequest(requestMessage);

				string output = await responseMessage.Content.ReadAsStringAsync();

				return output;
			}
			catch
			{
				return failJoke.GetRandomElement();
			}
		}

		public static async Task<string> GetDadJokeJSONAsync()
		{
			try
			{
				// No need for a further URI, the website uses the base URI and differs output based on the Accept header
				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, string.Empty);
				requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage responseMessage = await SendHttpRequest(requestMessage);

				string output = await responseMessage.Content.ReadAsStringAsync();

				return output;
			}
			catch
			{
				return GetFailJokeJSON();
			}
		}

		public static string GetDadJokeString()
		{
			return Task.Run(GetDadJokeStringAsync).Result;
		}

		public static string GetDadJokeJson()
		{
			return Task.Run(GetDadJokeJSONAsync).Result;
		}

		private static Task<HttpResponseMessage> SendHttpRequest(HttpRequestMessage requestMessage)
		{
			Task<HttpResponseMessage> response = httpClient.SendAsync(requestMessage);
			return response;
		}

		private static string GetFailJokeJSON()
		{
			return @$"{{
  ""id"": ""REQUEST_FAILED"",
  ""joke"": ""{failJoke.GetRandomElement()}"",
  ""status"": 200
}}";
		}
	}
}
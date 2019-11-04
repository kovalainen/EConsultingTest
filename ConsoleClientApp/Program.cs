using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Http.Headers;

namespace ConsoleClientApp
{
	class Program
	{
		private static readonly string uri = "https://localhost:44376/api/values";
		private static readonly string format = "yyyy-MM-dd";
		private static string token = "";

		static async Task Main(string[] args)
		{
			Interval interval;
			bool isExit = false;
			while (!isExit)
			{
				Console.Clear();
				Console.WriteLine("1 - Create Interval");
				Console.WriteLine("2 - Select intervals");
				Console.WriteLine("3 - Login");
				Console.WriteLine("4 - Exit");
				char selectedItem = Console.ReadKey().KeyChar;
				Console.Clear();
				switch (selectedItem)
				{
					case '1':
						{
							interval = ReadDate();
							Task CreateIntervalTask = CreateInterval(interval);
							Console.Clear();
							Console.WriteLine("Processing request");
							await CreateIntervalTask;
							break;
						}
					case '2':
						{
							interval = ReadDate();
							Task<IEnumerable<Interval>> SelectIntervalsTask = SelectIntervals(interval);
							Console.Clear();
							Console.WriteLine("Processing request");
							IEnumerable<Interval> intervals = await SelectIntervalsTask;
							Console.Clear();
							PrintIntervals(intervals);
							Console.ReadKey();
							break;
						}
					case '3':
						{
							Task LoginTask = Login();
							Console.Clear();
							Console.WriteLine("Processing request");
							await LoginTask;
							break;
						}
					case '4':
						{
							isExit = true;
							break;
						}
				}
			}
		}

		static Interval ReadDate()
		{
			DateTime startDate, endDate;

			Console.WriteLine($"Enter start date in format: {format}");
			string startDateString = Console.ReadLine();
			while (!DateTime.TryParseExact(startDateString, format, null, System.Globalization.DateTimeStyles.None, out startDate))
			{
				Console.WriteLine($"Incorrect start date input, date should be {format}");
				startDateString = Console.ReadLine();
			}

			Console.WriteLine($"Enter end date in format: {format}");
			string endDateString = Console.ReadLine();
			while (!DateTime.TryParseExact(endDateString, format, null, System.Globalization.DateTimeStyles.None, out endDate))
			{
				Console.WriteLine($"Incorrect end date input, date should be {format}");
				endDateString = Console.ReadLine();
			}

			if (startDate > endDate)
			{
				DateTime temp = startDate;
				startDate = endDate;
				endDate = temp;
			}

			Interval interval = new Interval()
			{
				Start = startDate,
				End = endDate,
			};
			return interval;
		}

		static async Task CreateInterval(Interval interval)
		{

			using (HttpClient client = new HttpClient())
			{
				try
				{
					string jsonInterval = JsonConvert.SerializeObject(interval);
					StringContent stringContent = new StringContent(jsonInterval, Encoding.UTF8, "application/json");
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
					var response = await client.PostAsync($"{uri}/addinterval", stringContent);
					response.EnsureSuccessStatusCode();
					string result = await response.Content.ReadAsStringAsync();

					Console.Clear();
					Console.WriteLine("Interval added");
					Console.ReadKey();
				}
				catch (HttpRequestException ex)
				{
					Console.Clear();
					Console.WriteLine(ex.Message);
					Console.ReadKey();
				}
			}
		}

		static async Task<IEnumerable<Interval>> SelectIntervals(Interval interval)
		{
			using (HttpClient client = new HttpClient())
			{
				IEnumerable<Interval> intervals = null;
				try
				{
					string jsonInterval = JsonConvert.SerializeObject(interval);
					StringContent stringContent = new StringContent(jsonInterval, Encoding.UTF8, "application/json");
					var response = await client.PostAsync($"{uri}/getintervalslist", stringContent);
					response.EnsureSuccessStatusCode();
					string result = await response.Content.ReadAsStringAsync();
					intervals = JsonConvert.DeserializeObject<IEnumerable<Interval>>(result);
				}
				catch (HttpRequestException ex)
				{
					Console.WriteLine(ex.Message);
					Console.ReadKey();
				}
				return intervals;
			}
		}

		static async Task Login()
		{
			Console.WriteLine("Enter login");
			string login = Console.ReadLine();
			Console.WriteLine("Enter password");
			string password = Console.ReadLine();
			User user = new User { Login = login, Password = password };
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string jsonUser = JsonConvert.SerializeObject(user);
					StringContent stringContent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
					var response = await client.PostAsync($"{uri}/token", stringContent);
					response.EnsureSuccessStatusCode();
					string result = await response.Content.ReadAsStringAsync();
					AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(result);
					token = authResponse.AccessToken;
				}
				catch (HttpRequestException ex)
				{
					Console.WriteLine(ex.Message);
					Console.ReadKey();
				}
			}
		}

		static void PrintIntervals(IEnumerable<Interval> intervals)
		{
			try
			{
				if (intervals.Count() == 0)
				{
					Console.WriteLine("List is empty");
				}
				foreach (var inter in intervals)
				{
					Console.WriteLine($"{inter.Start.ToString("yyyy-MM-dd")} - {inter.End.ToString("yyyy-MM-dd")}");
				}
			}
			catch (ArgumentNullException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}

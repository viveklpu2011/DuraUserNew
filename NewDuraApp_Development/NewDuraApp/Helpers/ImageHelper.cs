
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using Xamarin.Forms;

namespace NewDuraApp.Helpers
{
	public class ImageHelper
	{
		public static async Task<byte[]> GetStreamFormResource(string image)
		{
			var assembly = typeof(ImageHelper).GetTypeInfo().Assembly; // you can replace "this.GetType()" with "typeof(MyType)", where MyType is any type in your assembly.
			byte[] buffer;
			string fullpath = "NewDuraApp.Images." + image;
			Stream s = assembly.GetManifestResourceStream(fullpath);
			long length = s.Length;
			buffer = new byte[length];
			await s.ReadAsync(buffer, 0, (int)length);
			return buffer;
		}

		public static async Task<byte[]> GetImageFromUrl(string url)
		{
			if (Device.RuntimePlatform == Device.Android) {
				using (var client = new HttpClient(DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler())) {
					using (var response = await client.GetAsync(url)) {
						if (response.StatusCode == HttpStatusCode.Forbidden) {
							return null;
						} else {
							return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
						}

					}
				}
			} else {
				HttpClientHandler clientHandler = new HttpClientHandler();
				clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
				using (var client = new HttpClient(clientHandler)) {
					using (var response = await client.GetAsync(url)) {
						if (response.StatusCode == HttpStatusCode.Forbidden) {
							return null;
						} else {
							return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
						}

					}
				}
			}

		}
		public static async Task<Stream> GetStreamFromUrl(string url)
		{
			var result = await DownloadImageAsync(url);
			return new MemoryStream(result);
		}
		public static async Task<byte[]> DownloadImageAsync(string imageUrl)
		{
			HttpClient _client;
			var httpClientHandler = new HttpClientHandler();

			httpClientHandler.ServerCertificateCustomValidationCallback =
			(message, cert, chain, errors) => { return true; };

			_client = new HttpClient(httpClientHandler);
			try {
				using (var httpResponse = await _client.GetAsync(imageUrl)) {
					if (httpResponse.StatusCode == HttpStatusCode.OK) {
						return await httpResponse.Content.ReadAsByteArrayAsync();
					} else {
						//Url is Invalid
						return null;
					}
				}
			} catch (Exception e) {
				//Handle Exception
				return null;
			}
		}

		public static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			MemoryStream ms = new MemoryStream();
			int read;
			while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
				ms.WriteAsync(buffer, 0, read);
			}
			return ms.ToArray();
		}
		public static byte[] ReadToEnd(System.IO.Stream stream)
		{
			long originalPosition = 0;

			if (stream.CanSeek) {
				originalPosition = stream.Position;
				stream.Position = 0;
			}

			try {
				byte[] readBuffer = new byte[4096];

				int totalBytesRead = 0;
				int bytesRead;

				while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0) {
					totalBytesRead += bytesRead;

					if (totalBytesRead == readBuffer.Length) {
						int nextByte = stream.ReadByte();
						if (nextByte != -1) {
							byte[] temp = new byte[readBuffer.Length * 2];
							Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
							Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
							readBuffer = temp;
							totalBytesRead++;
						}
					}
				}

				byte[] buffer = readBuffer;
				if (readBuffer.Length != totalBytesRead) {
					buffer = new byte[totalBytesRead];
					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
				}
				return buffer;
			} finally {
				if (stream.CanSeek) {
					stream.Position = originalPosition;
				}
			}
		}

		public static ImageSource GetImageNameFromResource(string IconSource)
		{
			return ImageSource.FromResource(string.Format("NewDuraApp.Images.{0}", IconSource));
		}
		public static bool CheckImageExistOnServer(string url)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = "HEAD";
			bool exists;
			try {
				request.GetResponse();
				exists = true;
			} catch {
				exists = false;
			}
			return exists;
		}
		//public static void SafeClear<T>(this ObservableCollection<T> observableCollection)
		//{
		//    if (!MainThread.IsMainThread)
		//    {
		//        Device.BeginInvokeOnMainThread(() =>
		//        {
		//            while (observableCollection.Any())
		//            {
		//                ObservableCollection.RemoveAt(0);
		//            }
		//        });
		//    }
		//    else
		//    {
		//        while (observableCollection.Any())
		//        {
		//            ObservableCollection.RemoveAt(0);
		//        }
		//    }
		//}
	}

}

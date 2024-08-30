using AutoUploader.Selenium.Suggestions.Structure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoUploader.Selenium.Suggestions
{
    class GetSuggestions
    {
        private static readonly string url = "https://www.similarsongsfinder.com/api/getSong";

        public static async Task<SongResponse> ByPayloadAsync(string payload, TextBox tb)
        {
            try
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText("[INFO] Auto Behaviour - Start sending suggestion request...\n");
                    tb.AppendText($"[INFO] Auto Behaviour - Link {payload}\n");
                });

                using (HttpClient client = new HttpClient())
                {
                    HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        tb.Dispatcher.Invoke(() =>
                        {
                            tb.AppendText($"[ERROR] API response error: {response.StatusCode} - {response.ReasonPhrase}\n");
                            tb.AppendText($"[ERROR] Response content: {errorContent}\n");
                        });
                        return default;
                    }

                    string responseData = await response.Content.ReadAsStringAsync();

                    tb.Dispatcher.Invoke(() =>
                    {
                        tb.AppendText("[INFO] Suggestion request completed successfully.\n");
                    });

                    SongResponse convertedResponse = JsonConvert.DeserializeObject<SongResponse>(responseData);
                    return convertedResponse;
                }
            }
            catch (HttpRequestException httpEx)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERROR] HTTP request error: {httpEx.Message}\n");
                });
                return default;
            }
            catch (JsonException jsonEx)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERROR] JSON parsing error: {jsonEx.Message}\n");
                });
                return default;
            }
            catch (Exception ex)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERROR] An unexpected error occurred: {ex.Message}\n");
                });
                return default;
            }
        }

    }
}
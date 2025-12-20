using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SporSalonuYonetimi.Controllers
{
    public class AIController : Controller
    {
        // Google API Key'in burada kalacak
        private readonly string _geminiApiKey = "AIzaSyD_bqbI_qW4RnDngBQ9MHF5MgXAnqu_Rxw";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan(int age, double weight, double height, string goal, string gender, IFormFile userImage)
        {
            // 1. RESİM OLUŞTURMA (Pollinations - Gelecekteki hali)
            string imagePrompt = $"fitness photo of a {age} years old {gender}, {goal}, athletic body, gym environment, realistic lighting, 4k";
            string encodedPrompt = System.Net.WebUtility.UrlEncode(imagePrompt);
            string generatedImageUrl = $"https://image.pollinations.ai/prompt/{encodedPrompt}?width=512&height=512&nologo=true";

            // 2. METİN VE ANALİZ OLUŞTURMA (Google Gemini - Vision)
            string planText = "Analiz yapılamadı.";
            
            // Kullanıcı resim yükledi mi kontrolü
            string base64Image = "";
            string mimeType = "";

            if (userImage != null && userImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await userImage.CopyToAsync(ms);
                    byte[] imageBytes = ms.ToArray();
                    base64Image = Convert.ToBase64String(imageBytes);
                    mimeType = userImage.ContentType; // örn: image/jpeg
                }
            }

            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiApiKey}";

                    // Google'a gidecek mesajı hazırlıyoruz
                    var partsList = new List<object>();

                    // A) Metin Sorusu
                    string promptText = $"Ben {age} yaşında, {weight} kilo, {height} cm boyunda bir {gender} bireyim. Hedefim: {goal}. ";
                    
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        promptText += "Eklediğim fotoğraftaki vücut tipini analiz et ve buna göre bana kişisel bir diyet ve egzersiz programı hazırla.";
                    }
                    else
                    {
                        promptText += "Bana uygun diyet ve egzersiz programı hazırla.";
                    }
                    
                    promptText += " Cevabı Türkçe ver, samimi bir koç gibi konuş ve listele.";

                    partsList.Add(new { text = promptText });

                    // B) Resim (Varsa ekle)
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        partsList.Add(new 
                        { 
                            inline_data = new 
                            { 
                                mime_type = mimeType, 
                                data = base64Image 
                            } 
                        });
                    }

                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new { parts = partsList }
                        }
                    };

                    string jsonBody = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var jsonNode = JsonNode.Parse(responseString);
                        planText = jsonNode?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
                    }
                    else
                    {
                        planText = "Hata: Google API resmini okuyamadı veya kota doldu.";
                    }
                }
            }
            catch (Exception ex)
            {
                planText = "Sunucu Hatası: " + ex.Message;
            }

            ViewBag.Plan = planText;
            ViewBag.ImageUrl = generatedImageUrl;
            
            // Form verilerini geri doldur
            ViewBag.Age = age;
            ViewBag.Weight = weight;
            ViewBag.Height = height;
            ViewBag.Goal = goal;

            return View("Index");
        }
    }
}
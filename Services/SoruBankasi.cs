using System.Text.Json;
using BilgisayarAkademisi.Models;

namespace BilgisayarAkademisi.Services;

// İçerik App_Data/sorular.json dosyasında; controller'da soru metni bulunmaz.
public class SoruBankasi
{
    public IReadOnlyList<Etap> Etaplar { get; }

    public SoruBankasi(IWebHostEnvironment environment)
    {
        string yol = Path.Combine(environment.ContentRootPath, "App_Data", "sorular.json");
        var etaplar = JsonSerializer.Deserialize<List<Etap>>(File.ReadAllText(yol))
            ?? throw new InvalidDataException("Soru dosyası okunamadı.");

        bool gecersiz = etaplar.Count != 3 ||
            !etaplar.Select(e => e.No).OrderBy(n => n).SequenceEqual(new[] { 1, 2, 3 }) ||
            etaplar.Any(e => e.Sorular.Count != 15 || e.Sorular.Any(s =>
                string.IsNullOrWhiteSpace(s.Metin) || string.IsNullOrWhiteSpace(s.Aciklama) ||
                s.Secenekler.Count != 4 || s.Secenekler.Distinct().Count() != 4 ||
                s.DogruSecenekIndex < 0 || s.DogruSecenekIndex > 3)) ||
            etaplar.SelectMany(e => e.Sorular).Select(s => s.Id).Distinct().Count() != 45;

        if (gecersiz)
            throw new InvalidDataException("3 etap, her etapta 15 benzersiz soru ve her soruda 4 seçenek bulunmalıdır.");

        Etaplar = etaplar.OrderBy(e => e.No).ToList().AsReadOnly();
    }

    public Etap? Bul(int no) => Etaplar.FirstOrDefault(e => e.No == no);
}

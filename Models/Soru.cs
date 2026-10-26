namespace BilgisayarAkademisi.Models;

public class Soru
{
    public int Id { get; set; }

    public string Metin { get; set; } = string.Empty;

    public List<string> Secenekler { get; set; } = new();

    // Seçenekler 0'dan başlar: A=0, B=1, C=2, D=3.
    public int DogruSecenekIndex { get; set; }

    public string Aciklama { get; set; } = string.Empty;
}
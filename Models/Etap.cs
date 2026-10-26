namespace BilgisayarAkademisi.Models;

public class Etap
{
    public int No { get; set; }
    public string Baslik { get; set; } = "";
    public string Zorluk { get; set; } = "";
    public string Aciklama { get; set; } = "";
    public List<Soru> Sorular { get; set; } = new();
}

namespace BilgisayarAkademisi.Models;

// View yalnızca görüntülenecek bilgileri alır. Cevap anahtarı HTML'e yazılmaz.
public class SoruEkraniViewModel
{
    public int EtapNo { get; set; }
    public string EtapBasligi { get; set; } = "";
    public string EtapAciklamasi { get; set; } = "";
    public string Zorluk { get; set; } = "";
    public string DenemeId { get; set; } = "";
    public int SoruId { get; set; }
    public string SoruMetni { get; set; } = "";
    public int SoruNumarasi { get; set; }
    public int ToplamSoru { get; set; }
    public int CevaplananSoru { get; set; }
    public int Can { get; set; }
    public int DogruSayisi { get; set; }
    public bool Cevaplandi { get; set; }
    public bool DogruMu { get; set; }
    public bool Tamamlandi { get; set; }
    public string Aciklama { get; set; } = "";
    public string DogruCevap { get; set; } = "";
    public List<SecenekViewModel> Secenekler { get; set; } = new();
    public List<EtapKartViewModel> Etaplar { get; set; } = new();
}

public class SecenekViewModel
{
    public int Sira { get; set; }
    public string Metin { get; set; } = "";
    public bool Secildi { get; set; }
    public bool Dogru { get; set; }
}

public class EtapKartViewModel
{
    public int No { get; set; }
    public string Baslik { get; set; } = "";
    public string Zorluk { get; set; } = "";
    public bool Acik { get; set; }
    public bool Tamamlandi { get; set; }
    public int Cevaplanan { get; set; }
}

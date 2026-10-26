namespace BilgisayarAkademisi.Models;

// Her etap kendi ilerlemesini saklar. Etap değiştirmek cevapları silmez.
public class EgitimOturumu
{
    public int Surum { get; set; } = 1;
    public int AktifEtap { get; set; } = 1;
    public HashSet<int> TamamlananEtaplar { get; set; } = new();
    public Dictionary<int, EtapDurumu> Etaplar { get; set; } = new();
}

public class EtapDurumu
{
    public string DenemeId { get; set; } = Guid.NewGuid().ToString("N");
    public int Sira { get; set; }
    public int Can { get; set; } = 5;
    public int DogruSayisi { get; set; }
    public bool Tamamlandi { get; set; }
    public Dictionary<int, List<int>> SecenekSiralari { get; set; } = new();
    public Dictionary<int, int> Cevaplar { get; set; } = new();
}

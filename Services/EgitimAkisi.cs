using System.Security.Cryptography;
using System.Text.Json;
using BilgisayarAkademisi.Models;

namespace BilgisayarAkademisi.Services;

public class EgitimAkisi
{
    private const string OturumAnahtari = "Akademi.Egitim.v1";
    private readonly SoruBankasi _banka;
    public EgitimAkisi(SoruBankasi banka) => _banka = banka;

    public SoruEkraniViewModel Ekran(ISession session, int? etapNo = null)
    {
        var oturum = Oku(session);
        if (etapNo.HasValue && AcikMi(oturum, etapNo.Value))
            oturum.AktifEtap = etapNo.Value;

        var etap = _banka.Bul(oturum.AktifEtap)!;
        if (!oturum.Etaplar.TryGetValue(etap.No, out var durum))
        {
            durum = YeniDeneme(etap);
            oturum.Etaplar[etap.No] = durum;
        }

        var soru = etap.Sorular[durum.Sira];
        bool cevaplandi = durum.Cevaplar.TryGetValue(soru.Id, out int secilen);
        var siralama = durum.SecenekSiralari[soru.Id];

        var model = new SoruEkraniViewModel
        {
            EtapNo = etap.No,
            EtapBasligi = etap.Baslik,
            EtapAciklamasi = etap.Aciklama,
            Zorluk = etap.Zorluk,
            DenemeId = durum.DenemeId,
            SoruId = soru.Id,
            SoruMetni = soru.Metin,
            SoruNumarasi = durum.Sira + 1,
            ToplamSoru = etap.Sorular.Count,
            CevaplananSoru = durum.Cevaplar.Count,
            Can = durum.Can,
            DogruSayisi = durum.DogruSayisi,
            Cevaplandi = cevaplandi,
            DogruMu = cevaplandi && secilen == soru.DogruSecenekIndex,
            Tamamlandi = durum.Tamamlandi,
            Aciklama = cevaplandi ? soru.Aciklama : "",
            DogruCevap = cevaplandi ? soru.Secenekler[soru.DogruSecenekIndex] : "",
            Secenekler = siralama.Select((asilIndex, gorunenIndex) => new SecenekViewModel
            {
                Sira = gorunenIndex,
                Metin = soru.Secenekler[asilIndex],
                Secildi = cevaplandi && secilen == asilIndex,
                Dogru = cevaplandi && soru.DogruSecenekIndex == asilIndex
            }).ToList(),
            Etaplar = _banka.Etaplar.Select(e => new EtapKartViewModel
            {
                No = e.No,
                Baslik = e.Baslik,
                Zorluk = e.Zorluk,
                Acik = AcikMi(oturum, e.No),
                Tamamlandi = oturum.Etaplar.TryGetValue(e.No, out var d) && d.Tamamlandi,
                Cevaplanan = oturum.Etaplar.TryGetValue(e.No, out var p) ? p.Cevaplar.Count : 0
            }).ToList()
        };
        Kaydet(session, oturum);
        return model;
    }

    public string? Cevapla(ISession session, int etapNo, int soruId, string denemeId, int? secilenSecenek)
    {
        var oturum = Oku(session);
        if (!GecerliIstegiBul(oturum, etapNo, soruId, denemeId, out var etap, out var durum))
            return null;
        if (durum.Can <= 0 || durum.Tamamlandi || durum.Cevaplar.ContainsKey(soruId))
            return null;
        if (!secilenSecenek.HasValue || secilenSecenek.Value < 0 || secilenSecenek.Value > 3)
            return "Devam etmek için bir cevap seç.";

        int asilIndex = durum.SecenekSiralari[soruId][secilenSecenek.Value];
        durum.Cevaplar[soruId] = asilIndex;
        if (asilIndex == etap.Sorular[durum.Sira].DogruSecenekIndex)
            durum.DogruSayisi++;
        else
            durum.Can--;

        Kaydet(session, oturum);
        return null;
    }

    public void Sonraki(ISession session, int etapNo, int soruId, string denemeId)
    {
        var oturum = Oku(session);
        if (!GecerliIstegiBul(oturum, etapNo, soruId, denemeId, out var etap, out var durum) ||
            durum.Can <= 0 || durum.Tamamlandi || !durum.Cevaplar.ContainsKey(soruId))
            return;
        if (durum.Sira + 1 == etap.Sorular.Count)
        {
            durum.Tamamlandi = true;
            oturum.TamamlananEtaplar.Add(etapNo);
        }
        else
            durum.Sira++;
        Kaydet(session, oturum);
    }

    public void YenidenBaslat(ISession session, int etapNo, string denemeId)
    {
        var oturum = Oku(session);
        if (AcikMi(oturum, etapNo) && oturum.Etaplar.TryGetValue(etapNo, out var eski) &&
            eski.DenemeId == denemeId)
        {
            oturum.Etaplar[etapNo] = YeniDeneme(_banka.Bul(etapNo)!);
            oturum.AktifEtap = etapNo;
            Kaydet(session, oturum);
        }
    }

    private EgitimOturumu Oku(ISession session)
    {
        string? json = session.GetString(OturumAnahtari);
        if (json != null)
        {
            var mevcut = JsonSerializer.Deserialize<EgitimOturumu>(json);
            if (mevcut is { Surum: 1 }) return mevcut;
        }
        var yeni = new EgitimOturumu();
        yeni.Etaplar[1] = YeniDeneme(_banka.Bul(1)!);
        return yeni;
    }

    private static void Kaydet(ISession session, EgitimOturumu oturum) =>
        session.SetString(OturumAnahtari, JsonSerializer.Serialize(oturum));

    private bool AcikMi(EgitimOturumu oturum, int no) =>
        _banka.Bul(no) != null && (no == 1 || oturum.TamamlananEtaplar.Contains(no - 1));

    private bool GecerliIstegiBul(EgitimOturumu oturum, int etapNo, int soruId,
        string denemeId, out Etap etap, out EtapDurumu durum)
    {
        etap = _banka.Bul(etapNo)!;
        durum = null!;
        return AcikMi(oturum, etapNo) && oturum.AktifEtap == etapNo &&
            oturum.Etaplar.TryGetValue(etapNo, out durum!) &&
            durum.DenemeId == denemeId && etap.Sorular[durum.Sira].Id == soruId;
    }

    private static EtapDurumu YeniDeneme(Etap etap)
    {
        var durum = new EtapDurumu();
        // A/B/C/D doğru cevap dağılımı 4/4/4/3. Sabit bir cevap sırası yoktur.
        int eksikHarf = RandomNumberGenerator.GetInt32(4);
        var hedefler = Enumerable.Range(0, 4)
            .SelectMany(i => Enumerable.Repeat(i, i == eksikHarf ? 3 : 4)).ToList();
        do { Karistir(hedefler); }
        while (Enumerable.Range(2, hedefler.Count - 2)
            .Any(i => hedefler[i] == hedefler[i - 1] && hedefler[i] == hedefler[i - 2]));

        for (int i = 0; i < etap.Sorular.Count; i++)
        {
            var soru = etap.Sorular[i];
            var yanlislar = Enumerable.Range(0, 4)
                .Where(n => n != soru.DogruSecenekIndex).ToList();
            Karistir(yanlislar);
            yanlislar.Insert(hedefler[i], soru.DogruSecenekIndex);
            durum.SecenekSiralari[soru.Id] = yanlislar;
        }
        return durum;
    }

    private static void Karistir(List<int> liste)
    {
        for (int i = liste.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (liste[i], liste[j]) = (liste[j], liste[i]);
        }
    }
}

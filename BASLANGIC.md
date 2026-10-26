# Bilgisayar Akademisi

## Nasıl açılır

1. ZIP dosyasını yeni bir klasöre çıkar. Eski projenin üzerine yazmana gerek yok.
2. BilgisayarAkademisi klasöründeki BilgisayarAkademisi.csproj dosyasını Visual Studio ile aç.
3. ASP.NET ve web geliştirme iş yükü ve .NET 8 hedefleme desteği bulunan Visual Studio kullan.
4. Üstte http veya https profilini seç. Ctrl + F5 ile çalıştır.
5. Ana sayfadan Öğrenmeye başla düğmesine bas.

Bu proje harici NuGet paketi, ücretli servis veya veritabanı gerektirmez.
Terminalden çalıştırmak istersen proje klasöründe `dotnet run --launch-profile http` komutunu kullan.

## Eklenenler

- Özel CSS tasarımı ve telefon ekranlarına uyumlu soru ekranı.
- Dolu ve boş SVG kalplerle gösterilen 5 can.
- 1. kısım: Parçaları Tanı — kolay, 15 soru.
- 2. kısım: Bağlantıları Keşfet — orta, 15 soru.
- 3. kısım: Sorunları Çöz — zor, 15 soru.
- Her etabı tamamlayınca sonraki etap açılır; yeni etap 5 canla başlar.
- Her yeni denemede şıklar karışır. Bir etapta doğru cevapların harf dağılımı 4/4/4/3'tür.
- Aynı deneme içinde sayfa yenilendiğinde şıkların sırası değişmez.
- Cevap sonrası açıklama, etap sonucu ve tekrar çözme seçeneği.
- Can 0'a düştüğünde /Egitim/Oyun sayfasına otomatik geçiş.

## Oyun sayfasının mevcut durumu

Bu sürümde oynanabilir mini oyun yoktur. Oyun sayfası ve oyun için ayrılan görsel alan hazırdır.
Bu sayfada son yanlış cevabın açıklaması gösterilir. Etabı yeniden dene düğmesi mevcut etabı
baştan başlatır, bu etabın cevaplarını sıfırlar ve 5 can verir. Önceden açılmış etaplar korunur.
İleride toplama oyunu bu sayfaya eklenecek.

Son isteğe uygun olarak oyun sayfasına geçiş eşiği 0 candır; 2 can değildir.

## Sorular nerede

Controller'da soru metni, seçenek listesi veya cevap anahtarı bulunmaz.

- App_Data/sorular.json: 45 sorunun içeriği ve doğru cevapları.
- Models/Soru.cs ve Etap.cs: verilerin yapısı.
- Services/SoruBankasi.cs: soru dosyasını okur ve doğrular.
- Services/EgitimAkisi.cs: cevap, can, şık sırası ve etap kurallarını yönetir.
- Controllers/EgitimController.cs: sayfa isteklerini ilgili işleme yönlendirir.
- Views/Egitim/Index.cshtml: soru ve seçeneklerin HTML/Razor gösterimi.
- Views/Egitim/Oyun.cshtml: oyun için ayrılan sayfa.
- Views/Shared/_Kalpler.cshtml: kalplerin HTML/SVG gösterimi.
- wwwroot/css/site.css: renkler, kartlar, şıklar ve mobil görünüm.

MVC'de View, modelden gelen soruyu HTML olarak gösterir. Soru verisini ve doğru cevap anahtarını
View'ın içine sabitlemek yerine ayrı veri dosyasında tutmak, gösterim ile içeriği ayırır.
App_Data, wwwroot dışında kaldığı için soru dosyası statik dosya olarak sunulmaz.
Cevap kontrolü C# tarafında yapılır; cevap verilmeden doğru cevap HTML'e yazılmaz.

## Bir soru nasıl düzenlenir

Visual Studio'da App_Data/sorular.json dosyasını aç. Alanlar:

- Id: Sorunun benzersiz numarası. Mevcut 1–45 numaralarını koru.
- Metin: Soru cümlesi.
- Secenekler: Birbirinden farklı dört cevap.
- DogruSecenekIndex: Dosyadaki doğru seçeneğin konumu; 0 ilk, 1 ikinci, 2 üçüncü, 3 dördüncü.
- Aciklama: Cevaptan sonra gösterilecek kısa öğretici açıklama.

Ekrandaki A/B/C/D sırası dosyadaki sırayla aynı olmak zorunda değildir: uygulama sırayı
karıştırır ve cevabı buna göre kontrol eder. İçeriği değiştirdikten sonra uygulamayı yeniden başlat.
Bu sürüm 3 etap ve her etapta 15 soru olacak şekilde doğrulama yapar.

## İlerlemenin saklanması

Bu başlangıç sürümü kullanıcı hesabı veya kalıcı veritabanı kullanmaz. İlerleme sunucudaki
session'da saklanır. Sayfayı yenilemek canı sıfırlamaz. Ancak uygulama kapanırsa, session
çerezi kaybolursa veya 2 saat işlem yapılmazsa ilerleme kaybolabilir. Kalıcı öğrenci takibi
sonraki bir geliştirme olarak eklenebilir.

App_Data/keys klasörü çalıştırmada otomatik oluşur. Bu klasör çalışma zamanı anahtarlarını içerir;
paylaşılmamalıdır. Dağıtılan ZIP'e bin, obj ve keys klasörleri eklenmemiştir.

## Kontroller

Proje derlendi. 152 otomatik C# ve Razor kontrolü geçti. Kontroller; 45 soru, cevap kontrolü,
can kaybı, tekrar gönderim, geçersiz seçim, eski form, şık dağılımı, etap kilitleri, etap
sonuçları, yeniden deneme, son soruda can bitmesi ve oyun sayfasına yönlendirmeyi kapsar.
Ana sayfa, soru, doğru/yanlış geri bildirimi, üç etap sonucu ve oyun sayfası Razor motoruyla
HTML'e üretildi. Çalışma ortamındaki yerel ağ ve tarayıcı erişim kısıtı nedeniyle canlı
tarayıcı etkileşimi ve ekran görüntüsüyle görsel kontrol tamamlanamadı.

## Teknik içerik için başvuru kaynakları

- Kingston, bellek nesilleri ve uyumluluk: https://www.kingston.com/unitedkingdom/en/blog/pc-performance/ddr-memory-generation-differences
- Kingston, SATA ve M.2 SSD soruları: https://www.kingston.com/en/ssd/ssd-faq
- AMD, işlemci desteği ve BIOS: https://www.amd.com/en/resources/support-articles/faqs/cpu-99.html

Sorular özgün eğitim içeriğidir. Gerçek donanım montajında modelin üretici kılavuzu esas alınmalıdır.

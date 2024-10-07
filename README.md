Giriş:
Bu doküman, dinamik bir konfigürasyon yapısına sahip bir kütüphane uygulamasının geliştirilmesini açıklamaktadır. Projenin amacı, uygulama yapılandırmalarının Redis ortamından okunarak web.config veya app.config dosyaları gibi geleneksel yapıların yerine kullanılabilir olmasıdır. Bu yapı, herhangi bir deployment, restart veya recycle gerektirmeden konfigürasyonların güncellenebilmesini sağlar.

Amaç:
Yazılan kütüphanenin hedefleri şunlardır:
Farklı uygulamalar için, örneğin "SERVICE-A" ve "SERVICE-B", ilgili yapılandırma kayıtlarının depolama ortamından dinamik olarak okunması.
Belirli tiplerdeki (string, boolean, integer, double) konfigürasyon değerlerinin okunması ve yönetilmesi.
Depolama ortamında yalnızca aktif olan konfigürasyonların getirilmesi.
Belirli aralıklarla yapılandırma kayıtlarının güncellenip güncellenmediğinin kontrol edilmesi ve yeni eklenen kayıtların sisteme dahil edilmesi.
Web, WCF, Web API gibi farklı proje türleri için entegre edilebilecek bir yapı.
Web arayüzü üzerinden yapılandırma kayıtlarının listelenmesi, güncellenmesi ve yeni kayıt eklenebilmesi.

Kütüphane Detayları:
Kütüphane, farklı servislerin yalnızca kendi konfigürasyon kayıtlarına erişebileceği bir yapı sunar. Bu yapı, dışarıya üç parametre ile initialize edilebilecek bir ConfigurationReader sınıfı sağlar:

var _configurationReader = new ConfigurationReader(applicationName, connectionString, refreshTimerIntervalInMs);
ApplicationName: Üzerinde çalışacağı uygulamanın adı. Bu parametre, ilgili servisin sadece kendi kayıtlarına erişebilmesini sağlar.
ConnectionString: Depolama ortamına erişim bilgileri.
RefreshTimerIntervalInMs: Depolama ortamındaki yapılandırma kayıtlarının belirli aralıklarla kontrol edileceği süre (milisaniye cinsinden).

Kütüphane, yapılandırma kayıtlarına aşağıdaki yöntemle erişim sağlar:
T GetValue<T>(string key);
Bu yöntem, belirli bir anahtar ismine karşılık gelen konfigürasyon değerini uygun tipte döndürür. Örnek kullanım:
var siteName = _configurationReader.GetValue<string>("SiteName");
Bu kullanım sonucunda, siteName değişkeni "boyner.com.tr" değerini alır.

Özellikler:
Dinamik Konfigürasyon Yükleme:
Kütüphane, redis ortamından kayıtları yükler ve bunları uygun tiplerde (int, string, boolean, double) döndürür.
Yalnızca IsActive değeri 1 olan kayıtlar döndürülür.
Periyodik Kontrol:
Kütüphane, verilen süre aralıklarıyla(5000ms) depolama ortamındaki değişiklikleri kontrol eder. Yeni eklenen kayıtlar veya güncellenen değerler varsa bu değişiklikleri sisteme dahil eder.

Bağlantı Hatası Yönetimi:
Kütüphane, depolama ortamına erişilemediği durumlarda son başarılı konfigürasyon kayıtları ile çalışmaya devam eder.

Web Arayüzü:
Web arayüzü üzerinden mevcut yapılandırma kayıtları listelenebilir, yeni kayıtlar eklenebilir ve mevcut kayıtlar güncellenebilir. Her yeni kayır eklendiğinde RabbitMQ kuyruğuna bir mesaj bırakılır ve
bu mesaj konfigürasyonu kullanan uygulama tarafından dinlenir; anasayfada mesaj olarak kaydın adı 10 saniye boyunca ekrana yazdırılır.
Kayıtlar isme göre filtrelenebilir.

Kullanım Senaryoları
Konfigürasyon Okuma:
Belirli bir servis için konfigürasyon değerlerini almak:
var maxItemCount = _configurationReader.GetValue<int>("MaxItemCount");

Web Arayüzü:
Kullanıcı arayüzünden konfigürasyon kayıtlarını listeleyebilir, düzenleyebilir veya yenilerini ekleyebilirsiniz. Arayüz, uygulama adına göre kayıtları filtreleyebilir.

Teknik Detaylar:
Kütüphane .NET 5 kullanılarak geliştirilmiştir.
Veritabanı bağlantısı ConnectionString parametresiyle sağlanır.
Kütüphane ID, Name, Type, Value, IsActive, ApplicationName alanlarını barındıran konfigürasyon kayıtları üzerinde çalışır.
Yapılandırma kayıtları yalnızca ilgili uygulama adı ile eşleşen kayıtları döner.
Kayıtlar belirli tiplerde olabilir: int, string, boolean, double.
Sonuç
Bu kütüphane, farklı uygulamaların yapılandırma ihtiyaçlarını dinamik olarak karşılayabilmekte ve web tabanlı bir arayüzle kolayca yönetilebilmektedir. Kullanıcılar, uygulamaların konfigürasyon kayıtlarını güncel tutabilir ve yeni eklenen kayıtları hızlı bir şekilde sisteme dahil edebilir.

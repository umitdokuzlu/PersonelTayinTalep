Personel Tayin Talep Sistemi

----KURULUM TALİMATLARI----

ÖNEMLİ NOT:veritabanı yolu projenin içerisinde :/PersonelTayinTalep/DatabaseBackup/DbPersonelTayin.bak

##--> Veritabanını Yükleme DatabaseBackup/DbPersonelTayin.bak dosyasını bilgisayarınıza indirin. 
SQL Server Management Studio 18 ile: databases - sağ tıkla - Geri Yükle - Veritabanı seçin. "Aygıttan" seçeneğini işaretleyin ve .bak dosyasını gösterin. Veritabanı adı: DbPersonelTayin olarak ayarlayın.

Projeyi Çalıştırma Visual Studio 2022 ile açın. 
appsettings.json içinde DbPersonelTayin veritabanı ile bağlantılı olup olmadığına bakın: appsettings.json dosyasının içindeki veritabanı bağlantısından emin olun server kendi server adınız olabilir deneyin database doğru yazıldığından emin olun:

"ConnectionStrings": { "DefaultConnection": "Server=.;Database=DbPersonelTayin;Trusted_Connection=True;" }

projeyi kullanırken dikkat edilmesi gerekenler:


##--> projeye admin ile giriş yapmak için Admin Kullanıcı bilgileri bu şekilde-- Sicil no: 999999 şifre: Admin123

Not:Admin ile personel ekleyebilirsiniz, personel silebilirsiniz, personelin yaptığı tayin taleplerini görebilir ve silebilirsiniz.


##--> Ben bir tane de standart kullanıcı oluşturdum ona bakmak isterseniz bu sicil ve şifre ile girebilirsiniz: sicil no: 317740 şifre: 123456

Kendiniz de admin üzerinden kullanıcı oluşturabilirsiniz.


##-->> NOT: - projenin düzgün çalışabilmesi için 6.0 SDK yüklü olmalı. SQL Server management 18 kurulmuş ve çalışıyor olmalı.

###-->> NOT: lOG DOSYASINA ULAŞMAK İÇİN PROJENİN İÇİNDE Kİ PersonelTayinTalep\PersonelTayinTalep\Logs KLASÖRÜNDE OLUŞAN .TXT DOSYASINDAN GEREKLİ LOGLARI GÖREBİLİRSİNİZ.

###-->> Kullanılan Kütüphaneler ve Teknolojiler:
-.Net Core 6.0
-Entity Framework Core-> ORM için yani veritabanı ilişkileri yönetmek için
-SQL Server
-Serilog ->Loglama için
-ClosedXML ->Excel dışa aktarma için
-AspNetCore.Session -> Oturum yönetimi için
-AspNetCore.MVC -> UI katmanı için

###-->>Projede N Katmanlı Mimari kullanılmıştır
1)Entity -> nesneler için
2)DataAccess -> veritabanı için
3Business -> iş kuralları için
4)Core -> ortak kullanımlar için
5) MVC -> Kullanıcı arayüzü için

###-->> NOT: GÜVENLİK SEBEBİYLE ŞİFRELER VERİTABANINDA HASHLENEREK TUTULMAKTADIR.

PROJE TANITIMI:
1-) projede admin ve kullanıcının giriş yapacağı giriş paneli vardır.
2) admin giriş yaparken sicilno: 999999 şifre:Admin123 kullanacaktır.
3) giriş yaptıktan sonra tayin yapan personellerin listesini görecektir ve isterse excel ile dışarı aktarabilecektir. isterseniz tayinleri silebilirsiniz.
4) personel listesi ile tayin dönemlerinde tayin yapabilecek personelleri görebilirsiniz.
5) ayrıca tayin dönemlerinde tayin hakkı kazanan personelleri ekleyebilir ve eğer yönetici atamak istersenizde aynı şekilde yönetici ekleyebilirsiniz.
6) istediğiniz zamanda personelleri silebilirsiniz. ayrıca yaptığınız işlemlerden sonra da uyarı mesajları alacaksınız.
7) eklediğiniz personeller sizin verdiğiniz sicilno ve şifre ile giriş yapabilecektir.
8) ben bir tane standart kullanıcı oluşturdum sicilno: 317740 şifre:123456
9) kullanıcı giriş yaptığında sağ üst köşede çıkış yap butonu yanında giriş yapan hangi kullanıcı ise onun ismi yazacaktır.
10) tayin taleplerim sayfasında kişisel bilgilerini de görebilecektir. her giriş yapan kullanıcı kendi bilgilerini görebilecektir.
11) her farklı günü bir tayin dönemi olarak aldım.
12) kullanıcı yeni tayin talebi oluştur butonuna tıklarsa yeni tayin talebi oluşturur.
13) burada talep türünü seçmek zorunda seçmezse uyarı almaktadır. ayrıca en fazla 3 tercih yapabilir 1.tercih zorunlu diğer 2 tercih ise isteğe bağlıdır.
14) aynı gün yapılan tayin işlemi 1 dönem olarak kabul edilir bu yüzden 1 tayin döneminde 1 tane tayin yazabilmektedir.
15) Kullanıcı geçmişte yaptığı tayinleri de tayin taleplerimde görebilir.
16) tayin talebi yaptıktan sonra silememekte ve sadece düzenleyebilmektedir.
17) tayin talebini admin kullanıcı silebilmektedir.
18) çıkışyap ile çıkış yapabilmektedir.

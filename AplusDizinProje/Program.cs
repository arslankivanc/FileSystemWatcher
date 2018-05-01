using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplusDizinProje
{
    
    class Program
    {
        
        // C:\Aplus  dizininde klasör oluşturma.
        public static void DizinOlustur()
        {
            try
            {
                string path = @"C:\Aplus";
                //Aynı isimde klasör varsa tekrardan klasör oluşturmaz.
                if (!Directory.Exists(path))
                {
                    // CreateDirectory metodu ile dizin oluşturma işlemi.
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bir Hata Oluştu: {0}", e.ToString());
            }
        }
        public static void MevcutDosyalariGoster()
        {
            string[] dosyaYolu = Directory.GetFiles(@"C:\Aplus", "*", SearchOption.AllDirectories);

            foreach (var item in dosyaYolu)
            {
                Console.WriteLine(item);
            }
        }
        static void Main(string[] args)
        {
            /*DizinOlustur metodumuzu çağırdık. C:\Aplus  dizini altında klasör oluştu.
              Bu dizinde ki değişiklikeri izleyeceğiz*/
            DizinOlustur();

            //Mevcut dosyaları listeleme metodu
            MevcutDosyalariGoster();

            try
            {
                string yol = @"C:\Aplus";
                if (Directory.Exists(yol))
                {
                    FileSystemWatcher izle = new FileSystemWatcher(@"C:\Aplus");

                    izle.EnableRaisingEvents = true;
                    izle.IncludeSubdirectories = true;


                    izle.Renamed += YenidenAdlandir;
                    izle.Changed += Degistir;
                    izle.Created += Olustur;
                    izle.Deleted += Sil;

                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine( yol +" dizini mevcut değildir. Tekrar gözden geçiriniz. ");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dosya izlenmiyor: {0}", ex.ToString());
            }
            
        }

        //İlgili dosyaların isimleri yeniden adlandırdığında çalışacak kısım
        private static void YenidenAdlandir(object sender, RenamedEventArgs e)
        {
            #region YenidenAdlandırma
            try
            {
                Console.WriteLine("Dosya:-{0}- dizininde ki dosya -{1}- adıyla  {2} saatinde yeniden adlandırıldı.", e.OldName, e.Name, DateTime.Now.ToLocalTime());
            }
            catch (Exception detay)
            {
                Console.WriteLine("Değişim kaydedilemedi.Detay: {0}", detay.ToString());
            }
            #endregion
        }

        //İlgili dosyaların herhangi biri silindiğinde çalışacak kısım
        private static void Sil(object sender, FileSystemEventArgs e)
        {
            #region Silme
            try
            {
                Console.WriteLine("Dosya:-{0}- dizininde ki dosya , {1} saatinde silindi.", e.Name, DateTime.Now.ToLocalTime());
            }
            catch (Exception detay)
            {
                Console.WriteLine("Dosya silinirken bir hata oluştu. Detay: {0}", detay.ToString());
            }
            #endregion
        }

        //İlgili dosyaların herhangi birinin içeriğinde değişiklik olduğunda çalışacak kısım
        private static void Degistir(object sender, FileSystemEventArgs e)
        {
            #region Degistirme
            try
            {
                Console.WriteLine("Dosya:-{0}- dosya dizininde  {1} saatinde değişiklik yapıldı.", e.Name, DateTime.Now.ToLocalTime());
            }
            catch (Exception detay)
            {
                Console.WriteLine("Oluşturulan dosya kaydedilmedi.Detay: {0}", detay.ToString());
            }
            #endregion
        }

        //İlgili dizinde herhangi bir dosya oluşturulduğunda çalışacak kısım
        private static void Olustur(object sender, FileSystemEventArgs e)
        {
            AplusDosyaEntities db = new AplusDosyaEntities();
            AplusTable tablo = new AplusTable();

            #region Dosya Olusturma
            try
            {
                Console.WriteLine("Dosya:-{0}- dizininde ki dosya  {1} saatinde oluşturuldu.", e.Name, DateTime.Now.ToLocalTime());
                
                tablo.AplusFileName = Path.GetFileName(e.Name);
                tablo.AplusFilePath = e.FullPath;
                tablo.AplusFileType = Path.GetExtension(e.Name);
                if (tablo.AplusFileType == "")
                {
                    tablo.AplusFileType = "File";
                }
                db.AplusTable.Add(tablo);
                db.SaveChanges();
            }
            catch (Exception detay)
            {
                Console.WriteLine("Oluşturulan dosya kaydedilmedi.Detay: {0}", detay.ToString());
            }
            #endregion
        }
    }
}

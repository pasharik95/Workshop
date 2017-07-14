using MyCourseWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork.CoderDecoder
{
    public static class CoderDecoder
    {
        public static string code(string codeText = "3")
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(codeText);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
        public static string decode(string decodeText)
        {
            return decodeText;
        }
        public static string getFullInfoAboutProccess(string shortInfoAboutProccess)
        {
            string[] info = shortInfoAboutProccess.Split(' ');
            string fullInfoAboutProccess = "";
            try
            {
                using (var db = new workshopEntities())
                {
                    var oneProcess = db.process.Find(Convert.ToInt32(info[0]));
                    if (oneProcess == null)
                        throw new Exception("not found process");
                    if (oneProcess.Operation_idOperation != Convert.ToInt32(info[1]) || oneProcess.Order_idOrder != Convert.ToInt32(info[2]))
                        throw new Exception("not found process");
                    else
                    {
                        var detailList = db.process_has_detail.Where(phd => phd.Process_idProcess == oneProcess.idProcess).Select(d => d.detail).ToList();
                        if (detailList.Count != info.Length - 3)
                            throw new Exception("not found process");
                        else
                        {
                            for (int i = 3; i < info.Length; i++)
                            {
                                if (detailList[i - 3].idDetail != Convert.ToInt32(info[i]))
                                    throw new Exception("not found process");
                            }
                        }
                    }

                    //var oneWorker = db.worker.Find(Convert.ToInt32(info[1]));
                    //fullInfoAboutProccess = "Ім'я працівника - " + oneWorker.lastName + " " + oneWorker.firstName + " " + oneWorker.middleName + "\n";

                    var oneOperation = db.operation.Find(Convert.ToInt32(info[1]));
                    fullInfoAboutProccess += "Операція - " + oneOperation.nameOperation + "\n";

                    var oneOrder = db.order.Find(Convert.ToInt32(info[2]));
                    fullInfoAboutProccess += "Автомобіль - \n\tМодель - " + oneOrder.car.modelofcar.nameModelOfCar + "\n\tМарка - " + oneOrder.car.modelofcar.markofcar.nameMarkOfCar + "\n\tКолір - " + oneOrder.car.colorofcar.nameColorOfCar + "\n\tРеєстраційний номер - " + oneOrder.car.registrNumber + "\n";

                    fullInfoAboutProccess += "Деталі : \n";
                    for (int i = 3; i < info.Count(); i++)
                    {
                        var oneDetail = db.detail.Find(Convert.ToInt32(info[i]));
                        fullInfoAboutProccess += "  " + (i - 2).ToString() + ") " + oneDetail.standartdetail.nameStandartDetail + "\n";
                    }

                }
            }
            catch
            {
                throw new Exception("not found process");
            }
            return fullInfoAboutProccess;
        }
        public static int processOrWorker(string code)
        {
            if (CountWords(code, " ") == 0)
                return 1;
            else
                return 2;
        }
        private static int CountWords(string s, string s0)
        {
            int count = (s.Length - s.Replace(s0, "").Length) / s0.Length;
            return count;
        }

        internal static string getInfoAboutWorker(string code)
        {
            worker findWorker = new worker();
            using (var db = new workshopEntities())
            {
                foreach (worker w in db.worker.ToList())
                    if (code == w.getCode)
                    {
                        findWorker = w;
                        break;
                    }
                if (findWorker == null)
                    throw new Exception("not found worker");
                return "Працівник - " + findWorker.lastName + " " + findWorker.firstName + " " + findWorker.middleName + "_" + findWorker.idWorker.ToString();
            }
        }
    }
}

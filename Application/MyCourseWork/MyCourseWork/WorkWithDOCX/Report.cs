using MyCourseWork.Models;
using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork.WorkWithDOCX
{
    public static class Report
    {
        private static string fileName;


        public static void createWorksWithCarDocument(order o)
        {
            var doc = DocX.Create("WorksWithCar" + ".docx");
            doc.PageLayout.Orientation = Novacode.Orientation.Landscape;

            doc.MarginLeft = 40;
            doc.MarginTop = 10;

            //СЧ|ГЛ
            Novacode.Table table1 = doc.AddTable(1, 2);
            table1.AutoFit = AutoFit.Contents;
            table1.Alignment = Alignment.center;
            table1.Rows[0].Cells[0].Paragraphs.First().InsertText("СЧ", false, new Formatting { FontFamily = new System.Drawing.FontFamily("Times New Roman"), Size = 12 });
            table1.Rows[0].Cells[1].Paragraphs.First().InsertText("ГЛ", false, new Formatting { FontFamily = new System.Drawing.FontFamily("Times New Roman"), Size = 12 });
            doc.InsertTable(table1);

            //Час и дата получения и здачі авто
            Novacode.Table table2 = doc.AddTable(1, 2);
            table2.AutoFit = AutoFit.Window;
            table2.Alignment = Alignment.center;
            table2.Design = Novacode.TableDesign.TableNormal;

            table2.Rows[0].Cells[0].Paragraphs.First().InsertText("Мережа майстерень кузовного ремонту «PS»", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Bold = true, Size = 16 });
            table2.Rows[0].Cells[0].InsertParagraph("\nЗАКАЗ – НАРЯД  № ", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 12 });
            table2.Rows[0].Cells[0].Paragraphs[1].InsertText(o.idOrder.ToString(), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Bold = true, Italic = true, Size = 14 });

            table2.Rows[0].Cells[0].Paragraphs[1].InsertText("\nПІБ клієнта ", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 12 });
            table2.Rows[0].Cells[0].Paragraphs[1].InsertText(getFIOClient(o.Car_idCar), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Bold = true, Italic = true, Size = 14 });

            table2.Rows[0].Cells[0].Paragraphs[1].Alignment = Alignment.left;
            table2.Rows[0].Cells[1].Paragraphs.First().InsertTableBeforeSelf(5, 4);
            table2.Rows[0].Cells[1].Tables.First().AutoFit = AutoFit.Contents;
            table2.Rows[0].Cells[1].Tables.First().Rows[0].Cells[0].Paragraphs.First().InsertText("Дата прийому авто від клієнта", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[0].Cells[1].Paragraphs.First().InsertText(o.dateTime.Value.ToString().Split(' ')[0], false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[0].Cells[2].Paragraphs.First().InsertText("Час прийому авто від клієнта", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[0].Cells[3].Paragraphs.First().InsertText(o.dateTime.Value.ToString().Split(' ')[1], false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });

            table2.Rows[0].Cells[1].Tables.First().Rows[1].Cells[0].Paragraphs.First().InsertText("Дата і Час - авто в цеху\n(ставить НЧ)", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Bold = true, Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[1].Cells[2].Paragraphs.First().InsertText("Дата і Час завершення  цех\n(ставить НЧ)", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Bold = true, Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[3].Cells[0].Paragraphs.First().InsertText("Дата здачі авто клієнту", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[3].Cells[2].Paragraphs.First().InsertText("Час здачі авто клієнту", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });

            table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].Paragraphs.First().InsertText("Дата і Час – авто в цеху: ", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Italic = true, Bold = true, Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].Paragraphs.First().InsertText("автомобіль пригнано  в цех. Ставить НЧ.\n", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Italic = true, Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].Paragraphs.First().InsertText("Дата і Час завершення цех ", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Italic = true, Bold = true, Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].Paragraphs.First().InsertText("–роботи завершені по авто в цеху. Ставить НЧ.", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Italic = true, Size = 10 });
            table2.Rows[0].Cells[1].Tables.First().Rows[4].MergeCells(0, table2.Rows[0].Cells[1].Tables.First().ColumnCount - 1);

            for (int i = 0; i < 3; i++)
                table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].RemoveParagraph(table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].Paragraphs[1]);

            table2.Rows[0].Cells[1].Tables.First().Rows[4].Cells[0].Paragraphs.First().Alignment = Alignment.center;

            table2.Rows[0].Cells[1].Tables.First().MergeCellsInColumn(0, 1, 2);
            table2.Rows[0].Cells[1].Tables.First().MergeCellsInColumn(2, 1, 2);
            table2.Rows[0].Cells[1].Tables.First().Rows[1].Cells[1].FillColor = Color.WhiteSmoke;
            table2.Rows[0].Cells[1].Tables.First().Rows[2].Cells[1].FillColor = Color.WhiteSmoke;
            table2.Rows[0].Cells[1].Tables.First().Rows[1].Cells[3].FillColor = Color.WhiteSmoke;
            table2.Rows[0].Cells[1].Tables.First().Rows[2].Cells[3].FillColor = Color.WhiteSmoke;


            doc.InsertTable(table2);
            doc.InsertParagraph("");

            //Информация о авто
            Novacode.Table table3 = doc.AddTable(2, 21);
            table3.AutoFit = AutoFit.Contents;
            table3.Alignment = Alignment.left;

            table3.Rows[0].Cells[0].Paragraphs.First().InsertText("Марка, модель", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table3.Rows[0].Cells[1].Paragraphs.First().InsertText(getMarkaModelCar(o.Car_idCar), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Italic = true, Bold = true, Size = 12 });
            table3.Rows[1].Cells[0].Paragraphs.First().InsertText("Рік випуску", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table3.Rows[1].Cells[1].Paragraphs.First().InsertText(getYearOld(o.Car_idCar), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table3.Rows[0].Cells[2].Paragraphs.First().InsertText("Держ. реєс.номер", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table3.Rows[0].Cells[3].Paragraphs.First().InsertText(getRegNumber(o.Car_idCar), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 12, Bold = true });
            table3.Rows[1].Cells[2].Paragraphs.First().InsertText("VIN код кузова", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });

            for (int i = 0; i < 17; i++)
            {
                string vincode = getVinCode(o.Car_idCar);
                table3.Rows[1].Cells[3 + i].Paragraphs.First().InsertText(vincode[i].ToString(), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10, Bold = true, Italic = true });
            }
            table3.Rows[0].MergeCells(3, 19);
            for (int i = 0; i < 16; i++)
                table3.Rows[0].Cells[3].RemoveParagraph(table3.Rows[0].Cells[3].Paragraphs[1]);

            table3.Rows[0].Cells[4].Paragraphs.First().InsertText("Пробіг", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });
            table3.Rows[1].Cells[20].Paragraphs.First().InsertText(getDistance(o.Car_idCar), false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 10 });

            table3.Rows[0].Cells[0].Width = 120;// new List<double> { 30, 30, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 30 };
            table3.Rows[0].Cells[2].Width = 120;
            table3.Rows[0].Cells[4].Width = 120;
            table3.Rows[0].Cells[1].FillColor = Color.WhiteSmoke;
            table3.Rows[0].Cells[3].FillColor = Color.WhiteSmoke;
            table3.Rows[1].Cells[table3.Rows[1].Cells.Count - 1].FillColor = Color.WhiteSmoke;
            doc.InsertTable(table3);

            doc.InsertParagraph("роботи: ", false, new Formatting { FontFamily = new FontFamily("Times New Roman"), Size = 12 });

            List<WorkClass> worksList = getWorksList(o.idOrder);
            Novacode.Table table4 = doc.AddTable(worksList.Count + 1, 12);
            table4.Alignment = Alignment.left;

            Formatting f = new Formatting();

            f.FontFamily = new FontFamily("Times New Roman");
            f.Size = 10;

            table4.Rows[0].Cells[0].Paragraphs.First().InsertText("Код", false, f);
            table4.Rows[0].Cells[0].Width = 30;
            table4.Rows[0].Cells[1].Paragraphs.First().InsertText("Назва робіт", false, f);
            table4.Rows[0].Cells[1].Width = 350;
            table4.Rows[0].Cells[2].Paragraphs.First().InsertText("Початок\n(по плану)\nДата\tЧас", false, f);
            table4.Rows[0].MergeCells(2, 3);
            table4.Rows[0].RemoveParagraph(table4.Rows[0].Cells[2].Paragraphs[1]);
            table4.Rows[0].Cells[2].Width = 200;
            table4.Rows[0].Cells[3].Paragraphs.First().InsertText("Кінець\n(по плану)\nДата\tЧас", false, f);
            table4.Rows[0].MergeCells(3, 4);
            table4.Rows[0].RemoveParagraph(table4.Rows[0].Cells[3].Paragraphs[1]);
            table4.Rows[0].Cells[3].Width = 200;
            table4.Rows[0].Cells[4].Paragraphs.First().InsertText("Початок\n(по факту)\nДата\tЧас", false, f);
            table4.Rows[0].MergeCells(4, 5);
            table4.Rows[0].RemoveParagraph(table4.Rows[0].Cells[4].Paragraphs[1]);
            table4.Rows[0].Cells[4].Width = 200;
            table4.Rows[0].Cells[5].Paragraphs.First().InsertText("Кінець\n(по факту)\nДата\tЧас", false, f);
            table4.Rows[0].MergeCells(5, 6);
            table4.Rows[0].RemoveParagraph(table4.Rows[0].Cells[5].Paragraphs[1]);
            table4.Rows[0].Cells[5].Width = 200;
            table4.Rows[0].Cells[6].Paragraphs.First().InsertText("Виконавець", false, f);
            table4.Rows[0].Cells[6].Width = 150;
            table4.Rows[0].Cells[7].Paragraphs.First().InsertText("Стан", false, f);
            table4.Rows[0].Cells[7].Width = 100;
            foreach (Cell c in table4.Rows[0].Cells)
            {
                c.Paragraphs.First().Alignment = Alignment.center;
            }
            int nunmberRow = 1;
            foreach (WorkClass wc in worksList)
            {
                table4.Rows[nunmberRow].Cells[0].Paragraphs.First().InsertText(wc.Id.ToString(), false, f);

                table4.Rows[nunmberRow].Cells[1].Paragraphs.First().InsertText(wc.NameOperation, false, f);

                table4.Rows[nunmberRow].Cells[2].Paragraphs.First().InsertText(wc.StartPlan.Split(' ')[0], false, f);
                table4.Rows[nunmberRow].Cells[3].Paragraphs.First().InsertText(wc.StartPlan.Split(' ')[1], false, f);

                table4.Rows[nunmberRow].Cells[4].Paragraphs.First().InsertText(wc.FinishPlan.Split(' ')[0], false, f);
                table4.Rows[nunmberRow].Cells[5].Paragraphs.First().InsertText(wc.FinishPlan.Split(' ')[1], false, f);

                if (wc.StartFact != "")
                {
                    table4.Rows[nunmberRow].Cells[6].Paragraphs.First().InsertText(wc.StartFact.Split(' ')[0], false, f);
                    table4.Rows[nunmberRow].Cells[7].Paragraphs.First().InsertText(wc.StartFact.Split(' ')[1], false, f);
                }

                if (wc.FinishFact != "")
                {
                    table4.Rows[nunmberRow].Cells[8].Paragraphs.First().InsertText(wc.FinishFact.Split(' ')[0], false, f);
                    table4.Rows[nunmberRow].Cells[9].Paragraphs.First().InsertText(wc.FinishFact.Split(' ')[1], false, f);
                }

                table4.Rows[nunmberRow].Cells[10].Paragraphs.First().InsertText(wc.NameWorker, false, f);
                table4.Rows[nunmberRow].Cells[11].Paragraphs.First().InsertText(wc.Condition, false, f);

                nunmberRow++;
            }
            doc.InsertTable(table4);

            doc.Save();
            ViewModels.ClassForAudio.playScan();
        }

        public static void createTasksDocument(List<process> ps)
        {
            List<string> textForQRCode = new List<string>();
            List<string> textForInfoAboutCar = new List<string>();
            List<string> textForInfoAboutOperation = new List<string>();
            List<string> textForInfoAboutDetails = new List<string>();
            List<DateTime> dateTimeForStartProcess = new List<DateTime>();
            List<DateTime> dateTimeForFinishProcess = new List<DateTime>();
            foreach (process p in ps)
            {
                textForQRCode.Add(p.idProcess + " " + p.Operation_idOperation + " " + p.Order_idOrder + " ");

                textForQRCode[textForQRCode.Count() - 1] += listDetailIdForQRCode(p.idProcess);
                textForQRCode[textForQRCode.Count() - 1] = textForQRCode[textForQRCode.Count() - 1].Substring(0, textForQRCode[textForQRCode.Count() - 1].Length - 1);

                textForInfoAboutCar.Add(getInfoAboutCar(p.Order_idOrder));
                textForInfoAboutOperation.Add(getInfoAboutOperation(p.Operation_idOperation));
                textForInfoAboutDetails.Add(getInfoAboutDetails(p.idProcess));
                dateTimeForStartProcess.Add((DateTime)p.dateTimeStart);
                dateTimeForFinishProcess.Add((DateTime)p.dateTimeFinish);
            }
            createTaskDocument(textForQRCode, getFIOWorker(ps[0].Worker_idWorker), textForInfoAboutOperation, textForInfoAboutCar, textForInfoAboutDetails, dateTimeForStartProcess, dateTimeForFinishProcess);
 
        }
        public static void createTaskDocument(List<string> textForQRcode, string nameWorker, List<string> typeOfWork, List<string> infoAboutAuto, List<string> infoAboutDetails, List<DateTime> dateTimeStart, List<DateTime> dateTimeFinish)
        {
            // Create a document in memory:
            var doc = DocX.Create("Task" + ".docx");
            doc.PageLayout.Orientation = Novacode.Orientation.Landscape;
            for (int i = 0; i < textForQRcode.Count(); i++)
            {

                var text = "Завдання \"" + typeOfWork[i] + "\"\n\n";

                var format = new Formatting();
                format.Size = 24D;
                format.FontFamily = new System.Drawing.FontFamily("Times New Roman");

                var format1 = new Formatting();
                format1.Size = 20D;
                format1.Bold = true;
                format1.FontFamily = new System.Drawing.FontFamily("Times New Roman");

                Paragraph title = doc.InsertParagraph(text, false, format);
                title.Alignment = Alignment.center;

                Novacode.Table table = doc.AddTable(1, 3);

                table.Design = Novacode.TableDesign.TableNormal;
                table.Alignment = Alignment.left;

                using (var ms = new MemoryStream())
                {
                    System.Drawing.Image qrCode = generateQR(textForQRcode[i]);

                    qrCode.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);  // Save your picture in a memory stream.
                    ms.Seek(0, SeekOrigin.Begin);

                    Novacode.Image img = doc.AddImage(ms); // Create image.

                    Picture pic1 = img.CreatePicture();     // Create picture.

                    table.Rows[0].Cells[0].Paragraphs.First().InsertPicture(pic1, 0);
                }

                format.Size = 18D;
                table.SetWidths(new float[] { 300, 200, 750 });
                table.Rows[0].Cells[2].Paragraphs.First().InsertText("Автомобіль - ", false, format);
                table.Rows[0].Cells[2].Paragraphs.First().InsertText(infoAboutAuto[i], false, format1);
                table.Rows[0].Cells[2].InsertParagraph("Деталі : \n", false, format).InsertText(infoAboutDetails[i], false, format1);
                table.Rows[0].Cells[2].InsertParagraph("Виконавець(по плану) - ", false, format).InsertText(nameWorker, false, format1);
                table.Rows[0].Cells[2].InsertParagraph("Початок роботи - ", false, format).InsertText(dateTimeStart[i].ToString(), false, format1);
                table.Rows[0].Cells[2].InsertParagraph("Кінець роботи - ", false, format).InsertText(dateTimeFinish[i].ToString(), false, format1);


                doc.InsertTable(table);
                if (textForQRcode.Count() - i > 1)
                    doc.InsertSectionPageBreak();

                doc.PageLayout.Orientation = Novacode.Orientation.Landscape;
            }
            doc.Save();
            ViewModels.ClassForAudio.playScan();
        }

        private static Bitmap generateQR(string text)
        {
            var bw = new ZXing.BarcodeWriter();
            var encOptions = new ZXing.Common.EncodingOptions() { Width = 250, Height = 250, Margin = 0 };
            bw.Options = encOptions;
            bw.Format = ZXing.BarcodeFormat.QR_CODE;
            var result = new Bitmap(bw.Write(text));

            return result;
        }

        private static string getInfoAboutDetails(int idProcess)
        {
            string infoAboutCar = "";
            using (var db = new workshopEntities())
            {
                var _details = db.process_has_detail.Where(d => d.Process_idProcess == idProcess).Select(d => d.detail);
                foreach (detail d in _details.ToList())
                {
                    infoAboutCar += "   - " + d.standartdetail.nameStandartDetail + "\n";
                }
            }
            if (infoAboutCar.Length != 0)
                return infoAboutCar.Substring(0, infoAboutCar.Length - 1);
            else
                return "";
        }
        private static string getInfoAboutCar(int idOrder)
        {
            string infoAboutCar = "";
            using (var db = new workshopEntities())
            {
                car c = db.order.Find(idOrder).car;
                infoAboutCar = c.modelofcar.markofcar.nameMarkOfCar + " " + c.modelofcar.nameModelOfCar + " " + c.registrNumber;
            }
            return infoAboutCar;
        }
        private static string getYearOld(int idCar)
        {
            string year = "";
            using (var db = new workshopEntities())
            {
                car c = db.car.Find(idCar);
                year = c.Year.Value.ToString();
            }
            return year;
        }
        private static string getDistance(int idCar)
        {
            string distance = "";
            using (var db = new workshopEntities())
            {
                car c = db.car.Find(idCar);
                distance = c.Distance.Value.ToString();
            }
            return distance;
        }
        private static string getFIOWorker(int idWorker)
        {
            using (var db = new workshopEntities())
            {
                worker w = db.worker.Find(idWorker);
                return w.lastName + " " + w.firstName + " " + w.middleName;
            }
        }
        private static string getFIOClient(int idCar)
        {
            using (var db = new workshopEntities())
            {
                car c = db.car.Find(idCar);
                return c.client.lastName + " " + c.client.firstName + " " + c.client.middleName;
            }
        }
        private static string getInfoAboutOperation(int idOperation)
        {
            using (var db = new workshopEntities())
            {
                return db.operation.Find(idOperation).nameOperation;
            }
        }
        private static string getMarkaModelCar(int idCar)
        {
            using (var db = new workshopEntities())
            {
                car c = db.car.Find(idCar);
                return c.modelofcar.markofcar.nameMarkOfCar + " " + c.modelofcar.nameModelOfCar;
            }
        }
        private static string getRegNumber(int idCar)
        {
            using (var db = new workshopEntities())
            {
                return db.car.Find(idCar).registrNumber;
            }
        }
        private static string getVinCode(int idCar)
        {
            using (var db = new workshopEntities())
            {
                return db.car.Find(idCar).vincod;
            }
        }
        private static string listDetailIdForQRCode(int processId)
        {
            string detailIds = "";
            using (var db = new workshopEntities())
            {
                var _processHasDetailsList = db.process_has_detail.Where(phd => phd.Process_idProcess == processId).Select(phd => phd.Detail_idDetail);

                foreach (int idDetail in _processHasDetailsList.ToList())
                    detailIds += idDetail + " ";
            }
            return detailIds;
        }
        private static List<WorkClass> getWorksList(int idOrder)
        {
            List<WorkClass> worksList = new List<WorkClass>();
            int i = 1;
            using (var db = new workshopEntities())
            {
                foreach (process p in db.process.Where(p => p.Order_idOrder == idOrder).ToList())
                {
                    WorkClass wc = new WorkClass();
                    wc.Id = i;
                    wc.NameOperation = p.operation.nameOperation + "\nДеталі : " + getInfoAboutDetails(p.idProcess).Replace("\n", ", ").Replace("   - ", "");


                    wc.NameWorker = p.worker.lastName + " " + p.worker.firstName + " " + p.worker.middleName;
                    if (p.dateTimeStart == null)
                        wc.StartPlan = "";
                    else
                        wc.StartPlan = p.dateTimeStart.ToString();
                    if (p.dateTimeStartFact == null)
                        wc.StartFact = "";
                    else
                        wc.StartFact = p.dateTimeStartFact.ToString();
                    if (p.dateTimeFinish == null)
                        wc.FinishPlan = "";
                    else
                        wc.FinishPlan = p.dateTimeFinish.ToString();
                    if (p.dateTimeFinishFact == null)
                        wc.FinishFact = "";
                    else
                        wc.FinishFact = p.dateTimeFinishFact.ToString();
                    worksList.Add(wc);
                    i++;
                }
            }
            return worksList;
        }
    }
}

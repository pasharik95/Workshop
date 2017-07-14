//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyCourseWork.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public partial class process
    {
        public process()
        {
            this.process_has_detail = new HashSet<process_has_detail>();
        }
    
        public int idProcess { get; set; }
        public Nullable<System.DateTime> dateTimeStart { get; set; }
        public Nullable<System.DateTime> dateTimeFinish { get; set; }
        public Nullable<System.DateTime> dateTimeStartFact { get; set; }
        public Nullable<System.DateTime> dateTimeFinishFact { get; set; }
        public int Worker_idWorker { get; set; }
        public int Operation_idOperation { get; set; }
        public int Order_idOrder { get; set; }
        public Nullable<int> Worker_idWorker_Fact { get; set; }
    
        public virtual operation operation { get; set; }
        public virtual order order { get; set; }
        public virtual ICollection<process_has_detail> process_has_detail { get; set; }
        public virtual worker worker { get; set; }
        public string getSetOfDetails
        {
            get
            {
                string infoAboutCar = "";
                using (var db = new workshopEntities())
                {
                    var _details = db.process_has_detail.Where(d => d.Process_idProcess == idProcess).Select(d => d.detail);
                    int i = 1;
                    foreach (detail d in _details.ToList())
                    {
                        infoAboutCar += i + ") " + d.standartdetail.nameStandartDetail + "\n";
                        i++;
                    }
                }
                return infoAboutCar.Substring(0, infoAboutCar.Length - 1);
            }
        }
    }
}

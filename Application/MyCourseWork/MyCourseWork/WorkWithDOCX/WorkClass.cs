using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork.WorkWithDOCX
{
    class WorkClass
    {
        public int Id { get; set; }
        public string NameOperation { get; set; }
        public string StartPlan { get; set; }
        public string StartFact { get; set; }
        public string FinishPlan { get; set; }
        public string FinishFact { get; set; }
        public string NameWorker { get; set; }
        public string Condition
        {
            get
            {
                if (StartFact == "" && FinishFact == "")
                    return "Не розпочато";
                if (StartFact != "" && FinishFact == "")
                    return "Виконується";
                if (StartFact != "" && FinishFact != "")
                    return "Завершено";
                return Condition;
            }
            set
            {
                Condition = value;
            }
        }
    }
}

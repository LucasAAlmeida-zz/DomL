using DomL.Business.Utils.DTOs;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities
{
    public abstract class SingleDayActivity : Activity
    {
        public SingleDayActivity(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public SingleDayActivity() { }

        public static void EscreveConsolidadasNoArquivo(string filePath, List<SingleDayActivity> atividades)
        {
            using (var file = new StreamWriter(filePath)) {
                foreach (var atividade in atividades) {
                    file.WriteLine(atividade.ParseToString());
                }
            }
        }
    }
}

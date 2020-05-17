using DomL.Business.Activities;
using System.Collections.Generic;

namespace DomL.Business.Utils.DTOs
{
    public class ConsolidateDTO
    {
        public List<Activity> allNewAtividades;
        public string fileDir;
        public int year;
        public List<Activity> allAtividades;

        public ConsolidateDTO(List<Activity> atividades, string fileDir, int year, List<Activity> atividadesFull)
        {
            allNewAtividades = atividades;
            this.fileDir = fileDir;
            this.year = year;
            allAtividades = atividadesFull;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.Business.DTOs
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

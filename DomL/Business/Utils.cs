using DomL.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.Business
{
    public class Utils
    {
        public static Activity GetAtividadeVelha(string diaMes, int ano, Category categoria)
        {
            int dia = int.Parse(diaMes.Substring(0, 2));
            int mes = int.Parse(diaMes.Substring(3, 2));
            var atividadeVelha = new Activity
            {
                Categoria = categoria,
                Dia = new DateTime(ano, mes, dia)
            };
            return atividadeVelha;
        }

        public static IEnumerable<Activity> GetAtividadesToAdd(IEnumerable<Activity> atividadesNovas, List<Activity> atividadesVelhas)
        {
            var atividadesToAdd = new List<Activity>();
            foreach (Activity atividade in atividadesNovas)
            {
                Activity atividadeVelha = atividadesVelhas.FirstOrDefault(av => av.Dia == atividade.Dia);
                if (atividadeVelha == null) { atividadesToAdd.Add(atividade); }
            }
            return atividadesToAdd;
        }

        public static bool IsEqualTitle(string titulo1, string titulo2)
        {
            string titulo1Limpo = titulo1.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            string titulo2Limpo = titulo2.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            return titulo1Limpo == titulo2Limpo;
        }
    }
}

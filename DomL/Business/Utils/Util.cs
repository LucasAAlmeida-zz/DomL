using DomL.Business.Activities;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Utils
{
    public class Util
    {
        public static ActivityDTO GetAtividadeVelha(string diaMes, int year, Category categoria, Classification classification = Classification.Unica)
        {
            if (!int.TryParse(diaMes.Substring(0, 2), out int dia))
            {
                return null;
            }

            int mes = int.Parse(diaMes.Substring(3, 2));
            var atividadeVelhaDTO = new ActivityDTO
            {
                Categoria = categoria,
                Dia = new DateTime(year, mes, dia),
                Classificacao = classification,
            };

            return atividadeVelhaDTO;
        }

        public static IEnumerable<Activity> GetAtividadesToAdd(IEnumerable<Activity> atividadesNovas, IEnumerable<Activity> atividadesVelhas)
        {
            var atividadesToAdd = new List<Activity>();
            foreach (Activity atividade in atividadesNovas)
            {
                Activity atividadeVelha = atividadesVelhas.FirstOrDefault(av => av.Dia == atividade.Dia);
                if (atividadeVelha == null)
                {
                    atividadesToAdd.Add(atividade);
                }
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

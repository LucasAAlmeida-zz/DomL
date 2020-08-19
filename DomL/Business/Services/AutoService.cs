using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.Business.Services
{
    public class AutoService
    {
        public static void SaveFromRawLine(string[] segmentos, Activity activity)
        {
            //AUTO; (Assunto) Qual automovel; (Descricao) O que Aconteceu

            var autoName = segmentos[1];
            var description = segmentos[2];

            var autoRepo = new AutoRepository(new DomLContext());

            var auto = autoRepo.GetAutoByName(autoName);
            if (auto == null) {
                auto = new Auto() {
                    Name = autoName
                };
                auto = autoRepo.CreateAuto(auto);
            }

            var autoActivity = new AutoActivity() {
                Id = activity.Id,
                AutoId = auto.Id,
                Description = description
            };
            autoRepo.CreateAutoActivity(autoActivity);
        }
    }
}

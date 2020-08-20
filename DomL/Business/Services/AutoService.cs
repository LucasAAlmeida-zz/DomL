using DomL.Business.Entities;
using DomL.DataAccess;
using DomL.DataAccess.Repositories;
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

            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var auto = unitOfWork.AutoRepo.GetAutoByName(autoName);
                if (auto == null) {
                    auto = new Auto() {
                        Name = autoName
                    };
                }

                var autoActivity = new AutoActivity() {
                    Activity = activity,
                    Auto = auto,
                    Description = description
                };

                unitOfWork.AutoRepo.CreateAutoActivity(autoActivity);
                unitOfWork.Complete();
            }
        }
    }
}

using DomL.Business.Activities.SpecialActivities;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Activities
{
    public abstract class SpecialActivity : Activity
    {
        public SpecialActivity(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO)
        {
            ParseAtividade(segmentos);
        }

        public static void Consolidate(List<Activity> newCategoryActivities, string fileDir, int year)
        {
            var newEventActivities = newCategoryActivities.Where(ad => ad.Categoria == Category.Event).ToList();
            Event.Consolidate(newEventActivities, fileDir, year);
            
            var newSpecialEventActivities = newCategoryActivities.Where(ad => ad.IsInBlocoEspecial).ToList();
            SpecialEvent.Consolidate(newSpecialEventActivities, fileDir, year);
        }
    }
}

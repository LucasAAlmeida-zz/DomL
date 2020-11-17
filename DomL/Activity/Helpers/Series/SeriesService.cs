using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class SeriesService
    {
        public static Series GetOrCreateByName(string seriesName, UnitOfWork unitOfWork)
        {
            if (Util.IsStringEmpty(seriesName)) {
                return null;
            }

            var series = GetByName(seriesName, unitOfWork);

            if (series == null) {
                series = new Series() {
                    Name = seriesName
                };
                unitOfWork.SeriesRepo.Add(series);
            }

            return series;
        }

        public static List<Series> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.SeriesRepo.GetAll().ToList();
        }

        public static Series GetByName(string seriesName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(seriesName)) {
                return null;
            }
            return unitOfWork.SeriesRepo.GetByName(seriesName);
        }
    }
}
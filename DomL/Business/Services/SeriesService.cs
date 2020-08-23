using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.Services
{
    public class SeriesService
    {
        public static Series GetOrCreateByName(string seriesName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(seriesName)) {
                return null;
            }

            var cleanSeriesName = Util.CleanString(seriesName);
            var series = unitOfWork.SeriesRepo.SingleOrDefault(u =>
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanSeriesName
            );

            if (series == null) {
                series = new Series() {
                    Name = seriesName
                };
                unitOfWork.SeriesRepo.Add(series);
            }

            return series;
        }
    }
}
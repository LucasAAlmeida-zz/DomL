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

            var series = unitOfWork.SeriesRepo.SingleOrDefault(u => Util.IsEqualString(u.Name, seriesName));

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
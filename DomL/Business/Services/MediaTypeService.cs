using DomL.Business.Entities;
using DomL.Business.Utils;

namespace DomL.Business.Services
{
    public class MediaTypeService
    {
        public static MediaType GetOrCreateByName(string mediaTypeName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(mediaTypeName)) {
                return null;
            }

            var cleanMediaTypeName = Util.CleanString(mediaTypeName);
            MediaType mediaType = unitOfWork.MediaTypeRepo.SingleOrDefault(u => 
                u.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                == cleanMediaTypeName
            );

            if (mediaType == null) {
                mediaType = new MediaType() {
                    Name = mediaTypeName
                };
                unitOfWork.MediaTypeRepo.Add(mediaType);
            }

            return mediaType;
        }
    }
}
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

            MediaType mediaType = unitOfWork.MediaTypeRepo.SingleOrDefault(u => Util.IsEqualString(u.Name, mediaTypeName));

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
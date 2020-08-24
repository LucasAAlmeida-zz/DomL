using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class MediaTypeService
    {
        public static MediaType GetOrCreateByName(string mediaTypeName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(mediaTypeName)) {
                return null;
            }

            var mediaType = GetByName(mediaTypeName, unitOfWork);

            if (mediaType == null) {
                mediaType = new MediaType() {
                    Name = mediaTypeName
                };
                unitOfWork.MediaTypeRepo.Add(mediaType);
            }

            return mediaType;
        }

        public static List<MediaType> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.MediaTypeRepo.GetAll().ToList();
        }

        public static MediaType GetByName(string typeName, UnitOfWork unitOfWork)
        {
            return unitOfWork.MediaTypeRepo.GetByName(typeName);
        }
    }
}
using DomL.Business.Entities;
using System;
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

        public static MediaType GetByName(string typeName, UnitOfWork unitOfWork)
        {
            if (string.IsNullOrWhiteSpace(typeName)) {
                return null;
            }
            return unitOfWork.MediaTypeRepo.GetByName(typeName);
        }

        public static List<MediaType> GetAllComicTypes(UnitOfWork unitOfWork)
        {
            return unitOfWork.MediaTypeRepo.Find(u => u.CategoryId == ActivityCategory.COMIC_ID).ToList();
        }

        public static List<MediaType> GetAllPlatformTypes(UnitOfWork unitOfWork)
        {
            return unitOfWork.MediaTypeRepo.Find(u => u.CategoryId == ActivityCategory.GAME_ID).ToList();
        }

        public static List<MediaType> GetAllShowTypes(UnitOfWork unitOfWork)
        {
            return unitOfWork.MediaTypeRepo.Find(u => u.CategoryId == ActivityCategory.SHOW_ID).ToList();
        }
    }
}
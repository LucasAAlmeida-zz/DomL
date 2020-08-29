using DomL.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class ScoreService
    {
        public static List<Score> GetAll(UnitOfWork unitOfWork)
        {
            return unitOfWork.ScoreRepo.GetAll().ToList();
        }

        public static Score GetByValue(string scoreValue, UnitOfWork unitOfWork)
        {
            int value;
            if (string.IsNullOrWhiteSpace(scoreValue) || !int.TryParse(scoreValue, out value)) {
                return null;
            }
            return unitOfWork.ScoreRepo.SingleOrDefault(u => u.Value == value);
        }
    }
}
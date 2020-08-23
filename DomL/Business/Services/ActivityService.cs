using DomL.Business.Entities;
using System;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class ActivityService
    {
        public static Activity Create(DateTime date, int dayOrder, string activityRawLine, ActivityBlock activityBlock, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(activityRawLine, "; ");

            var status = GetStatusFromFirstSegment(segments[0], unitOfWork);
            var category = GetCategoryFromFirstSegment(segments[0], unitOfWork);

            var activity = new Activity() {
                Date = date,
                DayOrder = dayOrder,
                Status = status,
                Category = category,
                ActivityBlock = activityBlock,
                OriginalLine = activityRawLine
            };

            unitOfWork.ActivityRepo.Add(activity);
            return activity;
        }

        public static ActivityBlock ChangeActivityBlock(string rawLine, UnitOfWork unitOfWork)
        {
            if (rawLine == "<END>") {
                return null;
            }

            var newBlockName = rawLine.Substring(1, rawLine.Length - 2);
            var activityBlock = new ActivityBlock {
                Name = newBlockName
            };
            unitOfWork.ActivityRepo.CreateActivityBlock(activityBlock);
            return activityBlock;
        }

        private static ActivityCategory GetCategoryFromFirstSegment(string firstSegment, UnitOfWork unitOfWork)
        {
            var categoryName = Regex.Split(firstSegment, " ")[0];
            var category = unitOfWork.ActivityRepo.GetCategoryByName(categoryName);
            return category ?? unitOfWork.ActivityRepo.GetCategoryByName("EVENT");
        }

        private static ActivityStatus GetStatusFromFirstSegment(string firstSegment, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(firstSegment, " ");

            string statusName;
            if (segments.Length == 1) {
                statusName = "SINGLE";
            } else if (IsStringFinish(segments[1])) {
                statusName = "FINISH";
            } else if (IsStringStart(segments[1])) {
                statusName = "START";
            } else {
                statusName = "SINGLE";
            }
            return unitOfWork.ActivityRepo.GetStatusByName(statusName);
        }

        private static bool IsStringFinish(string word)
        {
            return word.ToLower() == "termino" || word.ToLower() == "término";
        }

        private static bool IsStringStart(string word)
        {
            return word.ToLower() == "comeco" || word.ToLower() == "começo";
        }
    }
}
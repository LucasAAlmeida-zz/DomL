using DomL.Business.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Entities
{
    [Table("Activity")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DayOrder { get; set; }
        public int CategoryId { get; set; }
        public int? StatusId { get; set; }
        public int? PairedActivityId { get; set; }
        public int? ActivityBlockId { get; set; }
        [Required]
        public string OriginalLine { get; set; }
        
        [ForeignKey("CategoryId")]
        public ActivityCategory Category { get; set; }
        [ForeignKey("StatusId")]
        public ActivityStatus Status { get; set; }
        [ForeignKey("PairedActivityId")]
        public Activity PairedActivity { get; set; }
        [ForeignKey("ActivityBlockId")]
        public ActivityBlock ActivityBlock { get; set; }

        public virtual AutoActivity AutoActivity { get; set; }
        public virtual BookActivity BookActivity { get; set; }
        public virtual ComicActivity ComicActivity { get; set; }
        public virtual DoomActivity DoomActivity { get; set; }
        public virtual EventActivity EventActivity { get; set; }

        public void SaveFromRawLine(string rawLine, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(rawLine, "; ");
            switch (this.Category.Id) {
                case ActivityCategory.AUTO:     AutoService.SaveFromRawSegments(segments, this, unitOfWork);    break;
                case ActivityCategory.BOOK:     BookService.SaveFromRawSegments(segments, this, unitOfWork);    break;
                case ActivityCategory.COMIC:    ComicService.SaveFromRawSegments(segments, this, unitOfWork);   break;
                case ActivityCategory.DOOM:     DoomService.SaveFromRawSegments(segments, this, unitOfWork);    break;
                case ActivityCategory.EVENT:    EventService.SaveFromRawSegments(segments, this, unitOfWork);    break;
                //case ActivityCategory.GIFT:     new Gift(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.HEALTH:   new Health(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.MOVIE:    new Movie(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.PERSON:   new Person(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.PET:      new Pet(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.PLAY:     new Play(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.PURCHASE: new Purchase(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.TRAVEL:   new Travel(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.WORK:     new Work(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.GAME:     new Game(atividadeDTO, segmentos).Save(); break;
                //case ActivityCategory.SERIES:   new Series(atividadeDTO, segmentos).Save(); break;
                //default:                        new Event(atividadeDTO, segmentos).Save(); break;
            }
        }

        public void PairActivity(UnitOfWork unitOfWork)
        {
            if (this.Status.Id != ActivityStatus.FINISH) {
                return;
            }

            var startingActivity = this.GetStartingActivity(unitOfWork);

            if (startingActivity != null) {
                this.PairedActivity = startingActivity;
                startingActivity.PairedActivity = this;
            }
        }

        private Activity GetStartingActivity(UnitOfWork unitOfWork)
        {
            var psa = unitOfWork.ActivityRepo.GetPreviousStartingActivities(this.Date);

            IEnumerable<Activity> pcsa = null; // Previous Category Starting Activities
            switch (this.Category.Id) {
                case ActivityCategory.AUTO:     pcsa = AutoService.GetStartingActivity(psa, this);  break;
                case ActivityCategory.BOOK:     pcsa = BookService.GetStartingActivity(psa, this);  break;
                case ActivityCategory.COMIC:    pcsa = ComicService.GetStartingActivity(psa, this); break;
                case ActivityCategory.DOOM:     pcsa = DoomService.GetStartingActivity(psa, this);  break;
                    //case ActivityCategory.GIFT:     T": new Gift(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.HEALTH:   LTH": new Health(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.MOVIE:    IE": new Movie(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PERSON:   SON": new Person(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PET:      ": new Pet(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PLAY:     Y": new Play(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PURCHASE: CHASE": new Purchase(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.TRAVEL:   VEL": new Travel(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.WORK:     K": new Work(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.GAME:     E": new Game(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.SERIES:   IES": new Series(atividadeDTO, segmentos).Save(); break;
                    //default:                        CH": new Watch(atividadeDTO, segmentos).Save(); break;
                    //new Event(atividadeDTO, segmentos).Save(); break;
            }
            return pcsa.OrderByDescending(u => u.Date).FirstOrDefault();
        }

        //0 - month kind
        //1 - recap kind
        public string GetString(int kindOfString)
        {
            var consolidated = "";
            switch (this.Category.Id) {
                case ActivityCategory.AUTO:     consolidated = AutoService.GetString(this, kindOfString);    break;
                case ActivityCategory.BOOK:     consolidated = BookService.GetString(this, kindOfString);    break;
                case ActivityCategory.COMIC:    consolidated = ComicService.GetString(this, kindOfString);   break;
                case ActivityCategory.DOOM:     consolidated = DoomService.GetString(this, kindOfString);    break;
                case ActivityCategory.EVENT:    consolidated = EventService.GetString(this, kindOfString);   break;
                    //case ActivityCategory.GIFT:     T": new Gift(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.HEALTH:   LTH": new Health(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.MOVIE:    IE": new Movie(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PERSON:   SON": new Person(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PET:      ": new Pet(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PLAY:     Y": new Play(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.PURCHASE: CHASE": new Purchase(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.TRAVEL:   VEL": new Travel(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.WORK:     K": new Work(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.GAME:     E": new Game(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.SERIES:   IES": new Series(atividadeDTO, segmentos).Save(); break;
                    //default:                        CH": new Watch(atividadeDTO, segmentos).Save(); break;
                    //new Event(atividadeDTO, segmentos).Save(); break;
            }
            return consolidated;
        }
    }

    [Table("ActivityBlock")]
    public class ActivityBlock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //public int? Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.ActivityBlockRepo.Exists(b => b.Name == this.Name)) {
        //            return null;
        //        }

        //        unitOfWork.ActivityBlockRepo.Add(this);
        //        unitOfWork.Complete();

        //        this.Id = unitOfWork.ActivityBlockRepo.SingleOrDefault(abr => abr.Name == this.Name).Id;
        //        return this.Id;
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allActivitiesInBlocksInYear = unitOfWork.ActivityBlockRepo.GetActivitiesInBlockYear(year).ToList();
        //        using (var file = new StreamWriter(fileDir + "ActivityBlocks" + year + ".txt")) {
        //            foreach (var atividade in allActivitiesInBlocksInYear) {
        //                file.WriteLine(atividade.ParseToString());
        //            }
        //        }
        //    }
        //}
    }

    [Table("ActivityCategory")]
    public class ActivityCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public const int AUTO = 1;
        public const int BOOK = 2;
        public const int COMIC = 3;
        public const int DOOM = 4;
        public const int GIFT = 5;
        public const int HEALTH = 6;
        public const int MOVIE = 7;
        public const int PERSON = 8;
        public const int PET = 9;
        public const int PLAY = 10;
        public const int PURCHASE = 11;
        public const int TRAVEL = 12;
        public const int WORK = 13;
        public const int GAME = 14;
        public const int SERIES = 15;
        public const int EVENT = 17;
    }

    [Table("ActivityStatus")]
    public class ActivityStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public const int SINGLE = 1;
        public const int START = 2;
        public const int FINISH = 3;
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int? BlockId { get; set; }
        [Required]
        public string OriginalLine { get; set; }
        
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        [ForeignKey("PairedActivityId")]
        public Activity PairedActivity { get; set; }
        [ForeignKey("BlockId")]
        public Block Block { get; set; }

        public string GetInfoMessage()
        {
            return "Date:\t\t" + Date.ToString("dd/MM/yyyy") + "\n" +
                "Category:\t" + Category.Name + "\n" +
                "Status:\t\t" + Status.Name;
        }

        public virtual AutoActivity AutoActivity { get; set; }
        public virtual BookActivity BookActivity { get; set; }
        public virtual ComicActivity ComicActivity { get; set; }
        public virtual CourseActivity CourseActivity { get; set; }
        public virtual DoomActivity DoomActivity { get; set; }
        public virtual EventActivity EventActivity { get; set; }
        public virtual GameActivity GameActivity { get; set; }
        public virtual GiftActivity GiftActivity { get; set; }
        public virtual HealthActivity HealthActivity { get; set; }
        public virtual MovieActivity MovieActivity { get; set; }
        public virtual PetActivity PetActivity { get; set; }
        public virtual MeetActivity MeetActivity { get; set; }
        public virtual PlayActivity PlayActivity { get; set; }
        public virtual PurchaseActivity PurchaseActivity { get; set; }
        public virtual ShowActivity ShowActivity { get; set; }
        public virtual TravelActivity TravelActivity { get; set; }
        public virtual WorkActivity WorkActivity { get; set; }
    }

    [Table("Block")]
    public class Block
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public const int AUTO_ID = 1;
        public const int BOOK_ID = 2;
        public const int COMIC_ID = 3;
        public const int DOOM_ID = 4;
        public const int GIFT_ID = 5;
        public const int HEALTH_ID = 6;
        public const int MOVIE_ID = 7;
        public const int MEET_ID = 8;
        public const int PET_ID = 9;
        public const int PLAY_ID = 10;
        public const int PURCHASE_ID = 11;
        public const int TRAVEL_ID = 12;
        public const int WORK_ID = 13;
        public const int GAME_ID = 14;
        public const int SHOW_ID = 15;
        public const int EVENT_ID = 17;
        public const int COURSE_ID = 18;
    }

    [Table("Status")]
    public class Status
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

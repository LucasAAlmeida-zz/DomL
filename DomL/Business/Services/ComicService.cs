using DomL.Business.Entities;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using DomL.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomL.Business.Services
{
    public class ComicService
    {
        public static void SaveFromRawLine(string[] segmentos, Activity activity)
        {
            // COMIC|MANGA (Classification); Series Name; Chapters; Author Name; Type Name; Score; Description

            segmentos[0] = "";
            var comicWindow = new ComicWindow(segmentos);
            comicWindow.ShowDialog();

            var seriesName = (string) comicWindow.SeriesCB.SelectedItem;
            var chapters = (string) comicWindow.ChaptersCB.SelectedItem;
            var authorName = (string) comicWindow.AuthorCB.SelectedItem;
            var typeName = (string) comicWindow.TypeCB.SelectedItem;
            var score = (string) comicWindow.ScoreCB.SelectedItem;
            var description = (string) comicWindow.DescriptionCB.SelectedItem;

            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var series = unitOfWork.ComicRepo.GetSeriesByName(seriesName);
                if (series == null) {
                    series = new ComicSeries() {
                        Name = seriesName
                    };
                }

                var author = unitOfWork.ComicRepo.GetAuthorByName(authorName);
                if (author == null) {
                    author = new ComicAuthor() {
                        Name = authorName
                    };
                }

                var type = unitOfWork.ComicRepo.GetTypeByName(typeName);
                if (type == null) {
                    type = new ComicType() {
                        Name = typeName
                    };
                }

                var comicVolume = unitOfWork.ComicRepo.GetComicVolumeByChapters(chapters);
                if (comicVolume == null) {
                    comicVolume = new ComicVolume() {
                        Series = series,
                        Chapters = chapters,
                        Author = author,
                        Type = type,
                        Score = score,
                    };
                }

                var comicActivity = new ComicActivity() {
                    Activity = activity,
                    ComicVolume = comicVolume,
                    Description = description
                };

                unitOfWork.ComicRepo.CreateComicActivity(comicActivity);
                unitOfWork.Complete();
            }
        }
    }
}

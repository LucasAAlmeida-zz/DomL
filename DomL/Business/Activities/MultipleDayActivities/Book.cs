using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.MultipleDayActivities
{
    [Table("Book")]
    public class Book : MultipleDayActivity
    {
        public Book(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        // BOOK; (De Quem) Autor; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.BookRepo
                        .Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
                    return;
                }

                unitOfWork.BookRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public static IEnumerable<Book> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.BookRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static IEnumerable<Book> GetInBlockFromYear(int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.BookRepo.Find(b => b.Date.Year == year && b.ActivityBlockId != null);
            }
        }

        public static void Consolidate(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allBooks = unitOfWork.BookRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Book" + year + ".txt", allBooks.Cast<MultipleDayActivity>().ToList());
            }
        }

        public static int CountEndedYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.BookRepo
                    .Find(g =>
                        (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
                        && g.Date.Year == ano)
                    .Count();
            }
        }
    }
}

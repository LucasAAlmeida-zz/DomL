﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("BookActivity")]
    public class BookActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }

        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Book Book { get; set; }
    }

    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int? SeriesId { get; set; }
        public string NumberInSeries { get; set; }
        public string Score { get; set; }

        [ForeignKey("AuthorId")]
        public Person Author { get; set; }
        [ForeignKey("SeriesId")]
        public Series Series { get; set; }
    }


        //// BOOK; (De Quem) Autor; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

        //public override void Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.BookRepo
        //                .Exists(b => b.Date == this.Date && b.Subject == this.Subject)) {
        //            return;
        //        }

        //        unitOfWork.BookRepo.Add(this);
        //        unitOfWork.Complete();
        //    }
        //}

        //public static IEnumerable<Book> GetAllFromMes(int mes, int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.BookRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allBooks = unitOfWork.BookRepo.Find(b => b.Date.Year == year).ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Book" + year + ".txt", allBooks.Cast<MultipleDayActivity>().ToList());
        //    }
        //}

        //public static void ConsolidateAll(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allBooks = unitOfWork.BookRepo.GetAll().ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Book.txt", allBooks.Cast<MultipleDayActivity>().ToList());
        //    }
        //}

        //public static int CountEndedYear(int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.BookRepo
        //            .Find(g =>
        //                (g.Classificacao == Classification.Termino || g.Classificacao == Classification.Unica)
        //                && g.Date.Year == ano)
        //            .Count();
        //    }
        //}

        //public static void FullRestoreFromFile(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allBooks = GetBooksFromFile(fileDir + "Book.txt");
        //        unitOfWork.BookRepo.AddRange(allBooks);
        //        unitOfWork.Complete();
        //    }
        //}

        //private static List<Book> GetBooksFromFile(string filePath)
        //{
        //    var books = new List<Book>();
        //    using (var reader = new StreamReader(filePath)) {

        //        string line = "";
        //        try {
        //            while ((line = reader.ReadLine()) != null) {
        //                if (string.IsNullOrWhiteSpace(line)) {
        //                    continue;
        //                }

        //                var segmentos = Regex.Split(line, "\t");

        //                // DataInicio; DataFim; (De Quem); (Assunto); (Nota); (Descrição)

        //                int? nota = segmentos[4] != "-" ? int.Parse(segmentos[4]) : (int?)null;
        //                string descricao = segmentos[5] != "-" ? segmentos[5] : null;

        //                if (segmentos[0] == segmentos[1]) {
        //                    var book = new Book() {
        //                        Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
        //                        Classificacao = Classification.Unica,
        //                        DeQuem = segmentos[2],
        //                        Subject = segmentos[3],
        //                        Nota = nota,
        //                        Description = descricao,

        //                        DayOrder = 0,
        //                    };
        //                    books.Add(book);
        //                    continue;
        //                }

        //                if (!segmentos[0].StartsWith("??/??")) {
        //                    var book = new Book() {
        //                        Date = DateTime.ParseExact(segmentos[0], "dd/MM/yy", null),
        //                        Classificacao = Classification.Comeco,
        //                        DeQuem = segmentos[2],
        //                        Subject = segmentos[3],
        //                        Nota = nota,
        //                        Description = segmentos[1].StartsWith("??/??") ? descricao : null,

        //                        DayOrder = 0,
        //                    };
        //                    books.Add(book);
        //                }

        //                if (!segmentos[1].StartsWith("??/??")) {
        //                    var book = new Book() {
        //                        Date = DateTime.ParseExact(segmentos[1], "dd/MM/yy", null),
        //                        Classificacao = Classification.Termino,
        //                        DeQuem = segmentos[2],
        //                        Subject = segmentos[3],
        //                        Nota = nota,
        //                        Description = descricao,

        //                        DayOrder = 0,
        //                    };
        //                    books.Add(book);
        //                }
        //            }
        //        } catch (Exception e) {
        //            var msg = "Deu ruim na linha " + line;
        //            throw new ParseException(msg, e);
        //        }
        //    }
        //    return books;
        //}
}

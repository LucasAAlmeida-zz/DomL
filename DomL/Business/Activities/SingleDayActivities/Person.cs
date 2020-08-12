﻿using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Person")]
    public class Person : SingleDayActivity
    {
        [Required]
        public string Origem { get; set; }

        public Person(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (Origem) De onde conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            this.Subject = segmentos[1];
            this.Origem = segmentos[2];
            this.Description = segmentos[3];
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.PersonRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.PersonRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Origem + "\t" + this.Description;
        }

        public static int CountYear(int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PersonRepo.Find(g => g.Date.Year == ano).Count();
            }
        }

        public static IEnumerable<Person> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.PersonRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPerson = unitOfWork.PersonRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Person" + year + ".txt", allPerson.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allPerson = unitOfWork.PersonRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Person.txt", allPerson.Cast<SingleDayActivity>().ToList());
            }
        }
    }
}

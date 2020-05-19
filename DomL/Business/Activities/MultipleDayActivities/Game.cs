using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Game : MultipleDayActivity
    {
        public Game(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos)
        {
            Categoria = Category.Game;
        }

        protected override void ParseAtividade(IReadOnlyList<string> segmentos)
        {
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Valor) Nota
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Começo
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // JOGO; (De Quem) Plataforma; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            DeQuem = segmentos[1];
            Assunto = segmentos[2];
            string segmentoToLower = segmentos[3].ToLower();
            string classificacao = "unica";
            switch (segmentos.Count)
            {
                case 4:
                    if (segmentoToLower == "comeco" || segmentoToLower == "começo")
                    {
                        classificacao = segmentoToLower;
                    }
                    else
                    {
                        Valor = segmentos[3];
                    }
                    break;
                case 5:
                    if (segmentoToLower == "termino" || segmentoToLower == "término")
                    {
                        classificacao = segmentoToLower;
                        Valor = segmentos[4];
                    }
                    else
                    {
                        Valor = segmentos[3];
                        Descricao = segmentos[4];
                    }
                    break;
                case 6:
                    classificacao = segmentos[3].ToLower();
                    Valor = segmentos[4];
                    Descricao = segmentos[5];
                    break;
                default:
                    throw new Exception("what");
            }

            switch (classificacao)
            {
                case "comeco": case "começo": Classificacao = Classification.Comeco; break;
                case "termino": case "término": Classificacao = Classification.Termino; break;
                case "unica": Classificacao = Classification.Unica; break;
                default: throw new Exception("what");
            }

            if (Classificacao != Classification.Comeco && Valor != "-" && !int.TryParse(Valor, out _))
            {
                throw new Exception("what");
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            DeQuem = segmentos[2];
            Assunto = segmentos[3];
            Valor = segmentos[4];
            Descricao = segmentos[5];
        }

        protected override string ConsolidateActivity()
        {
            var dataInicio = Dia != DateTime.MinValue ? Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00") : "??/??";
            var dataTermino = DiaTermino != DateTime.MinValue ? DiaTermino.Day.ToString("00") + "/" + DiaTermino.Month.ToString("00") : "??/??";
            return dataInicio + "\t" + dataTermino + "\t" + DeQuem + "\t" + Assunto + "\t" + Valor + "\t" + Descricao;
        }
    }
}

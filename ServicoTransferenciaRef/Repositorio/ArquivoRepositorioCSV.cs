﻿using ServicoTransferenciaRef.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServicoTransferenciaRef.Repositorio
{
    public class ArquivoRepositorioCSV : IArquivoRepositorio
    {
        private static string nomeArquivoCSV = "Repositorio\\arquivos.csv";
        private ListaDeTransacoes _transferencia;

        public ArquivoRepositorioCSV()
        {
            var arrayTransferencia = new List<Arquivo>();

            using (
                var file = File.OpenText(nomeArquivoCSV))
            {
                while (!file.EndOfStream)
                {
                    var textoArquivo = file.ReadLine();
                    if (string.IsNullOrEmpty(textoArquivo))
                    {
                        continue;
                    }
                    var infoArquivo = textoArquivo.Split(';');
                    var arquivo = new Arquivo
                    {
                        Id = Convert.ToInt32(infoArquivo[1]),
                        Nome = infoArquivo[2],
                        Conta = Convert.ToDouble(infoArquivo[3]),
                        Agencia = Convert.ToDouble(infoArquivo[4]),
                        Banco = infoArquivo[5],
                        Valor = Convert.ToDecimal(infoArquivo[6]),
                        Data = infoArquivo[7]
                    };
                    switch (infoArquivo[0])
                    {
                        case "transferencia":
                            arrayTransferencia.Add(arquivo);
                            break;
                        default:
                            break;
                    }
                }
            }

            _transferencia = new ListaDeTransacoes("Transferencia", arrayTransferencia.ToArray());
        }

        public static Arquivo CreateList(string[][] linhas)
        {
            var arrayTransferencia = new Arquivo();
            foreach (string[] line in linhas)
            {
                arrayTransferencia = new Arquivo
                {
                    Nome = line[0],
                    Conta = Convert.ToDouble(line[1]),
                    Agencia = Convert.ToDouble(line[2]),
                    Banco = line[3],
                    Valor = Convert.ToDecimal(line[4]),
                    Data = line[5]
                };
            }
            return arrayTransferencia;
        }

        public ListaDeTransacoes Transferencia => _transferencia;
        public IEnumerable<Arquivo> Todos => _transferencia.Arquivos;
        public void Incluir(Arquivo arquivo)
        {
            var id = Todos.Select(x => x.Id).Max();
            using (var file = File.AppendText(nomeArquivoCSV))
            {
                file.WriteLine($"transferencia;{id + 1};{arquivo.Nome};{arquivo.Conta};{arquivo.Agencia};{arquivo.Banco};{arquivo.Valor};{arquivo.Data}");
            }
        }
    }
}

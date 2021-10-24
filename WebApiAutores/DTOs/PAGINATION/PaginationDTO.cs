﻿namespace WebApiAutores.DTOs.PAGINATION
{
    public class PaginationDTO
    {
        public int pagina { get; set; } = 1;

        private int recordsPorPagina = 10;

        private readonly int cantidadMaximaPorPagina = 50;

        public int RecordsPorPagina
        {
            get{
                return recordsPorPagina;
            }
            set{ 
                recordsPorPagina = (value > cantidadMaximaPorPagina) ? cantidadMaximaPorPagina : value;
            }

        }
    }
}

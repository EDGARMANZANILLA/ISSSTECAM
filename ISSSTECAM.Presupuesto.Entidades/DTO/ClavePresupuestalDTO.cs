using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades.DTO
{
    public class ClavePresupuestalDTO
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public int IdPrograma { get; set; }
        public int IdProyecto { get; set; }
        public int IdActividad { get; set; }
        public int IdPartida { get; set; }
        public int IdCentroCosto { get; set; }
        public int Anio { get; set; }
        public bool Activo { get; set; }
        public decimal PresupuestoEnero { get; set; }
        public decimal PresupuestoFebrero { get; set; }
        public decimal PresupuestoMarzo { get; set; }
        public decimal PresupuestoAbril { get; set; }
        public decimal PresupuestoMayo { get; set; }
        public decimal PresupuestoJunio { get; set; }
        public decimal PresupuestoJulio { get; set; }
        public decimal PresupuestoAgosto { get; set; }
        public decimal PresupuestoSeptiembre { get; set; }
        public decimal PresupuestoOctubre { get; set; }
        public decimal PresupuestoNoviembre { get; set; }
        public decimal PresupuestoDiciembre { get; set; }
        public decimal PresupuestoAnual { 
            get {
                return PresupuestoEnero + PresupuestoFebrero + PresupuestoMarzo + PresupuestoAbril + PresupuestoMayo + PresupuestoJunio
                    + PresupuestoJulio + PresupuestoAgosto + PresupuestoSeptiembre + PresupuestoOctubre + PresupuestoNoviembre + PresupuestoDiciembre;
            } }

        public string ClavePrograma { get; set; }
        public string Programa { get; set; }
        public string ClaveProyecto { get; set; }
        public string Proyecto { get; set; }
        public string ClaveActividad { get; set; }
        public string Actividad { get; set; }
        public string ClavePartida { get; set; }
        public string Partida { get; set; }
        public string ClaveCentroCosto { get; set; }
        public string CentroCosto { get; set; }

        public string PartidaGeneral {
            get
            {
                return ClavePartida.Substring(0, 1) + "000";
            }
        }
        public decimal ObtenerMontoPorMes(int mes)
        {
            switch (mes)
            {
                case 1:
                    return PresupuestoEnero;
                case 2:
                    return PresupuestoFebrero;
                case 3:
                    return PresupuestoMarzo;
                case 4:
                    return PresupuestoAbril;

                case 5:
                    return PresupuestoMayo;
                case 6:
                    return PresupuestoJunio;
                case 7:
                    return PresupuestoJulio;
                case 8:
                    return PresupuestoAgosto;

                case 9:
                    return PresupuestoSeptiembre;
                case 10:
                    return PresupuestoOctubre;
                case 11:
                    return PresupuestoNoviembre;
                case 12:
                    return PresupuestoDiciembre;
                default:
                    throw new ArgumentOutOfRangeException("El mes está fuera del intervalo");
            }
        }

        /*
        public virtual CentrosCostos CentrosCostos { get; set; }
        public virtual Partidas Partidas { get; set; }
        public virtual Programas Programas { get; set; }
        public virtual Proyectos Proyectos { get; set; }
        */
    }
}

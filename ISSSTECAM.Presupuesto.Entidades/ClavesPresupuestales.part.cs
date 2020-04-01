using System;

namespace ISSSTECAM.Presupuesto.Entidades
{
    public partial class ClavesPresupuestales
    {
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Datos
{
    public class Transaccion : IDisposable
    {
        private PresupuestoDBEntities _contexto;
        public Transaccion()
        {
            _contexto = new PresupuestoDBEntities();
        }
        internal PresupuestoDBEntities Contexto
        {
            get
            {
                return _contexto;
            }
        }
        public void GuardarCambios()
        {
            _contexto.SaveChanges();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }
    }
}

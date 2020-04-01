using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.RH.Datos
{
    public class Transaccion : IDisposable
    {
        private UsuarioSSOEntidades _contexto;
        public Transaccion()
        {
            _contexto = new UsuarioSSOEntidades();
        }
        internal UsuarioSSOEntidades Contexto
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

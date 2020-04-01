using ISSSTECAM.Presupuesto.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ISSSTECAM.Presupuesto.ServiciosWeb
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServicioClavesPresupuestales
    {
        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: agregue aquí sus operaciones de servicio
        [OperationContract]
        IEnumerable<ClavePresupuestal> ObtenerClavesPresupuestalesComprasPorRamoUnidad(string cvRamo, string cvUnidad, string cvCentroCosto, DateTime fechaExtraordinaria);

        [OperationContract]
        decimal ObtenerDisponibleClavePresupuestal(int idClavePresupuestal, DateTime fechaExtraordinaria);
    }

    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace apinovo
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_medicao
    {
        public long autonumero { get; set; }
        public Nullable<long> autonumeroCliente { get; set; }
        public string obra { get; set; }
        public string endereco { get; set; }
        public Nullable<int> prazoInicialMeses { get; set; }
        public string contrato { get; set; }
        public Nullable<int> prorrogacoesMeses { get; set; }
        public Nullable<System.DateTime> dataInicio { get; set; }
        public Nullable<System.DateTime> dataFim { get; set; }
        public string medicao { get; set; }
        public string etapa { get; set; }
        public Nullable<System.DateTime> dataInicioMedicao { get; set; }
        public Nullable<System.DateTime> dataFimMedicao { get; set; }
        public Nullable<decimal> valorGlobalPrevisto { get; set; }
        public Nullable<decimal> valorGlobalMedido { get; set; }
        public Nullable<decimal> reducao { get; set; }
        public Nullable<decimal> valorMedicao { get; set; }
        public Nullable<decimal> variacaoContratual { get; set; }
        public Nullable<decimal> aFaturar { get; set; }
        public string cancelado { get; set; }
        public Nullable<decimal> valorTotalBdiServico { get; set; }
        public string unidadeDaSMS { get; set; }
        public string processo { get; set; }
        public string prazoInicialDias { get; set; }
        public Nullable<int> sequencia { get; set; }
    }
}

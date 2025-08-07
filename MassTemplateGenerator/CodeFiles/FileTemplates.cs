using System;
using System.Text;
using Clients = DataProcessing.ClientData;
using Config = MassTemplateGenerator.Properties.Settings;
using Utils = DataProcessing.DataFunctions;

/// <summary>
/// Wrapper for the various file templates.
/// </summary>
namespace FileTemplates
{
    /// <summary>
    /// Provides a base root for all file templates so they
    /// can be used in a generic manner.
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Generates the requested amount of entries for a single
        /// file template.
        /// </summary>
        /// <param name="amount">The amount of entries to create.</param>
        /// <returns>A string with all the generated entries.</returns>
        /// <remarks>Concrete classes implementing this interface will
        /// require a public static string property called
        /// "DescriptiveName" to correctly work with the code
        /// infrastructure.</remarks>
        string Generate(int amount);
    }

    public class Nomina : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        string cuenta = _settings.Nomina_Cuenta;
        string nombre = _settings.Nomina_Beneficiario;
        string refer = _settings.Nomina_RefTrx;
        double monto = _settings.Nomina_Monto;
        string concepto = _settings.Nomina_Concepto;
        string email = _settings.Nomina_Email;
        bool rng = _settings.Nomina_ValueRNG;
        bool auto = _settings.Nomina_RefTrxRNG;
        bool acctrng = _settings.Nomina_TargetRNG;

        public static string DescriptiveName
        { get { return "Pago de Nómina"; } }

        public string Generate(int amount)
        {
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (auto) { refer = Utils.GetRandomReference(10); }
                if (acctrng)
                {
                    var acct = Clients.GetRandomBHDClient();
                    cuenta = acct.Key;
                    nombre = acct.Value;
                }
                self.Append(cuenta + ";");
                self.Append(nombre + ";");
                self.Append(refer + ";");
                self.Append(monto.ToString() + ";");
                self.Append(concepto + ";");
                self.AppendLine(email);
            }
            return self.ToString();
        }
    }

    public class EfectivoEnCaja : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        string tipo = _settings.EfeCaja_TipoID;
        string id = _settings.EfeCaja_ID;
        string beneficiario = _settings.EfeCaja_Beneficiario;
        double monto = _settings.EfeCaja_Monto;
        ulong uniqrefer;
        string fecha = DateTime.Now.ToString("dd/MM/yyyy");
        string concepto = _settings.EfeCaja_Concepto;
        string registro = _settings.EfeCaja_Registro;
        string email = _settings.EfeCaja_Email;
        bool rng = _settings.EfeCaja_ValueRNG;
        bool acctrng = _settings.EfeCaja_TargetRNG;

        public static string DescriptiveName
        { get { return "Pago de efectivo en caja"; } }

        public string Generate(int amount)
        {
            uniqrefer = Utils.GetTimestamp(10);
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (acctrng)
                {
                    tipo = "C";
                    var client = Clients.GetRandomOtherClient();
                    id = client.Key;
                    beneficiario = client.Value;
                }
                self.Append(tipo + ";");
                self.Append(id + ";");
                self.Append(beneficiario + ";");
                self.Append(monto.ToString() + ";");
                self.Append((uniqrefer + (ulong)i).ToString() + ";");
                self.Append(fecha + ";");
                self.Append(concepto + ";");
                self.Append(registro + ";");
                self.AppendLine(email);
            }
            return self.ToString();
        }
    }

    public class PagoChequeAdm : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        string beneficiario = _settings.CkAdm_Beneficiario;
        double monto = _settings.CkAdm_Monto;
        ulong refer;
        string desc = _settings.CkAdm_Descripcion;
        string sucursal = _settings.CkAdm_Sucursal;
        string email = _settings.CkAdm_Email;
        bool rng = _settings.CkAdm_ValueRNG;
        bool acctrng = _settings.CkAdm_TargetRNG;

        public static string DescriptiveName
        { get { return "Solicitud pago cheque de administración"; } }

        public string Generate(int amount)
        {
            refer = Utils.GetTimestamp(10);
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (acctrng)
                { beneficiario = Clients.GetRandomOtherClient().Value; }
                self.Append("2;");
                self.Append(beneficiario + ";");
                self.Append(monto.ToString() + ";");
                self.Append(refer + ";");
                self.Append(desc + ";");
                self.Append(sucursal + ";");
                self.AppendLine(email);
            }
            return self.ToString();
        }
    }

    public class BloqYConfCheque : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        ulong numcheque;
        string numcuenta = _settings.BCCk_Cuenta;
        double monto = _settings.BCCk_Monto;
        string beneficiario = _settings.BCCk_Beneficiario;
        string fecha = DateTime.Now.ToString("dd/MM/yyyy");
        string concepto = _settings.BCCk_Concepto;
        string observ = _settings.BCCk_Observaciones;
        bool rng = _settings.BCCk_ValueRNG;
        bool benefrng = _settings.BCCk_TargetRNG;
        bool acctrng = _settings.BCCk_AcctRNG;
 
        public static string DescriptiveName
        { get { return "Bloqueo/Confirmación de cheque"; } }

        public string Generate(int amount)
        {
            numcheque = Utils.GetTimestamp(10);
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (benefrng)
                { beneficiario = Clients.GetRandomOtherClient().Value; }
                if (acctrng)
                { numcuenta = Utils.GetRandomAcctNumber(11); }
                self.Append((numcheque + (ulong)i).ToString() + ";");
                self.Append(numcuenta + ";");
                self.Append(monto.ToString() + ";");
                self.Append(beneficiario + ";");
                self.Append(fecha + ";");
                self.Append(concepto + ";");
                self.AppendLine(observ);
            }
            return self.ToString();
        }
    }

    public class TBHD : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        string nombre = _settings.TBHD_Beneficiario;
        double monto = _settings.TBHD_Monto;
        string refer = _settings.TBHD_RefTrx;
        string desc = _settings.TBHD_Descripcion;
        string tipocta = _settings.TBHD_TipoCta;
        string numcta = _settings.TBHD_NumCta;
        string email = _settings.TBHD_Email;
        string fax = _settings.TBHD_Fax;
        string refdeb = _settings.TBHD_RefDebito;
        bool rng = _settings.TBHD_ValueRNG;
        bool auto = _settings.TBHD_RefTrxRNG;
        bool acctrng = _settings.TBHD_TargetRNG;

        public static string DescriptiveName
        { get { return "Pago a tercero BHD León"; } }

        public string Generate(int amount)
        {
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (auto) { refer = Utils.GetRandomReference(10); }
                if (acctrng)
                {
                    tipocta = "CA";
                    var client = Clients.GetRandomBHDClient();
                    numcta = client.Key;
                    nombre = client.Value;
                }
                self.Append("1;");
                self.Append(nombre + ";");
                self.Append(monto.ToString() + ";");
                self.Append(refer + ";");
                self.Append(desc + ";");
                self.Append(tipocta + ";");
                self.Append(numcta + ";");
                self.Append(email + ";" + fax + ";");
                self.AppendLine(refdeb);
            }
            return self.ToString();
        }
    }

    public class ACH : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        string numcta = _settings.ACH_NumCta;
        string banco = _settings.ACH_Banco;
        string tipocta = _settings.ACH_TipoCta;
        string nombre = _settings.ACH_Beneficiario;
        string tipomov = _settings.ACH_TipoMov;
        double monto = _settings.ACH_Monto;
        string refer = _settings.ACH_RefTrx;
        string desc = _settings.ACH_Descripcion;
        string email = _settings.ACH_Email;
        string fax = _settings.ACH_Fax;
        bool rng = _settings.ACH_ValueRNG;
        bool auto = _settings.ACH_RefTrxRNG;
        bool acctrng = _settings.ACH_TargetRNG;

        public static string DescriptiveName
        { get { return "Pago ACH"; } }

        public string Generate(int amount)
        {
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (auto) { refer = Utils.GetRandomReference(10); }
                if (acctrng)
                {
                    var client = Clients.GetRandomOtherClient();
                    numcta = client.Key;
                    nombre = client.Value;
                }
                self.Append(numcta + ";");
                self.Append(banco + ";");
                self.Append(tipocta + ";");
                self.Append(nombre + ";");
                self.Append(tipomov + ";");
                self.Append(monto.ToString() + ";");
                self.Append(refer + ";");
                self.Append(desc + ";");
                self.AppendLine(email + ";" + fax);
            }
            return self.ToString();
        }
    }

    public class LBTR : ITemplate
    {
        static readonly Config _settings = Utils.GetSettings();
        string numcta = _settings.LBTR_NumCta;
        string banco = _settings.LBTR_Banco;
        string tipocta = _settings.LBTR_TipoCta;
        string nombre = _settings.LBTR_Beneficiario;
        string tipomov = _settings.LBTR_TipoMov;
        double monto = _settings.LBTR_Monto;
        string refer = _settings.LBTR_RefTrx;
        string desc = _settings.LBTR_Descripcion;
        string email = _settings.LBTR_Email;
        string fax = _settings.LBTR_Fax;
        string tipoid = _settings.LBTR_TipoID;
        string id = _settings.LBTR_ID;
        bool rng = _settings.LBTR_ValueRNG;
        bool auto = _settings.LBTR_RefTrxRNG;
        bool acctrng = _settings.LBTR_TargetRNG;

        public static string DescriptiveName
        { get { return "Pago al instante BCRD"; } }

        public string Generate(int amount)
        {
            StringBuilder self = new StringBuilder();
            for (int i = 0; i < amount; i++)
            {
                if (rng) { monto = Utils.GetRandomAmount(); }
                if (auto) { refer = Utils.GetRandomReference(10); }
                if (acctrng)
                {
                    numcta = Utils.GetRandomAcctNumber(9);
                    tipoid = "C";
                    var client = Clients.GetRandomOtherClient();
                    id = client.Key;
                    nombre = client.Value;
                }
                self.Append(numcta + ";");
                self.Append(banco + ";");
                self.Append(tipocta + ";");
                self.Append(nombre + ";");
                self.Append(tipomov + ";");
                self.Append(monto.ToString() + ";");
                self.Append(refer + ";");
                self.Append(desc + ";");
                self.Append(email + ";");
                self.Append(fax + ";");
                self.Append(tipoid + ";");
                self.AppendLine(id);
            }
            return self.ToString();
        }
    }
}
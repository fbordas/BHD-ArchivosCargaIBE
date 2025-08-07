#region [ usings/imports ]
using FileTemplates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Config = MassTemplateGenerator.Properties.Settings;
using ConfigProp = System.Configuration.SettingsProperty;
using Remoting = System.Runtime.Remoting;
#endregion

/// <summary>
/// Wrapper for various data processing functions and tools.
/// </summary>
namespace DataProcessing
{
    /// <summary>
    /// The state of the disk writing operation.
    /// </summary>
    public enum OperationState
    {
        Success = 0,
        IOException = 1,
        OtherException = 2
    }

    /// <summary>
    /// Manages client data from embedded files.
    /// </summary>
    public class ClientData
    {
        #region [ members ]
        /// <summary>
        /// Internal test client information is stored here.
        /// </summary>
        private static Dictionary<string, string> _bhdlc =
            new Dictionary<string, string>();

        /// <summary>
        /// External test client information is stored here.
        /// </summary>
        private static Dictionary<string, string> _othc =
            new Dictionary<string, string>();

        /// <summary>
        /// Random number generator for accessing collections.
        /// </summary>
        private static Random _rng = new Random();
        #endregion


        #region [ client data management ]
        /// <summary>
        /// Loads test client information from the embedded resource
        /// data files.
        /// </summary>
        internal static void LoadClientData()
        {
            using (var xmlStream = typeof(DataFunctions).Assembly
                .GetManifestResourceStream("bhdldata"))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlStream);
                foreach (XmlNode item in xdoc.DocumentElement)
                {
                    _bhdlc.Add(item.Attributes.GetNamedItem("Account")
                        .InnerText, item.InnerText);
                }
            }
            using (var xmlStream = typeof(DataFunctions).Assembly
                .GetManifestResourceStream("otherdata"))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlStream);
                foreach (XmlNode item in xdoc.DocumentElement)
                {
                    _othc.Add(item.Attributes.GetNamedItem("ID")
                        .InnerText, item.InnerText);
                }
            }
        }

        /// <summary>
        /// Gets a random client from the embedded internal clients database.
        /// </summary>
        /// <returns>A <see cref="KeyValuePair{TKey, TValue}"/> containing
        /// the account number and the client name selected at random from
        /// the available client collection.</returns>
        internal static KeyValuePair<string, string> GetRandomBHDClient()
        { return _bhdlc.ElementAt(_rng.Next(0, _bhdlc.Count)); }



        /// <summary>
        /// Gets a random non-internal client from the embedded client
        /// database.
        /// </summary>
        /// <returns>A <see cref="KeyValuePair{TKey, TValue}"/> containing
        /// the personal ID number and the client name selected at random
        /// from the available client collection.</returns>
        internal static KeyValuePair<string, string> GetRandomOtherClient()
        { return _othc.ElementAt(_rng.Next(0, _othc.Count)); }
        #endregion
    }

    /// <summary>
    /// Contains static data utility functions for general use.
    /// </summary>
    public class DataFunctions
    {
        #region [ members ]
        private static Random _rng = new Random();
        #endregion



        #region [ i/o-related functions ]
        /// <summary>
        /// Creates a temporary HTML file with the application changelog to
        /// be opened in a web browser.
        /// </summary>
        /// <returns>The path of the temporary file to open.</returns>
        public static string GetTempLogPath()
        {
            string html = MassTemplateGenerator.Properties.Resources.chlog;
            string path = Path.ChangeExtension(Path.GetTempFileName(), ".html");
            File.WriteAllText(path, html);
            return path;
        }


        /// <summary>
        /// Simple path validation. Verifies a specified path to a file
        /// is "valid" (exists, or can be created) before allowing access
        /// to the file.
        /// </summary>
        /// <param name="path">The user-specified path check.</param>
        /// <returns>True if the path is valid, false otherwise.</returns>
        internal static bool ValidatePath(string path)
        {
            if (!path.Contains("\\")) return false;
            string folder = path.Substring(0, path.LastIndexOf("\\"));
            if (folder.EndsWith("\\"))
            { folder = folder.Substring(0, folder.Length - 2); }
            if (!Directory.Exists(folder)) return false;

            return true;
        }



        /// <summary>
        /// Internal method for logging exception data to disk for
        /// future issue tracing and debugging.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        internal static void ExceptionLog(Exception ex)
        {
            using (StreamWriter logger = new StreamWriter("BHDLMassGen.log", true))
            {
                StringBuilder logdump = new StringBuilder();
                logdump.Append("[" + DateTime.Now.Year.ToString() +
                    GetTimestamp().ToString() + "] ");
                logdump.AppendLine(ex.ToString());
                logger.Flush(); logger.Close();
            }
        }



        /// <summary>
        /// Writes text content to disk in the specified filepath.
        /// </summary>
        /// <param name="contents">The text content to write to disk.</param>
        /// <param name="targetfile">The path of the file to write the content to.</param>
        /// <returns><see cref="OperationState"/></returns>
        internal static OperationState WriteToDisk(string contents, string targetfile)
        {
            OperationState opState = OperationState.Success;
            using (StreamWriter writer = new StreamWriter(targetfile))
            {
                try
                { writer.WriteLine(contents.Trim()); }
                catch (IOException ioex)
                { ExceptionLog(ioex); opState = OperationState.IOException; }
                catch (Exception ex)
                { ExceptionLog(ex); opState = OperationState.OtherException; }
                finally
                { writer.Flush(); writer.Close(); }
            }
            return opState;
        }



        /// <summary>
        /// Export the current settings to an easily shareable json file.
        /// </summary>
        /// <param name="targetfile">The name of the exported file.</param>
        /// <returns><see cref="OperationState">Success</see> if the 
        /// exporting operation was successful. If any errors occur, they
        /// will be logged in the default log file.</returns>
        internal static OperationState ExportSettings(string targetfile)
        {
            OperationState opState = OperationState.Success;
            var serializer = new JsonSerializer();
            var sw = new StreamWriter(targetfile);
            var jw = new JsonTextWriter(sw);
            var settings = Config.Default;
            Dictionary<string, string> currentConfig =
                new Dictionary<string, string>();
            foreach (ConfigProp s in settings.Properties)
            { currentConfig.Add(s.Name, settings[s.Name].ToString()); }
            try
            { serializer.Serialize(jw, currentConfig); }
            catch (IOException ioex)
            { ExceptionLog(ioex); opState = OperationState.IOException; }
            catch (Exception ex)
            { ExceptionLog(ex); opState = OperationState.OtherException; }
            finally { sw.Flush(); jw.Flush(); sw.Close(); jw.Close(); }
            return opState;
        }



        /// <summary>
        /// Import settings from a json file into the current application settings.
        /// </summary>
        /// <param name="sourcefile">The json file to import.</param>
        /// <returns>True if importing was successful and app settings were
        /// overwritten with the imported data; false otherwise.</returns>
        internal static bool ImportSettings(string sourcefile)
        {
            var settings = Config.Default;
            bool success = true;
            var sr = new StreamReader(sourcefile);
            try
            {
                Dictionary<string, string> newConfig =
                    JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(sr.ReadToEnd());
                foreach (var s in newConfig)
                { settings[s.Key] = Convert.ChangeType(s.Value,
                    settings[s.Key].GetType()); }
                settings.Save();
            }
            catch (IOException ioex) { ExceptionLog(ioex); success = false; }
            catch (Exception ex) { ExceptionLog(ex); success = false; }
            finally { sr.Close(); }
            return success;
        }
        #endregion



        #region [ template-handling ]
        /// <summary>
        /// Fetches a collection of all the available templates.
        /// </summary>
        /// <returns>An <seealso cref="IEnumerable{T}"/> of all
        /// the templates found.</returns>
        internal static IEnumerable<string> GetAllTemplateNames()
        {
            var temps = typeof(DataFunctions).Assembly.GetTypes()
                .Where(t => typeof(ITemplate).IsAssignableFrom(t)
                && t.IsInterface == false);
            List<string> names = new List<string>();
            foreach (var t in temps)
            {
                names.Add((string)t.GetProperty
                    ("DescriptiveName").GetValue(t, null));
            }
            return names;
        }



        /// <summary>
        /// Fetches a specific template given the descriptive template name.
        /// </summary>
        /// <param name="name">The descriptive template name.</param>
        /// <returns>The file template to use for generating entries.</returns>
        internal static ITemplate GetTemplateFromDescriptiveName(string name)
        {
            var tp = typeof(DataFunctions).Assembly.GetTypes()
                .Where(t => typeof(ITemplate).IsAssignableFrom(t)
                && t.IsInterface == false && t.GetProperty("DescriptiveName")
                .GetValue(t, null).ToString() == name).FirstOrDefault();
            Remoting.ObjectHandle oh = Activator.CreateInstance
                (typeof(DataFunctions).Assembly.FullName, tp.FullName);
            return (ITemplate)Activator.CreateInstance
                (oh.Unwrap().GetType(), null);
        }
        #endregion



        #region [ miscellaneous accessors & getters ]
        /// <summary>
        /// Generates a random alphanumeric string of a given length.
        /// </summary>
        /// <param name="length">The desired length of the string.</param>
        /// <returns>A string of random alphanumeric characters of the specified
        /// length.</returns>
        internal static string GetRandomReference(int length)
        {
            const string chars = "ABCDEFGHIJKLNMOPQRSTUVWXYZabcdefghijklnm" +
                "opqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_rng.Next(s.Length)]).ToArray());
        }


        /// <summary>
        /// Returns a random numerical account number, represented as a
        /// string, of the specified character length.
        /// </summary>
        /// <param name="length">The desired length of the number.</param>
        /// <returns>A string representation of an account number.</returns>
        internal static string GetRandomAcctNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_rng.Next(s.Length)]).ToArray());
        }

        

        /// <summary>
        /// Gets a random number between 0.01 and 999,999.99.
        /// </summary>
        /// <returns>A random value between 0.01 and 999,999.99</returns>
        internal static double GetRandomAmount()
        {
            double max = Config.Default.AppMaxCashValue;
            return _rng.Next(1, (int)(max * 100.00)) / 100.00;
        }



        /// <summary>
        /// Gets a timestamp to be used as an unique value for
        /// fields that require it.
        /// </summary>
        /// <param name="length">Optional: specifies a max length
        /// for the timestamp.</param>
        /// <returns>A large integer with an unique value based
        /// on the current system time..</returns>
        internal static ulong GetTimestamp(int length = 0)
        {
            string s = DateTime.Now.ToString("MMddHHmmssffffff");
            int offset = length == 0 ? length : s.Length - length;
            return ulong.Parse(s.Substring(offset));
        }



        /// <summary>
        /// Fetches the assembly configuration file.
        /// </summary>
        /// <returns>The current assembly configuration file.</returns>
        internal static Config GetSettings()
        { return Config.Default; }



        /// <summary>
        /// Resets the application settings to their default value, while
        /// keeping record that the application is no longer being run for
        /// the first time.
        /// </summary>
        internal static void ResetSettings()
        {
            var settings = Config.Default;
            foreach (ConfigProp p in settings.Properties)
            {
                string name = p.Name;
                if (name == "AppFirstRun") { continue; }
                settings[name] = Convert.ChangeType(p.DefaultValue, p.PropertyType);
            }
            settings.AppFirstRun = false;
            settings.Save();
        }



        /// <summary>
        /// Fetches the location of the log file to display to the user.
        /// </summary>
        /// <returns>The fixed location of the log file, which will always
        /// be the same as that of the running executable file.</returns>
        internal static string GetErrorLogLocation()
        {
            return Path.Combine(Path.GetDirectoryName
                (typeof(DataFunctions).Assembly.Location), "BHDLMassGen.log");
        }
        #endregion
    }
}

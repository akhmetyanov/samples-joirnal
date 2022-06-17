using SamplesJournal_v2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SamplesJournal_v2
{
    public static class DataBaseFactory
    {
        private static DataBase dataBase;

        public static DataBase DataBase 
        { 
            get 
            { 
                if (dataBase == null)
                {
                    dataBase = new DataBase(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SamplesJurnalDb.db3")  
                        );
                }

                return dataBase;
            }
        }
    }
}

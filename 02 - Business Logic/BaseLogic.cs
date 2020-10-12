using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhadsCarRental
{
    /// <summary>
    /// Represents the basic needs for a logic class!
    /// </summary>
    public abstract class BaseLogic : IDisposable
    {

        /// <summary>
        /// Holds the database!
        /// </summary>
        protected OhadsCarRentalEntities DB = new OhadsCarRentalEntities();

        /// <summary>
        /// Clears database resources!
        /// </summary>
        public void Dispose()
        {
            DB.Dispose();
        }

    }
}

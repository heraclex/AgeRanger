﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.DbContext.Entities;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Reflection;
using System.Data.Entity;

namespace AgeRanger.DbContext
{
    public class AgeRangerDbContext : System.Data.Entity.DbContext
    {
        public AgeRangerDbContext()
            : this("DefaultDb")
        {
        }

        public AgeRangerDbContext(string connectionString)
            : base(new SQLiteConnection() { ConnectionString = connectionString }, true)
        {
            /*
             * http://social.msdn.microsoft.com/Forums/en-US/dc5f5082-3fb8-45e6-b912-141235527f8a/invalid-object-name-dboedmmetadata-and-dbomigrationhistory?forum=adodotnetentityframework
             * When a DbContext is first created, a configurable DatabaseInitializer is run to find or create the database. 
             * The default DatabaseInitializer tries to compare the database schema needed to back the model with a hash of the schema stored in that EdmMetadata table that is created with a database. 
             * Existing databases won’t have the EdmMetadata table and so won’t have the hash…and our implementation today will throw if that table is missing. It sounds like we need to change this behavior. 
             * Until then, existing databases do not generally need any database initializer so it can be turned off for our context type by calling:
             */
            Database.SetInitializer<AgeRangerDbContext>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 300;
            this.Configure();
        }

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Db Context Configuration
        /// </summary>
        private void Configure()
        {
            https://msdn.microsoft.com/en-us/data/dn469464.aspx
            this.Database.Log = this.WriteLog;
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = true;
        }

        private void WriteLog(string mess)
        {
            if (string.IsNullOrEmpty(mess) || mess.Equals("\r\n"))
            {
                return;
            }

            //TODO: Add Db Execution log here
        }
    }
}

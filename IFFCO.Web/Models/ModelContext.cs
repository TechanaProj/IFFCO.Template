using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Devart.Data.Oracle;
using IFFCO.HRMS.Repository.Pattern.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using IFFCO.HRMS.Entities.AppConfig;
using IFFCO.HRMS.Repository.Pattern;

namespace IFFCO.NERRS.Web.Models
{
    public partial class ModelContext : DbContext
    {
        readonly string conn = new AppConfiguration().ConnectionString;
        readonly string SchemaName = new AppConfiguration().SchemaName;
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmEmporaMap> AdmEmporaMap { get; set; }
        public virtual DbSet<AdmEmpprgAccess> AdmEmpprgAccess { get; set; }
        public virtual DbSet<AdmEmpUnitAccess> AdmEmpUnitAccess { get; set; }
        public virtual DbSet<AdmPrgMaster> AdmPrgMaster { get; set; }
        public virtual DbSet<AdmProjmodMaster> AdmProjmodMaster { get; set; }
        public virtual DbSet<AdmSubMenuMsts> AdmSubMenuMsts { get; set; }
        public virtual DbSet<FAllotmentRentDtls> FAllotmentRentDtls { get; set; }
        public virtual DbSet<FFinalAllot> FFinalAllot { get; set; }
        public virtual DbSet<FIntCompute> FIntCompute { get; set; }
        public virtual DbSet<MOccupantMsts> MOccupantMsts { get; set; }
        public virtual DbSet<MQuarterTypeMsts> MQuarterTypeMsts { get; set; }
        public virtual DbSet<MRentMsts> MRentMsts { get; set; }
        public virtual DbSet<MVendorMsts> MVendorMsts { get; set; }


        public DataTable GetSQLQuery(string sqlquery)
        {
            DataTable dt = new DataTable();

            OracleConnection connection = new OracleConnection(conn);

            OracleDataAdapter oraAdapter = new OracleDataAdapter(sqlquery, connection);

            OracleCommandBuilder oraBuilder = new OracleCommandBuilder(oraAdapter);

            oraAdapter.Fill(dt);

            return dt;
        }

        public byte[] GetByteArray(object value)
        {
            if (value is byte[] byteArray)
            {
                return byteArray; // Already a byte array, no conversion needed
            }
            else if (value is string stringValue)
            {
                // Convert string to byte array using a specific encoding, such as UTF-8
                return Encoding.UTF8.GetBytes(stringValue);
            }

            // Return null or throw an exception depending on your requirements
            return null;
        }

        public int insertUpdateToDB(string sql)
        {
            OracleConnection connection = new OracleConnection(conn);
            OracleCommand cmd = new OracleCommand();
            int i = 0;
            try
            {
                cmd.CommandText = sql.ToString();
                cmd.Connection = connection;
                connection.Open();
                i = cmd.ExecuteNonQuery();
                connection.Close();
                return i;
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return i = 0;
            }
        }

        public int GetScalerFromDB(string sql)
        {
            OracleConnection connection = new OracleConnection(conn);
            OracleCommand cmd = new OracleCommand();
            int i = 0;
            try
            {
                cmd.CommandText = sql.ToString();
                cmd.Connection = connection;
                connection.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return i = 0;
            }
        }

        public string GetCharScalerFromDB(string sql)
        {
            OracleConnection connection = new OracleConnection(conn);
            OracleCommand cmd = new OracleCommand();
            int i = 0;
            try
            {
                cmd.CommandText = sql.ToString();
                cmd.Connection = connection;
                connection.Open();
                string result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return null;
            }
        }

        public int ExecuteProcedure(string procedure, params object[] parameters)
        {
            List<OracleParameter> oracleParameterList = ((List<OracleParameter>)parameters[0]);
            string[] oracleParameters = new string[oracleParameterList.Count];
            string query = "BEGIN " + procedure + "(";
            for (int i = 0; i < oracleParameterList.Count; i++)
            {
                OracleParameter parameter = oracleParameterList[i] as OracleParameter;
                oracleParameters[i] = ":" + parameter.ParameterName;
            }
            query += string.Join(",", oracleParameters);
            query += "); end;";
            //Database.OpenConnection()
            return Database.ExecuteSqlCommand(query, oracleParameterList);
        }

        public int ExecuteProcedureForRefCursor(string procedure, params object[] parameters)
        {
            List<OracleParameter> oracleParameterList = ((List<OracleParameter>)parameters[0]);
            string[] oracleParameters = new string[oracleParameterList.Count];
            string query = "BEGIN " + procedure + "(";
            for (int i = 0; i < oracleParameterList.Count; i++)
            {
                OracleParameter parameter = oracleParameterList[i] as OracleParameter;
                oracleParameters[i] = ":" + parameter.ParameterName;
            }
            query += string.Join(",", oracleParameters);
            query += "); end;";


            Database.OpenConnection();
            int x = Database.ExecuteSqlCommand(query, oracleParameterList);
            return x;
        }

        public async Task<int> ExecuteProcedureAsync(string procedure, params object[] parameters)
        {
            List<OracleParameter> oracleParameterList = ((List<OracleParameter>)parameters[0]);
            string[] oracleParameters = new string[oracleParameterList.Count];
            string query = "BEGIN " + procedure + "(";
            for (int i = 0; i < oracleParameterList.Count; i++)
            {
                OracleParameter parameter = oracleParameterList[i] as OracleParameter;
                oracleParameters[i] = ":" + parameter.ParameterName;
            }
            query += string.Join(",", oracleParameters);
            query += "); end;";
            //Database.OpenConnection()
            return await Database.ExecuteSqlCommandAsync(query, oracleParameterList);
        }

        public Task<int> ExecuteProcedureAsync<TElement>(string procedure, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteQuery<TElement>(string query)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteQueryAsync<TElement>(string query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> ExecuteSelectProcedure<TElement>(string procedure, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> ExecuteSelectQuery<TElement>(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }
        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("SERVICE NAME=ORACLE;Direct=true; License Key=vEyr8GdnIarOKlEHQKxi+4E0HlXN85PVGHI096M18fHO05syciZT/8xvOeNbTwuMbqdZkRZ1qbdPjO13mrnBnlMUyskKr9qbBNMTzJAp5+R858T7YUZaTY5rodcDl7pDutJBeuYiwHG+xtXnywKMPX+9u82fR1AMT9EailpEiBp1OAn6IbJ55eXY15+rsAfDDwUuIv/js610S6cy9vLt37IL4PcZ8Wx/MrQlA38Z+kEH9Wztcv+NSWFVRz2wnVRDtIowaySSKk30sA+MBbg2IIUI+/MgDUp6w53NCxQSsuM=; User Id=NERRS;Password=nerrs_123; Data Source= iffcoexadr-92rdq-scan.drhyddbcltsn01.drhydebsprodvcn.oraclevcn.com:1521/IFFCOAL.drhyddbcltsn01.drhydebsprodvcn.oraclevcn.com;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmEmporaMap>(entity =>
            {
                entity.HasKey(e => e.Empid);

                entity.ToTable("ADM_EMPORA_MAP", "NERRS");

                entity.HasIndex(e => e.Empid)
                    .HasName("PK_ADMEMPORA")
                    .IsUnique();

                entity.Property(e => e.Empid).HasColumnName("EMPID");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.EmailId)
                    .HasColumnName("EMAIL_ID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(50);

                entity.Property(e => e.Empname)
                    .HasColumnName("EMPNAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(50);

                entity.Property(e => e.Emppwd)
                    .HasColumnName("EMPPWD")
                    .HasColumnType("varchar2")
                    .HasMaxLength(255);

                entity.Property(e => e.MobileNo).HasColumnName("MOBILE_NO");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("MODIFIED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Orausername)
                    .IsRequired()
                    .HasColumnName("ORAUSERNAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.RepUnit)
                    .HasColumnName("REP_UNIT")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<AdmEmpprgAccess>(entity =>
            {
                entity.HasKey(e => new { e.Empid, e.Projectid, e.Moduleid, e.Programid, e.Programtype });

                entity.ToTable("ADM_EMPPRG_ACCESS", "NERRS");

                entity.HasIndex(e => new { e.Empid, e.Projectid, e.Moduleid, e.Programtype, e.Programid })
                    .HasName("PK_ADMEMPPRGACS")
                    .IsUnique();

                entity.Property(e => e.Empid)
                    .HasColumnName("EMPID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.Projectid)
                    .HasColumnName("PROJECTID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.Moduleid)
                    .HasColumnName("MODULEID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(20);

                entity.Property(e => e.Programid)
                    .HasColumnName("PROGRAMID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.Programtype)
                    .HasColumnName("PROGRAMTYPE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("MODIFIED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.PrivDelete)
                    .HasColumnName("PRIV_DELETE")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.PrivInsert)
                    .HasColumnName("PRIV_INSERT")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.PrivSelect)
                    .HasColumnName("PRIV_SELECT")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.PrivUpdate)
                    .HasColumnName("PRIV_UPDATE")
                    .HasColumnType("char")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<AdmEmpUnitAccess>(entity =>
            {
                entity.HasKey(e => new { e.Empid, e.Moduleid, e.UnitCode });

                entity.ToTable("ADM_EMP_UNIT_ACCESS", "NERRS");

                entity.HasIndex(e => new { e.Empid, e.Moduleid, e.UnitCode })
                    .HasName("ADM_EMP_UNIT_ACCESS_PK")
                    .IsUnique();

                entity.Property(e => e.Empid).HasColumnName("EMPID");

                entity.Property(e => e.Moduleid)
                    .HasColumnName("MODULEID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(20);

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.AllDeptAccess)
                    .HasColumnName("ALL_DEPT_ACCESS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.AllSectionAccess)
                    .HasColumnName("ALL_SECTION_ACCESS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("char")
                    .HasMaxLength(30);

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.DefaultUnit)
                    .IsRequired()
                    .HasColumnName("DEFAULT_UNIT")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.HierYn)
                    .IsRequired()
                    .HasColumnName("HIER_YN")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("char")
                    .HasMaxLength(30);

                entity.Property(e => e.OnlyAreaAccess)
                    .HasColumnName("ONLY_AREA_ACCESS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.Projectid)
                    .HasColumnName("PROJECTID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<AdmPrgMaster>(entity =>
            {
                entity.HasKey(e => new { e.Projectid, e.Moduleid, e.Programtype, e.Programid });

                entity.ToTable("ADM_PRG_MASTER", "NERRS");

                entity.HasIndex(e => new { e.Projectid, e.Moduleid, e.Programtype, e.Programid })
                    .HasName("PK_ADMPRGMAS")
                    .IsUnique();

                entity.Property(e => e.Projectid)
                    .HasColumnName("PROJECTID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.Moduleid)
                    .HasColumnName("MODULEID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(20);

                entity.Property(e => e.Programtype)
                    .HasColumnName("PROGRAMTYPE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.Programid)
                    .HasColumnName("PROGRAMID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.ActiveInactive)
                    .HasColumnName("ACTIVE_INACTIVE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(5);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.Ismainform)
                    .HasColumnName("ISMAINFORM")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("MODIFIED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Programname)
                    .IsRequired()
                    .HasColumnName("PROGRAMNAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(255);

                entity.Property(e => e.SubMenuName)
                    .HasColumnName("SUB_MENU_NAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<AdmProjmodMaster>(entity =>
            {
                entity.HasKey(e => new { e.Projectid, e.Moduleid });

                entity.ToTable("ADM_PROJMOD_MASTER", "NERRS");

                entity.HasIndex(e => new { e.Projectid, e.Moduleid })
                    .HasName("PK_ADMPROJMOD")
                    .IsUnique();

                entity.Property(e => e.Projectid)
                    .HasColumnName("PROJECTID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.Moduleid)
                    .HasColumnName("MODULEID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CREATED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ErpPrefix)
                    .HasColumnName("ERP_PREFIX")
                    .HasColumnType("varchar2")
                    .HasMaxLength(4);

                entity.Property(e => e.GstCompliant)
                    .HasColumnName("GST_COMPLIANT")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.Migrated)
                    .HasColumnName("MIGRATED")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("MODIFIED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Modulename)
                    .IsRequired()
                    .HasColumnName("MODULENAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(255);

                entity.Property(e => e.ReadOnly)
                    .HasColumnName("READ_ONLY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<AdmSubMenuMsts>(entity =>
            {
                entity.HasKey(e => new { e.Moduleid, e.SubMenuId });

                entity.ToTable("ADM_SUB_MENU_MSTS", "NERRS");

                entity.HasIndex(e => new { e.Moduleid, e.SubMenuId })
                    .HasName("ADM_SUB_MENU_MSTS_PK")
                    .IsUnique();

                entity.Property(e => e.Moduleid)
                    .HasColumnName("MODULEID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(100);

                entity.Property(e => e.SubMenuId)
                    .HasColumnName("SUB_MENU_ID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(100);

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.ParentMenuId)
                    .HasColumnName("PARENT_MENU_ID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(100);

                entity.Property(e => e.Projectid)
                    .HasColumnName("PROJECTID")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.SubMenuName)
                    .HasColumnName("SUB_MENU_NAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<FAllotmentRentDtls>(entity =>
            {
                entity.HasKey(e => new { e.UnitCode, e.QuarterNo, e.SlNo, e.AllotmentNo, e.RentCode });

                entity.ToTable("F_ALLOTMENT_RENT_DTLS", "NERRS");

                entity.HasIndex(e => new { e.QuarterNo, e.SlNo, e.AllotmentNo, e.RentCode, e.UnitCode })
                    .HasName("F_ALLOTMENT_RENT_DTLS_PK")
                    .IsUnique();

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.QuarterNo).HasColumnName("QUARTER_NO");

                entity.Property(e => e.SlNo).HasColumnName("SL_NO");

                entity.Property(e => e.AllotmentNo).HasColumnName("ALLOTMENT_NO");

                entity.Property(e => e.RentCode)
                    .HasColumnName("RENT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.AcInstallationDate)
                    .HasColumnName("AC_INSTALLATION_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.AllotmentCancelDate)
                    .HasColumnName("ALLOTMENT_CANCEL_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.AllotmentDate)
                    .HasColumnName("ALLOTMENT_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.RentFromDate)
                   .HasColumnName("RENT_FROM_DATE")
                   .HasColumnType("date");

                entity.Property(e => e.RentToDate)
                   .HasColumnName("RENT_TO_DATE")
                   .HasColumnType("date");

                entity.Property(e => e.ColonyCode).HasColumnName("COLONY_CODE");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.ElecRate).HasColumnName("ELEC_RATE");

                entity.Property(e => e.ElectricityCount)
                    .HasColumnName("ELECTRICITY_COUNT")
                    .HasAnnotation("Scale", 0);

                entity.Property(e => e.ExpectedVacancyDate)
                    .HasColumnName("EXPECTED_VACANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ExtensionApprovedBy).HasColumnName("EXTENSION_APPROVED_BY");

                entity.Property(e => e.ExtensionApprovedDate)
                    .HasColumnName("EXTENSION_APPROVED_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ExtensionCategory)
                    .HasColumnName("EXTENSION_CATEGORY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.ExtensionFromDate)
                    .HasColumnName("EXTENSION_FROM_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ExtensionToDate)
                    .HasColumnName("EXTENSION_TO_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.FloorNo).HasColumnName("FLOOR_NO");

                entity.Property(e => e.MarketHrrFromDate)
                    .HasColumnName("MARKET_HRR_FROM_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.MarketHrrRate)
                    .HasColumnName("MARKET_HRR_RATE")
                    .HasColumnType("double");

                entity.Property(e => e.MarketHrrToDate)
                    .HasColumnName("MARKET_HRR_TO_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.MonthDayType)
                    .HasColumnName("MONTH_DAY_TYPE")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.NoOfAcs)
                    .HasColumnName("NO_OF_ACS")
                    .HasColumnType("double");

                entity.Property(e => e.NoOfDays)
                   .HasColumnName("NO_OF_DAYS")
                   .HasColumnType("double");

                entity.Property(e => e.NoOfBeds).HasColumnName("NO_OF_BEDS");

                entity.Property(e => e.NormalHrrRate)
                    .HasColumnName("NORMAL_HRR_RATE")
                    .HasColumnType("double");

                entity.Property(e => e.NormalHrrToDate)
                    .HasColumnName("NORMAL_HRR_TO_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.NormarlHrrFromDate)
                    .HasColumnName("NORMARL_HRR_FROM_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.OccupancyDate)
                    .HasColumnName("OCCUPANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.OccupantCode)
                    .HasColumnName("OCCUPANT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.PayrollProcessStatus)
                    .HasColumnName("PAYROLL_PROCESS_STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.PayrollYearmonth).HasColumnName("PAYROLL_YEARMONTH");

                entity.Property(e => e.PenalHrrFromDate)
                    .HasColumnName("PENAL_HRR_FROM_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.PenalHrrRate)
                    .HasColumnName("PENAL_HRR_RATE")
                    .HasColumnType("double");

                entity.Property(e => e.PenalHrrToDate)
                    .HasColumnName("PENAL_HRR_TO_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.PersonalNo).HasColumnName("PERSONAL_NO");

                entity.Property(e => e.QuarterCategory)
                    .IsRequired()
                    .HasColumnName("QUARTER_CATEGORY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(3);

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.RepUnit)
                    .HasColumnName("REP_UNIT")
                    .HasColumnType("varchar2")
                    .HasMaxLength(200);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.VacancyDate)
                    .HasColumnName("VACANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.VendorCode)
                    .HasColumnName("VENDOR_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<FFinalAllot>(entity =>
            {
                entity.HasKey(e => new { e.UnitCode, e.AllotmentNo, e.ComputationRun });

                entity.ToTable("F_FINAL_ALLOT", "NERRS");

                entity.HasIndex(e => new { e.UnitCode, e.AllotmentNo, e.ComputationRun })
                    .HasName("FM_ALLOT_RENT_DTLS_PK")
                    .IsUnique();

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.AllotmentNo).HasColumnName("ALLOTMENT_NO");

                entity.Property(e => e.ComputationRun).HasColumnName("COMPUTATION_RUN");

                entity.Property(e => e.AllotmentDate)
                    .HasColumnName("ALLOTMENT_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.DaysRemaining).HasColumnName("DAYS_REMAINING");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.NoOfAcs)
                    .HasColumnName("NO_OF_ACS")
                    .HasColumnType("double");

                entity.Property(e => e.OccupancyDate)
                    .HasColumnName("OCCUPANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.OccupantCode)
                    .HasColumnName("OCCUPANT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.PersonalNo).HasColumnName("PERSONAL_NO");

                entity.Property(e => e.QuarterCategory)
                    .IsRequired()
                    .HasColumnName("QUARTER_CATEGORY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(3);

                entity.Property(e => e.QuarterNo).HasColumnName("QUARTER_NO");

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.RentCode)
                    .HasColumnName("RENT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.RepUnit)
                    .HasColumnName("REP_UNIT")
                    .HasColumnType("varchar2")
                    .HasMaxLength(200);

                entity.Property(e => e.SlNo).HasColumnName("SL_NO");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.TotalAmt)
                    .HasColumnName("TOTAL_AMT")
                    .HasColumnType("double");

                entity.Property(e => e.VacancyDate)
                    .HasColumnName("VACANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.VendorCode)
                    .HasColumnName("VENDOR_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<FIntCompute>(entity =>
            {
                entity.HasKey(e => new { e.UnitCode, e.AllotmentNo, e.ComputationRun });

                entity.ToTable("F_INT_COMPUTE", "NERRS");

                entity.HasIndex(e => new { e.UnitCode, e.AllotmentNo, e.ComputationRun })
                    .HasName("FM_ALLOTMENT_RENT_DTLS_PK")
                    .IsUnique();

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.AllotmentNo).HasColumnName("ALLOTMENT_NO");

                entity.Property(e => e.ComputationRun).HasColumnName("COMPUTATION_RUN");

                entity.Property(e => e.AllotmentDate)
                    .HasColumnName("ALLOTMENT_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.CurrentComputeAmount).HasColumnName("CURRENT_COMPUTE_AMOUNT");

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.DaysRemaining).HasColumnName("DAYS_REMAINING");

                entity.Property(e => e.ElectAmt).HasColumnName("ELECT_AMT");

                entity.Property(e => e.ElectRate).HasColumnName("ELECT_RATE");

                entity.Property(e => e.ElectUnit).HasColumnName("ELECT_UNIT");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.FromDate)
                    .HasColumnName("FROM_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.MonthDayType)
                    .HasColumnName("MONTH_DAY_TYPE")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.NoOfAcs)
                    .HasColumnName("NO_OF_ACS")
                    .HasColumnType("double");

                entity.Property(e => e.NxtFromDate)
                    .HasColumnName("NXT_FROM_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.NxtToDate)
                    .HasColumnName("NXT_TO_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.OccupancyDate)
                    .HasColumnName("OCCUPANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.OccupantCode)
                    .HasColumnName("OCCUPANT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.PersonalNo).HasColumnName("PERSONAL_NO");

                entity.Property(e => e.QuarterCategory)
                    .IsRequired()
                    .HasColumnName("QUARTER_CATEGORY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(3);

                entity.Property(e => e.QuarterNo).HasColumnName("QUARTER_NO");

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.RentCode)
                    .HasColumnName("RENT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.RepUnit)
                    .HasColumnName("REP_UNIT")
                    .HasColumnType("varchar2")
                    .HasMaxLength(200);

                entity.Property(e => e.SlNo).HasColumnName("SL_NO");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.ToDate)
                    .HasColumnName("TO_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.TotalAmt)
                    .HasColumnName("TOTAL_AMT")
                    .HasColumnType("double");

                entity.Property(e => e.VacancyDate)
                    .HasColumnName("VACANCY_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.VendorCode)
                    .HasColumnName("VENDOR_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<MOccupantMsts>(entity =>
            {
                entity.HasKey(e => new { e.UnitCode, e.OccupantCode });

                entity.ToTable("M_OCCUPANT_MSTS", "NERRS");

                entity.HasIndex(e => new { e.UnitCode, e.OccupantCode })
                    .HasName("M_OCCUPANT_MSTS_PK")
                    .IsUnique();

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.OccupantCode)
                    .HasColumnName("OCCUPANT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.OccupantType)
                    .IsRequired()
                    .HasColumnName("OCCUPANT_TYPE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(100);

                entity.Property(e => e.QuarterFor)
                    .HasColumnName("QUARTER_FOR")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.QuarterIssuedTo)
                    .HasColumnName("QUARTER_ISSUED_TO")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<MQuarterTypeMsts>(entity =>
            {
                entity.HasKey(e => new { e.UnitCode, e.QuarterType });

                entity.ToTable("M_QUARTER_TYPE_MSTS", "NERRS");

                entity.HasIndex(e => new { e.UnitCode, e.QuarterType })
                    .HasName("M_QUARTER_TYPE_MSTS_PK")
                    .IsUnique();

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.QuarterType)
                    .HasColumnName("QUARTER_TYPE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(2);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("varchar2")
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<MRentMsts>(entity =>
            {
                entity.HasKey(e => new { e.UnitCode, e.RentCode });

                entity.ToTable("M_RENT_MSTS", "NERRS");

                entity.HasIndex(e => new { e.UnitCode, e.RentCode })
                    .HasName("M_RENT_MSTS_PK")
                    .IsUnique();

                entity.Property(e => e.UnitCode).HasColumnName("UNIT_CODE");

                entity.Property(e => e.RentCode)
                    .HasColumnName("RENT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(5);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.DRates)
                    .HasColumnName("D_RATES")
                    .HasColumnType("double");

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.ElectStatus)
                    .HasColumnName("ELECT_STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.MonthDayType)
                    .HasColumnName("MONTH_DAY_TYPE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(5);

                entity.Property(e => e.QuarterIssuedTo)
                    .HasColumnName("QUARTER_ISSUED_TO")
                    .HasColumnType("char")
                    .HasMaxLength(1);

                entity.Property(e => e.Rates)
                    .HasColumnName("RATES")
                    .HasColumnType("double");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar2")
                    .HasMaxLength(1);

                entity.Property(e => e.TypeResiAccom)
                    .IsRequired()
                    .HasColumnName("TYPE_RESI_ACCOM")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<MVendorMsts>(entity =>
            {
                entity.HasKey(e => new { e.VendorCode, e.VendorSiteId, e.VendorSiteCode });

                entity.ToTable("M_VENDOR_MSTS", "NERRS");

                entity.HasIndex(e => new { e.VendorCode, e.VendorSiteId, e.VendorSiteCode })
                    .HasName("M_VENDOR_MSTS_PK")
                    .IsUnique();

                entity.Property(e => e.VendorCode)
                    .HasColumnName("VENDOR_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.VendorSiteId)
                    .HasColumnName("VENDOR_SITE_ID")
                    .HasAnnotation("Precision", 20)
                    .HasAnnotation("Scale", 0);

                entity.Property(e => e.VendorSiteCode)
                    .HasColumnName("VENDOR_SITE_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(320);

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.Country)
                    .HasColumnName("COUNTRY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.DatetimeCreated)
                    .HasColumnName("DATETIME_CREATED")
                    .HasColumnType("date");

                entity.Property(e => e.DatetimeModified)
                    .HasColumnName("DATETIME_MODIFIED")
                    .HasColumnType("date");

                entity.Property(e => e.EmployeeNo)
                    .HasColumnName("EMPLOYEE_NO")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.FullName)
                    .HasColumnName("FULL_NAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(240);

                entity.Property(e => e.HrmsUnitCd).HasColumnName("HRMS_UNIT_CD");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("MODIFIED_BY")
                    .HasColumnType("varchar2")
                    .HasMaxLength(30);

                entity.Property(e => e.State)
                    .HasColumnName("STATE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(150);

                entity.Property(e => e.UnitCode)
                    .HasColumnName("UNIT_CODE")
                    .HasColumnType("varchar2")
                    .HasMaxLength(4000);

                entity.Property(e => e.VendorId)
                    .HasColumnName("VENDOR_ID")
                    .HasAnnotation("Precision", 20)
                    .HasAnnotation("Scale", 0);

                entity.Property(e => e.VendorName)
                    .HasColumnName("VENDOR_NAME")
                    .HasColumnType("varchar2")
                    .HasMaxLength(320);
            });
        }

        internal void ExecuteProcedure(string v, List<OracleParameter> oracleParameterCollection)
        {
            throw new NotImplementedException();
        }
    }
}

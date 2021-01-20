using Backend.RuletaMasivian.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.RuletaMasivian.Repositories
{
    public partial class RuletaMasivianContext : DbContext
    {
        public RuletaMasivianContext()
        {
        }

        public RuletaMasivianContext(DbContextOptions<RuletaMasivianContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AprobacionRuletaMasivian> AprobacionRuletaMasivian { get; set; }
        public virtual DbSet<Campo> Campo { get; set; }
        public virtual DbSet<CampoContrato> CampoContrato { get; set; }
        public virtual DbSet<ClaseSistema> ClaseSistema { get; set; }
        public virtual DbSet<ClasificacionLahee> ClasificacionLahee { get; set; }
        public virtual DbSet<CompaniaAsociada> CompaniaAsociada { get; set; }
        public virtual DbSet<CompaniaOperadora> CompaniaOperadora { get; set; }
        public virtual DbSet<CompaniasAsociadasContrato> CompaniasAsociadasContrato { get; set; }
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<Cuenca> Cuenca { get; set; }
        public virtual DbSet<DatumSistemaCoordenadas> DatumSistemaCoordenadas { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<EstadoFacilidad> EstadoFacilidad { get; set; }
        public virtual DbSet<EstadoPozo> EstadoPozo { get; set; }
        public virtual DbSet<EstadosEscenarios> EstadosEscenarios { get; set; }
        public virtual DbSet<Facilidad> Facilidad { get; set; }
        public virtual DbSet<Falla> Falla { get; set; }
        public virtual DbSet<JerarquiaAdministrativa> JerarquiaAdministrativa { get; set; }
        public virtual DbSet<LeyRegalias> LeyRegalias { get; set; }
        public virtual DbSet<MetodoProduccion> MetodoProduccion { get; set; }
        public virtual DbSet<Mezcla> Mezcla { get; set; }
        public virtual DbSet<ModalidadCampoContrato> ModalidadCampoContrato { get; set; }
        public virtual DbSet<Municipios> Municipios { get; set; }
        public virtual DbSet<Paises> Paises { get; set; }
        public virtual DbSet<Pozo> Pozo { get; set; }
        public virtual DbSet<PozoYacimiento> PozoYacimiento { get; set; }
        public virtual DbSet<Proceso> Proceso { get; set; }
        public virtual DbSet<ProductoPrimario> ProductoPrimario { get; set; }
        public virtual DbSet<PuntoOrigen> PuntoOrigen { get; set; }
        public virtual DbSet<SistemaTransporte> SistemaTransporte { get; set; }
        public virtual DbSet<TipoFacilidad> TipoFacilidad { get; set; }
        public virtual DbSet<TipoPozo> TipoPozo { get; set; }
        public virtual DbSet<TipoPozoYacimiento> TipoPozoYacimiento { get; set; }
        public virtual DbSet<TipoProduccionPozoYacimiento> TipoProduccionPozoYacimiento { get; set; }
        public virtual DbSet<TiposCampo> TiposCampo { get; set; }
        public virtual DbSet<TiposContrato> TiposContrato { get; set; }
        public virtual DbSet<Yacimiento> Yacimiento { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<Sarta> Sarta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area", "RuletaMasivian");

                entity.HasIndex(e => e.NombreArea)
                    .HasName("UQ__Area__D5E8EEB57494EB0F")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreArea)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClaseSistema>(entity =>
            {
                entity.ToTable("ClaseSistema", "RuletaMasivian");

                entity.HasIndex(e => e.NombreClaseSistema)
                    .HasName("UQ__ClaseSis__0D7031D89E4B1586")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreClaseSistema)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AprobacionRuletaMasivian>().ToView("VW_RuletaMasivianPorAprobar", "RuletaMasivian").HasNoKey();
            modelBuilder.Entity<ClasificacionLahee>(entity =>
            {
                entity.ToTable("ClasificacionLAHEE", "RuletaMasivian");

                entity.HasIndex(e => e.WellClass)
                    .HasName("UQ__Clasific__021F77453CE04F87")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("ABBREVIATION")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LongName)
                    .HasColumnName("LONG_NAME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasColumnName("SHORT_NAME")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasColumnName("SOURCE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WellClass)
                    .HasColumnName("WELL_CLASS")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompaniaAsociada>(entity =>
            {
                entity.ToTable("CompaniaAsociada", "RuletaMasivian");

                entity.HasIndex(e => e.BaName)
                    .HasName("UQ__Compania__9E87D1FAEA2D5EB3")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaName)
                    .IsRequired()
                    .HasColumnName("ba_name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BaShortName)
                    .HasColumnName("ba_short_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessAssociate)
                    .HasColumnName("business_associate")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<CompaniasAsociadasContrato>(entity =>
            {
                entity.ToTable("CompaniasAsociadas_Contrato", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Cuenca>(entity =>
            {
                entity.ToTable("Cuenca", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.GeologicProvince)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.LongName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DatumSistemaCoordenadas>(entity =>
            {
                entity.ToTable("DatumSistemaCoordenadas", "RuletaMasivian");

                entity.HasIndex(e => e.NombreDatum)
                    .HasName("UQ__DatumSis__37C099FAF56F9265")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreDatum)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Departamentos>(entity =>
            {
                entity.ToTable("Departamentos", "RuletaMasivian");

                entity.HasIndex(e => e.NombreDepartamento)
                    .HasName("UQ__Departam__2B0383DCBD25BA80")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodigoDepartamento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDepartamento)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.ToTable("Estado", "RuletaMasivian");

                entity.HasIndex(e => e.NombreEstado)
                    .HasName("UQ__Estado__6CE506152CD84698")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreEstado)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoFacilidad>(entity =>
            {
                entity.ToTable("EstadoFacilidad", "RuletaMasivian");

                entity.HasIndex(e => e.NombreEstadoFacilidad)
                    .HasName("UQ__EstadoFa__5A024D2D2E89F4BB")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreEstadoFacilidad)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoPozo>(entity =>
            {
                entity.ToTable("EstadoPozo", "RuletaMasivian");

                entity.HasIndex(e => e.NombreEstadoPozo)
                    .HasName("UQ__EstadoPo__596F3EB0A64A319A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreEstadoPozo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadosEscenarios>(entity =>
            {
                entity.ToTable("Estados_Escenarios", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Diferencia)
                    .HasColumnName("diferencia")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.Escenario)
                    .IsRequired()
                    .HasColumnName("escenario")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCorte)
                    .HasColumnName("fecha_corte")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Falla>(entity =>
            {
                entity.ToTable("Falla", "RuletaMasivian");

                entity.HasIndex(e => e.LongName)
                    .HasName("UQ__Falla__A6D3E88003B59125")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FaultType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.LongName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);
            });

            modelBuilder.Entity<JerarquiaAdministrativa>(entity =>
            {
                entity.ToTable("JerarquiaAdministrativa", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gerencia)
                    .HasColumnName("GERENCIA")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdPrincipal).HasColumnName("ID_PRINCIPAL");

                entity.Property(e => e.Idger).HasColumnName("IDGER");

                entity.Property(e => e.Idvice).HasColumnName("IDVICE");

                entity.Property(e => e.Nombregerencia)
                    .HasColumnName("NOMBREGERENCIA")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Organizacion)
                    .HasColumnName("ORGANIZACION")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadNivel3Estado)
                    .HasColumnName("UNIDAD_NIVEL3_ESTADO")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadNivel3VigenteDesde)
                    .HasColumnName("UNIDAD_NIVEL3_VIGENTE_DESDE")
                    .HasColumnType("date");

                entity.Property(e => e.UnidadNivel3VigenteHasta)
                    .HasColumnName("UNIDAD_NIVEL3_VIGENTE_HASTA")
                    .HasColumnType("date");

                entity.Property(e => e.Vice)
                    .HasColumnName("VICE")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LeyRegalias>(entity =>
            {
                entity.ToTable("LeyRegalias", "RuletaMasivian");

                entity.HasIndex(e => e.NombreLeyRegalias)
                    .HasName("UQ__LeyRegal__BE8EC4A6F15615FA")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreLeyRegalias)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MetodoProduccion>(entity =>
            {
                entity.ToTable("MetodoProduccion", "RuletaMasivian");

                entity.HasIndex(e => e.NombreMetodoProduccion)
                    .HasName("UQ__MetodoPr__16811312321B9930")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreMetodoProduccion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mezcla>(entity =>
            {
                entity.ToTable("Mezcla", "RuletaMasivian");

                entity.HasIndex(e => e.PoolName)
                    .HasName("UQ__Mezcla__441B49B69834042B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PoolId).HasColumnName("pool_id");

                entity.Property(e => e.PoolName)
                    .HasColumnName("pool_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ModalidadCampoContrato>(entity =>
            {
                entity.ToTable("ModalidadCampoContrato", "RuletaMasivian");

                entity.HasIndex(e => e.NombreModalidadCampoContrato)
                    .HasName("UQ__Modalida__483A71D257B79DFB")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreModalidadCampoContrato)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Municipios>(entity =>
            {
                entity.ToTable("Municipios", "RuletaMasivian");

                entity.HasIndex(e => e.Municipio)
                    .HasName("UQ__Municipi__C023EB1150113352")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Departamento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DepartamentoId)
                    .HasColumnName("Departamento_Id")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Municipio)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MunicipioId)
                    .HasColumnName("Municipio_id")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Paises>(entity =>
            {
                entity.ToTable("Paises", "Internacional");

                entity.HasIndex(e => e.PreferredName)
                    .HasName("UQ__Paises__EF0FDFDA11CB6F0B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AreaId)
                    .HasColumnName("AREA_ID")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AreaType)
                    .HasColumnName("AREA_TYPE")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PreferredName)
                    .IsRequired()
                    .HasColumnName("PREFERRED_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Campo>(entity =>
            {
                entity.ToTable("Campo", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionBreve)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaDescubrimiento).HasColumnType("date");

                entity.Property(e => e.FechaEfectividadCampo).HasColumnType("date");

                entity.Property(e => e.FechaExpiracionCampo).HasColumnType("date");

                entity.Property(e => e.IdCampo)
                    .HasColumnName("idCampo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdCebe)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.idCebe_Anterior)
                    .HasColumnName("idCebe_Anterior")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdCeco)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.idCeco_Anterior)
                    .HasColumnName("idCeco_Anterior")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdMezclaCampoProductoSiv)
                    .HasColumnName("IdMezclaCampoProductoSIV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.idMezclaCampoProductoSiv_Anterior)
                    .HasColumnName("idMezclaCampoProductoSiv_Anterior")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdNombreProceso)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.idNombreProceso_Anterior)
                    .HasColumnName("idNombreProceso_Anterior")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdSuperintendenciaCoordinacion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoCampo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(true);

                entity.Property(e => e.NombreCebe)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCeco)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCompania)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreMezclaCampoProductoSiv)
                    .HasColumnName("NombreMezclaCampoProductoSIV")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreProceso)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreSuperintendenciaCoordinacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RequiereCompletamiento).IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<CampoContrato>(entity =>
            {
                entity.ToTable("CampoContrato", "RuletaMasivian");

                entity.HasIndex(e => e.NombreCampoContrato)
                    .HasName("UQ__CampoCon__B3980F5F86C03C83")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Asociados)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ClaveDeAmortizacion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionCampoContrato).IsUnicode(false);

                entity.Property(e => e.FechaDescubrimientoCampoContrato).HasColumnType("date");

                entity.Property(e => e.FechaEfectividadCampoContrato).HasColumnType("date");

                entity.Property(e => e.IdCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampoContrato)
                    .HasColumnName("idCampoContrato")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdClaveAmortizacion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdLeyRegalias).HasColumnName("idLeyRegalias");

                entity.Property(e => e.IdModalidadCampoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreAsociados).IsUnicode(false);

                entity.Property(e => e.NombreCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampoContrato)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCompania)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreLeyRegalias)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreModalidadCampoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PorcParticipacionBasica).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.PorcParticipacionIncremental).HasColumnType("numeric(18, 4)");

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<CompaniaOperadora>(entity =>
            {
                entity.ToTable("CompaniaOperadora", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveInd)
                    .HasColumnName("active_ind")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BaAbbreviation)
                    .HasColumnName("ba_abbreviation")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.BaCode)
                    .HasColumnName("ba_code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BaName)
                    .HasColumnName("ba_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BaShortName)
                    .HasColumnName("ba_short_name")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.BaType)
                    .HasColumnName("ba_type")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessAssociate)
                    .IsRequired()
                    .HasColumnName("business_associate")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompaniaOperadoraId).HasColumnName("compania_operadora_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(250);

                entity.Property(e => e.EffectiveDate)
                    .HasColumnName("effective_date")
                    .HasColumnType("date");

                entity.Property(e => e.ExisteMaestraSap)
                    .HasColumnName("existe_Maestra_SAP")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .HasColumnType("date");

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombrePais)
                    .HasColumnName("Nombre_Pais")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(250);

                entity.Property(e => e.Padre)
                    .HasColumnName("padre")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PaisId)
                    .HasColumnName("pais_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Propiedad)
                    .HasColumnName("propiedad")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPadre)
                    .HasColumnName("tipo_padre")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasColumnName("observacion")
                    .HasMaxLength(250);

                entity.Property(e => e.Categoria)
                    .HasColumnName("categoria")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria")
                    .HasMaxLength(255)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.ToTable("Contrato", "RuletaMasivian");

                entity.HasIndex(e => e.NombreContrato)
                    .HasName("UQ__Contrato__393B3697904FDF71")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompaniasAdministradorasContrato)
                    .HasColumnName("CompaniasAdministradoras_Contrato")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FinalizacionContrato).HasColumnType("date");

                entity.Property(e => e.IdNombreContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InicioContrato).HasColumnType("date");

                entity.Property(e => e.NombreCompaniaOperadora)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContrato)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreJerarquiaAdministrativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoEnlace)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoEnlace)
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.NombresCompaniasAdministradorasContrato)
                    .HasColumnName("NombresCompaniasAdministradoras_Contrato")
                    .IsUnicode(false);

                entity.Property(e => e.NumeroContrato)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                
                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Facilidad>(entity =>
            {
                entity.ToTable("Facilidad", "RuletaMasivian");

                entity.HasIndex(e => e.NombreFacilidad)
                    .HasName("UQ__Facilida__1889B410C8027122")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CapacidadDeAlmacenamiento).HasColumnType("numeric(18, 3)");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.IdFacilidad)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdAreaDministrativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdDepartamento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdEmpresaOperadora)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstadoFacilidad)
                    .HasColumnName("idEstadoFacilidad")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdMezclaProducccionSiv)
                    .HasColumnName("IdMezclaProducccionSIV")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdMunicipio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdProceso)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoFacilidad)
                    .HasColumnName("idTipoFacilidad")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreAreaAdministrativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDepartamento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEmpresaOperadora)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstadoFacilidad)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreFacilidad)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreMezclaProductoSiv)
                    .HasColumnName("NombreMezclaProductoSIV")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreMunicipio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreProceso)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoFacilidad)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsableAprobacion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdResponsableAprobacion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Tiporesponsable)
                    .HasColumnName("TIPOResponsable")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NombreAlmacenLogistico)
                    .HasColumnName("NombreAlmacenLogistico")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdAlmacenLogistico)
                    .HasColumnName("IdAlmacenLogistico")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoEstado)
                    .HasColumnName("TipoEstado")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pozo>(entity =>
            {
                entity.ToTable("Pozo", "RuletaMasivian");

                entity.HasIndex(e => e.NombrePozo)
                    .HasName("UQ__Pozo__B567C92B26691B1B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoordenadaPlanaSuperficieEx)
                    .HasColumnName("CoordenadaPlanaSuperficieEX")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CoordenadaPlanaSuperficieNy)
                    .HasColumnName("CoordenadaPlanaSuperficieNY")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCompletamiento).HasColumnType("date");

                entity.Property(e => e.FechaInicioOperacion).HasColumnType("date");

                entity.Property(e => e.FechaPerforacion).HasColumnType("date");

                entity.Property(e => e.IdArea)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdClasificacionLahee)
                    .HasColumnName("IdClasificacionLAHEE")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCompaniaOperadora)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdDatum)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdDepartamento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstadoPozo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdJerarquiaAdministrativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdMetodoProduccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdMunicipio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdPozo)
                    .HasColumnName("idPozo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdPuntoOrigen)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoPozo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdentificadorPozo)
                    .HasMaxLength(38)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("numeric(22, 9)");

                entity.Property(e => e.Longitude).HasColumnType("numeric(22, 9)");

                entity.Property(e => e.RequiereCompletamiento).IsUnicode(false);

                entity.Property(e => e.NombreArea)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreClasificacionLahee)
                    .HasColumnName("NombreClasificacionLAHEE")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCompaniaOperadora)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDatum)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDepartamento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstadoPozo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreJerarquiaAdministrativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreMetodoProduccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreMunicipio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombrePozo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.NombrePuntoOrigen)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoPozo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProfundidadPies)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<PozoYacimiento>(entity =>
            {
                entity.ToTable("PozoYacimiento", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdPozoYacimiento)
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCompletamiento).HasColumnType("date");

                entity.Property(e => e.FechaInicioMetodoOperacion).HasColumnType("date");

                entity.Property(e => e.FechaPerforacion).HasColumnType("date");

                entity.Property(e => e.IdArea)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdCampoContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstadoPozoYacimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdFacilidadBombeo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdFacilidadRecoleccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdFacilidadTratamiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdMetodoProduccion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPozo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdProductoPrimario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoPozoYacimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdYacimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreArea)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCampoContrato)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstadoPozoYacimiento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreFacilidadBombeo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreFacilidadRecoleccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreFacilidadTratamiento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreMetodoProduccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombrePozo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombrePozoYacimiento)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreProductoPrimario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoPozoYacimiento)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreYacimiento)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sarta)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StratUnitId)
                    .HasColumnName("strat_unit_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StratNameSetId)
                    .HasColumnName("strat_name_set_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<SistemaTransporte>(entity =>
            {
                entity.ToTable("SistemaTransporte", "RuletaMasivian");

                entity.HasIndex(e => e.NombreLineaDeTransporte)
                    .HasName("UQ__SistemaT__F8A1839939AA52B5")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CapacidadBombeo).HasColumnType("numeric(14, 4)");

                entity.Property(e => e.CapacidadContratada).HasColumnType("numeric(14, 4)");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(240)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionSistemaTransporte).IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("date");

                entity.Property(e => e.IdClaseSistema)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.tipoClaseSistema)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstacionFinal)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstacionInicial)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdJerarquiaAdministrativa)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdLineaDeTransporte)
                    .IsRequired()
                    .HasColumnName("idLineaDeTransporte")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdSistemaTransporte)
                    .HasColumnName("idSistemaTransporte")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdNombreSistemaTransporteAsociado)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdPuntoDeEntrega)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LlenoDeLinea).HasColumnType("numeric(14, 4)");

                entity.Property(e => e.LongitudDeLinea).HasColumnType("numeric(14, 4)");

                entity.Property(e => e.NombreClaseSistema)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstacionFinal)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstacionInicial)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreJerarquiaAdministrativa)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreLineaDeTransporte)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.NombrePuntoDeEntrega)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NombreSistemaTransporteAsociado)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.TiempoFueraServicio).HasColumnType("numeric(14, 4)");

                entity.Property(e => e.TiempoRealServicio).HasColumnType("numeric(14, 4)");

                entity.Property(e => e.TipoFacilidad)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoFacilidadEstacionInicial)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoFacilidadEstacionFinal)
                    .HasMaxLength(20)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Yacimiento>(entity =>
            {
                entity.ToTable("Yacimiento", "RuletaMasivian");

                entity.HasIndex(e => e.NombreYacimiento)
                    .HasName("UQ__Yacimien__E4EEB52A3BFEBA48")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdYacimiento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AbreviacionYacimiento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionYacimiento)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdCuenca)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCuenca)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreTipoFalla)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NombreYacimiento)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedBy)
                    .HasColumnName("row_changed_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowChangedDate)
                    .HasColumnName("row_changed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RowCreatedBy)
                    .HasColumnName("row_created_by")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDate)
                    .HasColumnName("row_created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.TipoFalla)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Proceso>(entity =>
            {
                entity.ToTable("Proceso", "RuletaMasivian");

                entity.HasIndex(e => e.NombreProceso)
                    .HasName("UQ__Proceso__018CFE9ED40B71E7")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreProceso)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductoPrimario>(entity =>
            {
                entity.ToTable("ProductoPrimario", "RuletaMasivian");

                entity.HasIndex(e => e.NombreProductoPrimario)
                    .HasName("UQ__Producto__9A2FEB2602A64EDE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreProductoPrimario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PuntoOrigen>(entity =>
            {
                entity.ToTable("PuntoOrigen", "RuletaMasivian");

                entity.HasIndex(e => e.NombrePuntoOrigen)
                    .HasName("UQ__PuntoOri__CD92005D96C5F57E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombrePuntoOrigen)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoFacilidad>(entity =>
            {
                entity.ToTable("TipoFacilidad", "RuletaMasivian");

                entity.HasIndex(e => e.NombreTipoFacilidad)
                    .HasName("UQ__TipoFaci__446699EC53426C8E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreTipoFacilidad)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoPozo>(entity =>
            {
                entity.ToTable("TipoPozo", "RuletaMasivian");

                entity.HasIndex(e => e.NombreTipoPozo)
                    .HasName("UQ__TipoPozo__3F1BEED8F5B6FB83")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreTipoPozo)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoPozoYacimiento>(entity =>
            {
                entity.ToTable("TipoPozoYacimiento", "RuletaMasivian");

                entity.HasIndex(e => e.NombreTipoPozoYacimiento)
                    .HasName("UQ__TipoPozo__366C39AFB95ADC17")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreTipoPozoYacimiento)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoProduccionPozoYacimiento>(entity =>
            {
                entity.ToTable("TipoProduccionPozoYacimiento", "RuletaMasivian");

                entity.HasIndex(e => e.NombreTipoProduccionPozoYacimiento)
                    .HasName("UQ__TipoProd__A4FB5D03331DE23A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreTipoProduccionPozoYacimiento)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposCampo>(entity =>
            {
                entity.ToTable("TiposCampo", "RuletaMasivian");

                entity.HasIndex(e => e.FieldType)
                    .HasName("UQ__TiposCam__375CEEF9D232ABA0")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasColumnName("ABBREVIATION")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FieldType)
                    .IsRequired()
                    .HasColumnName("FIELD_TYPE")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.LongName)
                    .IsRequired()
                    .HasColumnName("LONG_NAME")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasColumnName("SHORT_NAME")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposContrato>(entity =>
            {
                entity.ToTable("TiposContrato", "RuletaMasivian");

                entity.HasIndex(e => e.LandUnitType)
                    .HasName("UQ__TiposCon__AA2DE8B0707859C3")
                    .IsUnique();

                entity.HasIndex(e => e.ShortName)
                    .HasName("UQ__TiposCon__F4E7E33E21F31D01")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LandUnitType)
                    .HasColumnName("LAND_UNIT_TYPE")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.LongName)
                    .HasColumnName("LONG_NAME")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .HasColumnName("SHORT_NAME")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasColumnName("SOURCE")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Parametros>(entity =>
            {
                entity.HasKey(e => e.IdParametro)
                    .HasName("PK__Parametr__9C816E5F6ABB0207");

                entity.ToTable("Parametros", "RuletaMasivian");

                entity.HasIndex(e => e.NombreParametro)
                    .HasName("UQ__Parametr__45E3A89BD4588F3E")
                    .IsUnique();

                entity.Property(e => e.IdParametro).HasColumnName("idParametro");

                entity.Property(e => e.NombreParametro)
                    .IsRequired()
                    .HasColumnName("nombreParametro")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParametroBit).HasColumnName("parametroBit");

                entity.Property(e => e.ParametroDate)
                    .HasColumnName("parametroDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ParametroInt).HasColumnName("parametroInt");

                entity.Property(e => e.ParametroVarchar)
                    .HasColumnName("parametroVarchar")
                    .HasMaxLength(150);

                entity.Property(e => e.TipoParametro)
                    .HasColumnName("tipoParametro")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sarta>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_SARTA_Id");

                entity.ToTable("Sarta", "RuletaMasivian");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.IdSarta)
                    .HasColumnName("IdSarta")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Field_Id)
                    .HasColumnName("Field_Id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Field_Name)
                    .HasColumnName("Field_Name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Uwi)
                    .HasColumnName("Uwi")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Uwi_Name)
                    .HasColumnName("Uwi_Name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Strat_Name_Set_Id)
                    .HasColumnName("Strat_Name_Set_Id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Strat_Unit_Id)
                    .HasColumnName("Strat_Unit_Id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Strat_Name_Set)
                    .HasColumnName("Strat_Name_Set")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Prod_String_Type)
                    .HasColumnName("Prod_String_Type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Prod_String_Type_Name)
                    .HasColumnName("Prod_String_Type_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.String_Id)
                    .HasColumnName("String_Id")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Alias_Id)
                    .HasColumnName("Alias_Id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Alias_Full_Name)
                    .HasColumnName("Alias_Full_Name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasColumnName("Source")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Effective_Date)
                    .HasColumnName("Effective_Date")
                    .HasColumnType("date");

                entity.Property(e => e.Expiry_Date)
                    .HasColumnName("Expiry_Date")
                    .HasColumnType("date");

                entity.Property(e => e.Total_Depht)
                    .HasColumnName("Total_Depht")
                    .HasColumnType("numeric(10, 5)");

                entity.Property(e => e.Business_Associate)
                    .HasColumnName("Business_Associate")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Business_Associate_Name)
                    .HasColumnName("Business_Associate_Name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Active_Ind)
                    .HasColumnName("Active_Ind")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.NombreEstado)
                    .HasColumnName("NombreEstado")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasColumnName("Descripcion")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Row_Created_By)
                    .HasColumnName("Row_Created_By")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Row_Changed_By)
                    .HasColumnName("Row_Changed_By")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasColumnName("Observacion")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasColumnName("CorreoElectronico")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Row_Created_Date)
                    .HasColumnName("Row_Created_Date")
                    .HasColumnType("date");

                entity.Property(e => e.Row_Changed_Date)
                    .HasColumnName("Row_Changed_Date")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status_Type)
                      .HasColumnName("StatusType")
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.Property(e => e.Status_Name)
                      .HasColumnName("Status_Name")
                      .HasMaxLength(60)
                      .IsUnicode(false);

            });
        }
    }
}

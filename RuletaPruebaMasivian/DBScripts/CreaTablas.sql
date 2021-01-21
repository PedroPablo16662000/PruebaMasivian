CREATE TABLE Ruletas(
idRuleta int primary key identity(1,1),
fechaInicial DateTime,
fechaFinal DateTime,
marca varchar(150),
observacion varchar(max),
dineroPorGanar integer,
estadoActual bit
);
CREATE TABLE Usuarios(
idUsuario varchar(50) primary key,
apodoUsuario varchar(100) not null unique,
dineroAFavor int
);
Insert into Usuarios output inserted.idUsuario values('RWDWEF124','JOKERMEN2',5000),('LACASA','CASINO',150000);
CREATE TABLE Apuestas(
idApuesta int primary key identity(1,1),
numero int,
color varchar(5),
valorApostado int not null,
idRuleta int foreign key references Ruletas(idRuleta),
idUsuario varchar(50) not null foreign key references Usuarios(idUsuario),
idUsuarioGanador varchar(50),
fechaApuesta DateTime
);

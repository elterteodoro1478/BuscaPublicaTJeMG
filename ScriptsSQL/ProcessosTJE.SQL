GO
use master
GO
 create database ProcessosTJE

GO
USE [ProcessosTJE]
GO
GO
/****** Object:  Table [dbo].[Movimentacao]    Script Date: 21/11/2022 20:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Movimentacao]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Movimentacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProcesso] [int] NULL,
	[DataMovimentacao] [datetime] NULL,
	[Descricao] [varchar](255) NULL,
 CONSTRAINT [PK_Movimentacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Participante]    Script Date: 21/11/2022 20:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Participante]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Participante](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProcesso] [int] NULL,
	[Nome] [varchar](255) NULL,
	[Documento] [varchar](50) NULL,
	[Polo] [varchar](50) NULL,
	[TipoDocumento] [varchar](50) NULL,
 CONSTRAINT [PK_Participante] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Processo]    Script Date: 21/11/2022 20:32:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Processo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Processo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumeroProcesso] [varchar](50) NOT NULL,
	[DataDistribuicao] [datetime] NOT NULL,
	[ClasseJuridicial] [varchar](150) NULL,
	[Assunto] [varchar](255) NOT NULL,
	[Jurisdicao] [varchar](50) NULL,
	[OrgaoJulgador] [varchar](255) NULL,
	[NumeroProcessoReferencia] [varchar](50) NULL,
 CONSTRAINT [PK_Processo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movimentacao_Processo]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movimentacao]'))
ALTER TABLE [dbo].[Movimentacao]  WITH CHECK ADD  CONSTRAINT [FK_Movimentacao_Processo] FOREIGN KEY([IdProcesso])
REFERENCES [dbo].[Processo] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movimentacao_Processo]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movimentacao]'))
ALTER TABLE [dbo].[Movimentacao] CHECK CONSTRAINT [FK_Movimentacao_Processo]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Participante_Processo]') AND parent_object_id = OBJECT_ID(N'[dbo].[Participante]'))
ALTER TABLE [dbo].[Participante]  WITH NOCHECK ADD  CONSTRAINT [FK_Participante_Processo] FOREIGN KEY([IdProcesso])
REFERENCES [dbo].[Processo] ([Id])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Participante_Processo]') AND parent_object_id = OBJECT_ID(N'[dbo].[Participante]'))
ALTER TABLE [dbo].[Participante] CHECK CONSTRAINT [FK_Participante_Processo]
GO

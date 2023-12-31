USE [master]
GO
/****** Object:  Database [SimpleWalletSystem]    Script Date: 8/29/2023 8:22:17 PM ******/
CREATE DATABASE [SimpleWalletSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SimpleWalletSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER22\MSSQL\DATA\SimpleWalletSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SimpleWalletSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER22\MSSQL\DATA\SimpleWalletSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SimpleWalletSystem] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SimpleWalletSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SimpleWalletSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SimpleWalletSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SimpleWalletSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SimpleWalletSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SimpleWalletSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [SimpleWalletSystem] SET  MULTI_USER 
GO
ALTER DATABASE [SimpleWalletSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SimpleWalletSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SimpleWalletSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SimpleWalletSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SimpleWalletSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SimpleWalletSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SimpleWalletSystem', N'ON'
GO
ALTER DATABASE [SimpleWalletSystem] SET QUERY_STORE = ON
GO
ALTER DATABASE [SimpleWalletSystem] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SimpleWalletSystem]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[WalletId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Amount] [money] NULL,
	[TransactionDatetime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transfers]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transfers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SenderWalletId] [int] NOT NULL,
	[ReceiversWalletId] [int] NOT NULL,
	[Amount] [money] NULL,
	[TransferDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](30) NOT NULL,
	[Password] [nvarchar](30) NOT NULL,
	[Fullname] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UsersId] [int] NOT NULL,
	[AccountNumber] [bigint] NOT NULL,
	[Balance] [money] NULL,
	[DateCreated] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [TransactionDatetime]
GO
ALTER TABLE [dbo].[Transfers] ADD  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Transfers] ADD  DEFAULT (getdate()) FOR [TransferDateTime]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Wallet] ADD  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[Wallet] ADD  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_WalletId] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_WalletId]
GO
ALTER TABLE [dbo].[Transfers]  WITH CHECK ADD  CONSTRAINT [FK_Transfers_ReceiversWalletId] FOREIGN KEY([ReceiversWalletId])
REFERENCES [dbo].[Wallet] ([id])
GO
ALTER TABLE [dbo].[Transfers] CHECK CONSTRAINT [FK_Transfers_ReceiversWalletId]
GO
ALTER TABLE [dbo].[Transfers]  WITH CHECK ADD  CONSTRAINT [FK_Transfers_SenderWalletId] FOREIGN KEY([SenderWalletId])
REFERENCES [dbo].[Wallet] ([id])
GO
ALTER TABLE [dbo].[Transfers] CHECK CONSTRAINT [FK_Transfers_SenderWalletId]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_UsersId]
GO
/****** Object:  StoredProcedure [dbo].[SPR_CheckAccntDetails]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPR_CheckAccntDetails]
@Userid int
AS
BEGIN

	SELECT 
		u.Fullname,
		w.AccountNumber,
		CAST(w.Balance AS VARCHAR(20)) AS Balance,
		u.DateCreated

	FROM dbo.Users AS u
	INNER JOIN dbo.Wallet AS w
	ON
	u.id = w.UsersId
	WHERE
	u.id = @Userid;

END
GO
/****** Object:  StoredProcedure [dbo].[SPR_CheckExistingUser]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPR_CheckExistingUser]
@Username Nvarchar(30)
AS
BEGIN
	Select 
	* 
	from dbo.Users
	Where 
	Username =@Username;
END
GO
/****** Object:  StoredProcedure [dbo].[SPR_RegisterNewUser]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPR_RegisterNewUser]
@Fullname NVARCHAR(50),
@Username NVARCHAR(20),
@Password NVARCHAR(30),
@AccountNumber BIGINT
AS
BEGIN
	
	DECLARE @UserId INT; 

	BEGIN TRY
		BEGIN TRANSACTION;

		INSERT INTO dbo.Users (Fullname, Username, Password)
						VALUES (@Fullname, @Username, @Password);

		SET @UserId = SCOPE_IDENTITY();

		INSERT INTO dbo.Wallet (UsersId, AccountNumber)
						VALUES (@UserId, @AccountNumber);

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH;
	
END

GO
/****** Object:  StoredProcedure [dbo].[SPR_Transaction]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPR_Transaction]
@Userid int,
@Amount Bigint,
@Type int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;

			UPDATE dbo.Wallet 
		SET 
			Balance = CASE
						WHEN @Type = 0 THEN Balance + @Amount
						WHEN @Type = 1 THEN Balance - @Amount
					  END
		WHERE
			UsersId = @Userid;

		INSERT INTO dbo.Transactions(WalletId,Type,Amount)
		SELECT 
			ID,
			'0',
			@Amount
		FROM dbo.Wallet
		WHERE UsersId = @Userid;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[SPR_TransactionHistory]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPR_TransactionHistory]
@UserId Int
AS
BEGIN
	
	SELECt	TOP 20  
		CASE	
			WHEN type = 0 THEN 'Deposit'
			WHEN type = 1 THEN 'Widthraw'
			END AS 'Type',
		CAST(Amount AS VARCHAR(20)) AS 'Amount',
		TransactionDatetime

	FROM dbo.Transactions
	Where
	WalletId IN(SELECT ID FROM dbo.Wallet where UsersId=@UserId)
	
	ORDER BY TransactionDatetime DESC
	;


END
GO
/****** Object:  StoredProcedure [dbo].[SPR_Transferhistory]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPR_Transferhistory]
@UserId Int
AS
BEGIN
	
	SELECt	TOP 20  
		sender.Fullname AS 'SendersName',
		receiver.Fullname AS 'ReceiversName',
		CAST(t.Amount AS VARCHAR(20)) AS 'Amount',
		t.TransferDateTime

	FROM dbo.Transfers AS t
	LEFT JOIN dbo.Users AS sender
	ON
	t.SenderWalletId = sender.id
	LEFT JOIN dbo.Users AS receiver
	ON
	t.ReceiversWalletId = receiver.id
	WHERE 
	sender.id = @UserId;

END
GO
/****** Object:  StoredProcedure [dbo].[SPR_TransferMoney]    Script Date: 8/29/2023 8:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SPR_TransferMoney]
    @SENDERSWALLETID INT,
    @RECEIVERSSWALLETID INT,
    @AMOUNT BIGINT
AS
BEGIN

	BEGIN TRY
		BEGIN TRANSACTION;

			-- Update Two wallet
			UPDATE dbo.Wallet
			SET Balance = CASE
							  WHEN UsersId = @SENDERSWALLETID THEN Balance - @AMOUNT
							  WHEN UsersId = @RECEIVERSSWALLETID THEN Balance + @AMOUNT
							  ELSE Balance
						  END
			WHERE UsersId IN (@SENDERSWALLETID, @RECEIVERSSWALLETID);

			-- If the sender's balance goes negative then rollback the transaction
			IF (SELECT Balance FROM dbo.Wallet WHERE UsersId = @SENDERSWALLETID) < 0
			BEGIN
				ROLLBACK;
				RETURN;
			END

			-- Insert the transfer record
			INSERT INTO dbo.Transfers (SenderWalletId, ReceiversWalletId, Amount)
			VALUES (@SENDERSWALLETID, @RECEIVERSSWALLETID, @AMOUNT);

			COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH;

END;
GO
USE [master]
GO
ALTER DATABASE [SimpleWalletSystem] SET  READ_WRITE 
GO

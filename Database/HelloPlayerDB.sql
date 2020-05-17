/* check whether db already exists */
if exists(select 1 from master.dbo.sysdatabases
		  where name = 'HelloPlayerDB')
begin
	drop database [HelloPlayerDB]
	print '' print '*** Dropping Database HelloPlayerDB ***'
end
go

print '' print '*** Creating Database HelloPlayerDB ***'
go

create database [HelloPlayerDB]
go

print '' print '*** Using Database HelloPlayerDB ***'
go

use [HelloPlayerDB]
go

print '' print '*** Creating Employee Table'
go

create table [dbo].[Employee](
	[EmployeeID]  [int] identity(1000000,1) not null,
	[FirstName]   [nvarchar](50)            not null,
	[LastName]    [nvarchar](50)            not null,
	[UserName]    [nvarchar](100)			not null,
	[PhoneNumber] [nvarchar](11) 			not null,
	[Email]       [nvarchar](250)			not null,
	[PasswordHash][nvarchar](100)			not null
		default '9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	[Active]      [bit]         			not null default 1,
	constraint [pk_EmployeeID] primary key([EmployeeID] asc),
	constraint [ak_Employee_Email] unique([Email] asc),
	constraint [ak_Employee_UserName] unique ([UserName] asc)
)
go

print '' print '*** Creating Sample Employee Records'
go

insert into [dbo].[Employee]
	([FirstName], [LastName], [UserName], [PhoneNumber], [Email])
	values
	('System', 'Admin', 'admin', '00000000000', 'admin@helloplayer.com'),
	('Josh', 'Jackson', 'jjackson', '13192522443', 'jjackson@helloplayer.com'),
	('Toney', 'Balogna', 'stoneycat', '13192342443', 'toneythecat@helloplayer.com')
go

print '' print '*** Creating Sample Deactivated Employee'
go

insert into [dbo].[Employee]
	([FirstName], [LastName], [UserName], [PhoneNumber], [Email], [Active])
	values
	('Brandon', 'Andeway', 'dickhead', '13194568896', 'doofus@helloplayer.com', 0)

print '' print '*** Creating sp_insert_employee'
GO
CREATE PROCEDURE [sp_insert_employee]
(
	@FirstName		[nvarchar](50),
	@LastName		[nvarchar](50),
	@UserName       [nvarchar](100),
	@PhoneNumber 	[nvarchar](11),
	@Email 			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Employee]
		([FirstName], [LastName], [UserName], [PhoneNumber], [Email])
	VALUES
		(@FirstName, @LastName, @UserName, @PhoneNumber, @Email)
	SELECT SCOPE_IDENTITY()
END
GO

print '' print '*** Creating sp_update_employee'
go
create procedure [sp_update_employee]
(
	@EmployeeID			[int],
	@OldFirstName   	[nvarchar](50),
	@OldLastName    	[nvarchar](50),
	@OldUserName        [nvarchar](100),
	@OldPhoneNumber 	[nvarchar](11),
	@OldEmail           [nvarchar](250),
	@NewFirstName   	[nvarchar](50),
	@NewLastName    	[nvarchar](50),
	@NewUserName        [nvarchar](100),
	@NewPhoneNumber 	[nvarchar](11),
	@NewEmail           [nvarchar](250)
)
as
begin
update [dbo].[Employee]
set
	[FirstName] = 	@NewFirstName,
	[LastName] = 	@NewLastName,
	[UserName] = @NewUserName,
	[PhoneNumber] = @NewPhoneNumber,
	[Email] = 		@NewEmail
where [EmployeeID] = @EmployeeID
	  AND	[FirstName] = 	@OldFirstName
	  AND	[LastName] = 	@OldLastName
	  AND   [UserName] =    @OldUserName
	  AND	[PhoneNumber] = @OldPhoneNumber
	  AND	[Email] = 		@OldEmail
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_deactivate_employee'
go
create procedure [sp_deactivate_employee]
(
	@EmployeeID		[int]
)
as
begin
update [dbo].[Employee]
set
	[Active] = 0
where [EmployeeID] = @EmployeeID
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_reactivate_employee'
go
create procedure [sp_reactivate_employee]
(
	@EmployeeID		[int]
)
as
begin
update [dbo].[Employee]
set
	[Active] = 1
where [EmployeeID] = @EmployeeID
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_delete_employee'
go
create procedure [sp_delete_employee]
(
	@EmployeeID		[int]
)
as
begin
delete
from [Employee]
where [EmployeeID] = @EmployeeID
end
go

print '' print '*** Creating PlayerAccountStatus Table'
go

create table [dbo].[PlayerAccountStatus](
	[StatusID]   	  [nvarchar](50)            not null,
	[Description] [nvarchar](250)           null,
	constraint [pk_StatusID] primary key([StatusID] asc)
)
go

print '' print '*** Creating Sample PlayerAccountStatus Records'
go

insert into [dbo].[PlayerAccountStatus]
	([StatusID])
	values
	('Free Account'),
	('Premium Account')
go

print '' print '*** Creating sp_select_all_player_statuses'
go
create procedure [sp_select_all_player_statuses]
as
begin

select
	[StatusID]
from [PlayerAccountStatus]
end
go

print '' print '*** Creating Player Table'
go

create table [dbo].[Player](
	[PlayerID]  [int] identity(1000000,1) 	not null,
	[FirstName]   [nvarchar](50)           		not null,
	[LastName]    [nvarchar](50)            	not null,
	[Username]	  [nvarchar](20)				not null,
	[PhoneNumber] [nvarchar](11) 		    null,
	[Email]       [nvarchar](250)			not null,
	[Experience]  [int]                     not null default 0,
	[PlayerLevel] [int]						not null default 1,
	[Active]      [bit]         			not null default 1,
	[PlayerStatus][nvarchar](50)			not null default "Free Account",
	[PasswordHash][nvarchar](100)			not null
		default '9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	constraint [pk_PlayerID] primary key([PlayerID] asc),
	constraint [ak_Player_Email] unique([Email] asc),
	constraint [ak_Player_UserName] unique([Username] asc),
	constraint [fk_PlayerStatus] foreign key([PlayerStatus])
		references [PlayerAccountStatus]([StatusID]) on update cascade	
)
go



print '' print '*** Creating Sample Player Records'
go

insert into [dbo].[Player]
	([FirstName], [LastName], [Username], [PhoneNumber], [Email])
	values
	('Frank', 'Baxter', 'BigBoomer69', '13192254343', 'frankiefresh25@hotmail.com')
go

print '' print '*** Creating sp_select_all_players'
go
create procedure [sp_select_all_players]
as
begin
    select [PlayerID],[FirstName],[LastName],[UserName],[PhoneNumber],[Email],[Experience], [PlayerLevel], [PlayerStatus]
    from [Player]
    order by [PlayerID]
end
go

print '' print '*** Creating sp_get_pasword_hash_by_player_id'
go
create procedure [sp_get_pasword_hash_by_player_id]
(
	@PlayerID     [int]
)
as
begin
    select [PasswordHash]
    from [Player]
    where [PlayerID] = @PlayerID
end
go

print '' print '*** Creating sp_insert_player'
GO
CREATE PROCEDURE [sp_insert_player]
(
	@FirstName		[nvarchar](50),
	@LastName		[nvarchar](50),
	@UserName       [nvarchar](100),
	@Email 			[nvarchar](250)
)
AS
BEGIN
	INSERT INTO [dbo].[Player]
		([FirstName], [LastName], [UserName], [Email])
	VALUES
		(@FirstName, @LastName, @UserName, @Email)
	SELECT SCOPE_IDENTITY()
END
GO

print '' print '*** Creating sp_update_player'
go
create procedure [sp_update_player]
(
	@PlayerID			[int],
	@OldFirstName   	[nvarchar](50),
	@OldLastName    	[nvarchar](50),
	@OldUserName        [nvarchar](100),
	@OldPhoneNumber 	[nvarchar](11),
	@OldEmail           [nvarchar](250),
	@NewFirstName   	[nvarchar](50),
	@NewLastName    	[nvarchar](50),
	@NewUserName        [nvarchar](100),
	@NewPhoneNumber 	[nvarchar](11),
	@NewEmail           [nvarchar](250)
)
as
begin
update [dbo].[Player]
set
	[FirstName] = 	@NewFirstName,
	[LastName] = 	@NewLastName,
	[UserName] = @NewUserName,
	[PhoneNumber] = @NewPhoneNumber,
	[Email] = 		@NewEmail
where [PlayerID] = @PlayerID
	  AND	[FirstName] = 	@OldFirstName
	  AND	[LastName] = 	@OldLastName
	  AND   [UserName] =    @OldUserName
	  AND	[PhoneNumber] = @OldPhoneNumber
	  AND	[Email] = 		@OldEmail
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_upgrade_to_premium'
go
create procedure [sp_upgrade_to_premium]
(
	@PlayerID			[int],
	@NewPlayerStatus   	[nvarchar](50),
	@OldPlayerStatus   	[nvarchar](50)
)
as
begin
update [dbo].[Player]
set
	[PlayerStatus] = 	@NewPlayerStatus
where [PlayerID] = @PlayerID
	  AND	[PlayerStatus] = 	@OldPlayerStatus
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_deactivate_player'
go
create procedure [sp_deactivate_player]
(
	@PlayerID		[int]
)
as
begin
update [dbo].[Player]
set
	[Active] = 0
where [PlayerID] = @PlayerID
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_reactivate_player'
go
create procedure [sp_reactivate_player]
(
	@PlayerID		[int]
)
as
begin
update [dbo].[Player]
set
	[Active] = 1
where [PlayerID] = @PlayerID
return @@ROWCOUNT
end
go

print '' print '*** Creating Role Table'
go

create table [dbo].[Role](
	[RoleID]   	  [nvarchar](50)            not null,
	[Description] [nvarchar](250)           null,
	constraint [pk_RoleID] primary key([RoleID] asc)
)
go

print '' print '*** Creating Sample Role Records'
go

insert into [dbo].[Role]
	([RoleID])
	values
	('Administrator'),
	('Quest Master'),
	('Skill Manager'),
	('Payout Manager'),
	('Business Coordinator'),
	('Player Support')
go

print '' print '*** Creating sp_select_all_employee_roles'
go
create procedure [sp_select_all_employee_roles]
as
begin

select
	[RoleID]
from [Role]
end
go

print '' print '*** Creating EmployeeRole Table'
go

create table [dbo].[EmployeeRole](
	[EmployeeID]  [int]            not null,
	[RoleID] 	  [nvarchar](50)   not null,
	constraint [pk_EmployeeID_RoleID] primary key([EmployeeID] asc, [RoleID] asc),
	constraint [fk_Employee_EmployeeID] foreign key([EmployeeID])
		references [Employee]([EmployeeID])on delete cascade,
    constraint [fk_Role_RoleID] foreign key([RoleID])
		references [Role]([RoleID]) on update cascade			
)
go

print '' print '*** Creating Sample EmployeeRole Records'
go

insert into [dbo].[EmployeeRole]
	([EmployeeID], [RoleID])
	values
	(1000000, 'Administrator'),
	(1000001, 'Quest Master'),
	(1000002, 'Skill Manager')	
go

print '' print '*** Creating sp_create_employee_role'
go
create procedure [sp_create_employee_role]
(
	@EmployeeID		  [int],
	@RoleID      	  [nvarchar](50)
)
as
begin
insert into [dbo].[EmployeeRole]
	([EmployeeID], [RoleID])
	values
	(@EmployeeID, @RoleID)
select
	[EmployeeID] = @EmployeeID,
	[RoleID] = @RoleID
from [EmployeeRole]
where [EmployeeID] = @EmployeeID
end
go

print '' print '*** Creating sp_delete_employee_role'
go
create procedure [sp_delete_employee_role]
(
	@EmployeeID			[int],
	@RoleID				[nvarchar](50)
)
as
begin
delete
from [EmployeeRole]
where [EmployeeID] = @EmployeeID
	and [RoleID] = @RoleID
	and [RoleID] = @RoleID
end
go

print '' print '*** Creating Category Table'
go

create table [dbo].[Category](
	[Category]             [nvarchar](150)   not null,
	[CategoryDescription]  [nvarchar](4000)  null
	constraint [pk_Category] primary key([Category] asc)
)
go

print '' print '*** Creating Sample Category Records'
go

insert into [dbo].[Category]
	([Category], [CategoryDescription])
	values
	('Athletics', 'A quest in this category requires athletic ability'),
	('Cooking', 'A quest in this category requires cooking ability'),
	('Reading', 'A quest in this category requires reading ability')	
go

print '' print '*** Creating Quest Table'
go

create table [dbo].[Quest](
	[QuestID]  		   [int] identity(1000000,1) 	not null,
	[QuestName]  	   [nvarchar](150)           	not null,
	[QuestDescription] [nvarchar](2000)            	not null,
	[WorthExp]		   [int]                        not null,
	[Active]      	   [bit]         			    not null default 1,
	constraint [pk_QuestID] primary key([QuestID] asc),
	constraint [ak_QuestName] unique([QuestName] asc)
)
go

print '' print '*** Creating PlayerQuest Table'
go

create table [dbo].[PlayerQuest](
	[QuestID]  		   [int]  	not null,
	[PlayerID]  	   [int]    not null,
	[Completed]        [bit]    not null default 0,
	constraint [pk_QuestID_PlayerID] primary key([QuestID] asc, [PlayerID] asc),
	constraint [fk_Quest_QuestID] foreign key([QuestID])
		references [Quest]([QuestID])on delete cascade,
    constraint [fk_Player_PlayerID] foreign key([PlayerID])
		references [Player]([PlayerID]) on update cascade
)
go

print '' print '*** Creating sp_accept_quest'
go
create procedure [sp_accept_quest]
(
	@QuestID		  [int],
	@PlayerID      	  [int]
)
as
begin
insert into [dbo].[PlayerQuest]
	([QuestID], [PlayerID])
	values
	(@QuestID, @PlayerID)
select
	[QuestID] = @QuestID,
	[PlayerID] = @PlayerID
from [PlayerQuest]
where [PlayerID] = @PlayerID
end
go

print '' print '*** Creating sp_insert_quest'
GO
CREATE PROCEDURE [sp_insert_quest]
(
	@QuestName		  [nvarchar](150),
	@QuestDescription [nvarchar](2000),
	@WorthExp         [int]
)
AS 
BEGIN
	INSERT INTO [dbo].[Quest]
		([QuestName], [QuestDescription], [WorthExp])
	VALUES
		(@QuestName, @QuestDescription, @WorthExp)
	SELECT SCOPE_IDENTITY()
END
GO

print '' print '*** Creating sp_update_quest'
go
create procedure [sp_update_quest]
(
	@QuestID			 [int],
	@NewQuestName   	 [nvarchar](150),
	@NewQuestDescription [nvarchar](2000),
	@NewWorthExp         [int],
	@OldQuestName        [nvarchar](150),
	@OldQuestDescription [nvarchar](2000),
	@OldWorthExp    	 [int]
)
as
begin
update [dbo].[Quest]
set
	[QuestName] = 	@NewQuestName,
	[QuestDescription] = 	@NewQuestDescription,
	[WorthExp] = @NewWorthExp
where [QuestID] = @QuestID
	  AND	[QuestName] = 	@OldQuestName
	  AND	[QuestDescription] = 	@OldQuestDescription
	  AND   [WorthExp] =    @OldWorthExp
return @@ROWCOUNT
end
go

print '' print '*** Creating sp_deactivate_quest'
go
create procedure [sp_deactivate_quest]
(
	@QuestID		[int]
)
as
begin
update [dbo].[Quest]
set
	[Active] = 0
where [QuestID] = @QuestID
return @@ROWCOUNT
end
go

print '' print '*** Creating Sample Quest Records'
go

insert into [dbo].[Quest]
	([QuestName], [QuestDescription], [WorthExp])
	values
	('Book Novice', 'Read 5 books', 25)	
go


print '' print '*** Creating sp_authenticate_user'
go
create procedure [sp_authenticate_user]
(
	@Email 			[nvarchar](250),
	@PasswordHash	[nvarchar](100)
)
as
begin
	select count([EmployeeID])
	from  [dbo].[Employee]
	where [Email] = @Email
	  and [PasswordHash] = @PasswordHash
	  and [Active] = 1
end
go

print '' print '*** Creating sp_authenticate_player'
go
create procedure [sp_authenticate_player]
(
	@Email 			[nvarchar](250),
	@PasswordHash	[nvarchar](100)
)
as
begin
	select count([PlayerID])
	from  [dbo].[Player]
	where [Email] = @Email
	  and [PasswordHash] = @PasswordHash
	  and [Active] = 1
end
go


print '' print '*** Creating sp_update_email'
go
create procedure [sp_update_email]
(
	@OldEmail 			[nvarchar](250),
	@NewEmail			[nvarchar](250),
	@PasswordHash		[nvarchar](100)
)
as
begin
	update  [dbo].[Employee]
	set 	[Email] = @NewEmail
	where 	[Email] = @OldEmail
	  and 	[PasswordHash] = @PasswordHash
	  and 	[Active] = 1
	return @@ROWCOUNT
end
go

print '' print '*** Creating sp_select_employee_by_email'
go
create procedure [sp_select_employee_by_email]
(
	@Email          [nvarchar](250)
)
as
begin
select
	[EmployeeID],
	[FirstName],
	[LastName],
	[UserName],
	[PhoneNumber]
from [Employee]
where [Email] = @Email
end
go

print '' print '*** Creating sp_select_roles_by_employeeID'
go
create procedure [sp_select_roles_by_employeeID]
(
	@EmployeeID          [int]
)
as
begin
select
	[RoleID]
from [EmployeeRole]
where [EmployeeID] = @EmployeeID
end
go

print '' print '*** Creating sp_select_status_by_player_id'
go
create procedure [sp_select_status_by_player_id]
(
	@PlayerID          [int]
)
as
begin
select
	[PlayerStatus]
from [Player]
where [PlayerID] = @PlayerID
end
go

print '' print '*** Creating sp_select_quest_by_id'
go
create procedure [sp_select_quest_by_id]
(
	@QuestID          [int]
)
as
begin
select
	[QuestID] , [QuestName], [QuestDescription], [WorthExp], [Active]
from [Quest]
where [QuestID] = @QuestID
end
go

print '' print '*** Creating sp_select_users_by_active'
go
create procedure [sp_select_users_by_active]
(
    @Active            [bit]
)
as
begin
    select [EmployeeID],[FirstName],[LastName],[UserName],[PhoneNumber],[Email],[Active]
    from [Employee]
    where [Active] = @Active
    order by [LastName]
end
go

print '' print '*** Creating sp_select_quests_by_active'
go
create procedure [sp_select_quests_by_active]
(
    @Active            [bit]
)
as
begin
    select [QuestID],[QuestName],[QuestDescription],[WorthExp], [Active]
    from [Quest]
    where [Active] = @Active
    order by [QuestName]
end
go

print '' print '*** Creating sp_update_password'
go
create procedure [sp_update_password]
(
	@EmployeeID		[int],
	@OldPasswordHash	[nvarchar](100),
	@NewPasswordHash	[nvarchar](100)
)
as
begin
    update [dbo].[Employee]
    set [PasswordHash] = @NewPasswordHash
    where [EmployeeID] = @EmployeeID
    and [PasswordHash] = @OldPasswordHash
    and [Active] = 1
    return @@ROWCOUNT
end
go

print '' print '*** Creating sp_update_player_password'
go
create procedure [sp_update_player_password]
(
	@PlayerID		[int],
	@OldPasswordHash	[nvarchar](100),
	@NewPasswordHash	[nvarchar](100)
)
as
begin
    update [dbo].[Player]
    set [PasswordHash] = @NewPasswordHash
    where [PlayerID] = @PlayerID
    and [PasswordHash] = @OldPasswordHash
    return @@ROWCOUNT
end
go

print '' print '*** Creating Log4Net Table ***'
go
create table [dbo].[Log4Net] (
    [LogId] [int] identity (1, 1) not null,
    [LogDate] [datetime] not null,
    [LogThread] [varchar] (255) not null,
    [LogLevel] [varchar] (50) not null,
    [LogSource] [varchar] (255) not null,
    [LogMessage] [varchar] (4000) not null,
    [LogException] [varchar] (2000) null,
	constraint [pk_LogId)] primary key([LogId] asc)
)
go

print '' print '*** Creating Procedure sp_create_log ***'
go
create procedure [dbo].[sp_create_log]
(
	@LogDate datetime,
	@LogThread varchar(50),
	@LogLevel varchar(50),
	@LogSource varchar(300),
	@LogMessage varchar(4000),
	@LogException varchar(4000)
)	
as
begin
	set NOCOUNT on;
insert into [dbo].[Log4Net]
	   ([LogDate], [LogThread], [LogLevel], [LogSource], [LogMessage], [LogException])
values (@LogDate, @LogThread, @LogLevel, @LogSource, @LogMessage, @LogException)
declare @LogID int
set @LogID = SCOPE_IDENTITY()
end
go

print '' print '*** Creating sp_select_player_by_email'
go
create procedure [sp_select_player_by_email]
(
	@Email          [nvarchar](250)
)
as
begin
select
	[PlayerID],
	[FirstName],
	[LastName],
	[UserName],
	[PlayerStatus]
from [Player]
where [Email] = @Email
end
go

print '' print '*** Creating PatchNote Table'
go

create table [dbo].[PatchNote](
	[PatchID]  		   [nvarchar](10)			 	not null,
	[PatchName]  	   [nvarchar](150)           	not null,
	[PatchDescription] [nvarchar](max)            	not null,
	[PatchDate]		   [datetime]                   not null,
	constraint [pk_PatchID] primary key([PatchID] asc),
)
go

print '' print '*** Creating Sample PatchNote Records'
go

insert into [dbo].[PatchNote]
	([PatchID], [PatchName], [PatchDescription], [PatchDate])
	values
	('0.0.0.1', 'Version One', 'The basic application is created', '2020-05-09 12:47:00:000')	
go

print '' print '*** Creating PatchLine Table'
go

create table [dbo].[PatchLine](
	[PatchLineID]  	   [int]identity(1000000,1)	    not null,
	[PatchID]		   [nvarchar](10)               not null,
	[PatchLineName]    [nvarchar](150)           	not null,
	[PatchLineDescription] [nvarchar](max)            	not null,
	constraint [pk_PatchLineID] primary key([PatchLineID] asc),
	constraint [fk_PatchNote_PatchID] foreign key([PatchID])
		references [PatchNote]([PatchID])
)
go

print '' print '*** Creating sp_select_all_patch_notes'
go
create procedure [sp_select_all_patch_notes]
as
begin
    select [PatchID],[PatchName],[PatchDescription],[PatchDate]
    from [PatchNote]
    order by [PatchDate]
end
go

print '' print '*** Creating sp_select_most_recent_patch'
go
create procedure [sp_select_most_recent_patch]
as
begin
    select [PatchID],[PatchName],[PatchDescription],[PatchDate]
    from [PatchNote]
    where PatchDate IN (SELECT max(PatchDate) FROM PatchNote);
end
go

print '' print '*** Creating sp_select_players_quests'
go
create procedure [sp_select_players_quests]
(
	@PlayerID          [int]
)
as
begin
select
	[PlayerQuest].[QuestID],
	[Quest].[QuestName],
	[Quest].[QuestDescription],
	[Quest].[WorthExp],
	[Quest].[Active],
	[PlayerQuest].[Completed]
from [PlayerQuest] inner join [Quest] on [PlayerQuest].[QuestID] = [Quest].[QuestID]
where [PlayerID] = @PlayerID
end
go

print '' print '*** Creating Sample PlayerQuest Records'
go

insert into [dbo].[PlayerQuest]
	([QuestID], [PlayerID])
	values
	(1000000, 1000000)
go
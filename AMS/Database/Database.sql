DELIMITER ;

/*
-- ---------------------------------------------------------- 
-- Drop the database
-- ---------------------------------------------------------- 
*/
DROP DATABASE IF EXISTS db_a48a53_ams;

/*
-- --------------------------------------------------------------------------------------------------------------------
--Create statements for database
-- --------------------------------------------------------------------------------------------------------------------
*/
CREATE DATABASE IF NOT EXISTS db_a48a53_ams;

/*
-- --------------------------------------------------------------------------------------------------------------------
--Create statements for EditUser
-- --------------------------------------------------------------------------------------------------------------------
*/
--DROP USER IF EXISTS 'AMSEditUser'@'localhost';
--CREATE USER IF NOT EXISTS 'AMSEditUser'@'localhost' IDENTIFIED BY 'yViEpZpg7i&11*f&O';

/*
-- --------------------------------------------------------------------------------------------------------------------
--Create statements for ReadUser
-- --------------------------------------------------------------------------------------------------------------------
*/
--DROP USER IF EXISTS 'AMSReadUser'@'localhost';
--CREATE USER 'AMSReadUser'@'localhost' IDENTIFIED BY '&3kn90kR5w&PUNlZR';

USE db_a48a53_ams;

/*
-- ---------------------------------------------------------- 
-- Statements to drop all the tables
-- ---------------------------------------------------------- 
*/
DROP TABLE IF EXISTS UserLocationRoles;
DROP TABLE IF EXISTS UserOrganizationRoles; 
DROP TABLE IF EXISTS AssetLocations;
DROP TABLE IF EXISTS LocationRoles;
DROP TABLE IF EXISTS UserAssets;
DROP TABLE IF EXISTS TicketLocations;
DROP TABLE IF EXISTS AssetsTickets;
DROP TABLE IF EXISTS UserTicketsRoles;
DROP TABLE IF EXISTS UserNotes;
DROP TABLE IF EXISTS TicketNotes;
DROP TABLE IF EXISTS Queues;
DROP TABLE IF EXISTS LoanedAssets;
DROP TABLE IF EXISTS Assets;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Tickets; 
DROP TABLE IF EXISTS Locations; 
DROP TABLE IF EXISTS States;
DROP TABLE IF EXISTS Models; 
DROP TABLE IF EXISTS Devices; 
DROP TABLE IF EXISTS Brands; 
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS Organizations; 
DROP TABLE IF EXISTS Categories; 
DROP TABLE IF EXISTS Statuses;
DROP TABLE IF EXISTS Notes;

/*
-- ---------------------------------------------------------- 
-- Notes Table 
-- Description: Comments/information/Emails on the ticket
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Notes( 
 NoteID int NOT NULL AUTO_INCREMENT,
 Description text NOT NULL,
 DateCreated datetime NOT NULL,
 CONSTRAINT Notes_pk PRIMARY KEY (NoteID) 
) ENGINE = InnoDB;
/*
-- ----------------------------------------------------------
-- Statuses Table  
-- Description: These are conditions that a ticket can be in,
--   such as open, closed, awaiting vendor
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Statuses( 
 StatusID int NOT NULL AUTO_INCREMENT,
 Status varchar(255) NOT NULL,
 CONSTRAINT Statuses_pk PRIMARY KEY (StatusID) 
) ENGINE = InnoDB;

/* 
-- ---------------------------------------------------------- 
-- Categories Table
-- Description: A category is a grouping that a ticket can be 
--   apart of depending on what is being tracked. An example 
--   would be a user needs to get a system downloaded on a PC,
--   it would be a system category.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Categories( 
 CategoryID int NOT NULL AUTO_INCREMENT,
 Name varchar(50) NOT NULL,
 CONSTRAINT Categories_pk PRIMARY KEY (CategoryID) 
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Organizations Table  
-- Description: This can be a group of people or bodies that
--   have a particular purpose for the users
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Organizations( 
 OrganizationID int NOT NULL AUTO_INCREMENT,
 Description varchar(100) NOT NULL,
 Name varchar(255) NOT NULL,
 CONSTRAINT Organizations_pk PRIMARY KEY (OrganizationID) 
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Roles Table  
-- Description: These are the different levels of access and 
--   ability that a user can have
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Roles( 
 RoleID int NOT NULL AUTO_INCREMENT,
 Title varchar(255) NOT NULL,
 TicketsView tinyint DEFAULT 0,
 TicketsComment tinyint DEFAULT 0,
 TicketsOpen tinyint DEFAULT 0,
 TicketsResolve tinyint DEFAULT 0,
 TicketsEdit tinyint DEFAULT 0,
 AssetsArchive tinyint DEFAULT 0,
 AssetsView tinyint DEFAULT 0,
 AssetsEdit tinyint DEFAULT 0,
 AssetsAdd tinyint DEFAULT 0,
 UsersView tinyint DEFAULT 0,
 UsersAdd tinyint DEFAULT 0,
 UsersEdit tinyint DEFAULT 0,
 UsersDisable tinyint DEFAULT 0,
 RolesView tinyint DEFAULT 0,
 DeleteAsset tinyint DEFAULT 0,
 CONSTRAINT Roles_pk PRIMARY KEY (RoleID) 
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Brands Table  
-- Description: What company or institutions built or 
--   supplied the asset
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Brands( 
 BrandID int NOT NULL AUTO_INCREMENT,
 Name varchar(255) NOT NULL,
 DateArchived datetime DEFAULT '9999-12-31 23:59:59',
 CONSTRAINT Brands_pk PRIMARY KEY (BrandID)
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Devices Table  
-- Description: What an asset can be. A device would be a 
-- computer, printer, projector, etc.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Devices( 
 DeviceID int NOT NULL AUTO_INCREMENT,
 Name varchar(255) NOT NULL,
 DateArchived datetime DEFAULT '9999-12-31 23:59:59',
 BrandID int NOT NULL,
 CONSTRAINT Devices_pk PRIMARY KEY (DeviceID),
 CONSTRAINT Devices_BrandID 
	FOREIGN KEY (BrandID) REFERENCES Brands(BrandID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Models Table  
-- Description: What version or make the device can be. 
--   Different laptops from the same company would be a 
--   different model.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Models( 
 ModelID int NOT NULL AUTO_INCREMENT,
 Name varchar(255) NOT NULL,
 DateArchived datetime DEFAULT '9999-12-31 23:59:59',
 DeviceID int NOT NULL,
 CONSTRAINT Models_pk PRIMARY KEY (ModelID),
 CONSTRAINT Models_DeviceID
	FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
) ENGINE = InnoDB; 

/* 

-- ---------------------------------------------------------- 
-- States Table  
-- Description: A state is a condition assigned to an asset. 
--   These are different that a status which are the 
--   conditions of tickets. A state for a device would broken,
--   out for repair, or in good condition.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS States( 
 StateID int NOT NULL AUTO_INCREMENT,
 Name varchar(255) NOT NULL,
 DateArchived datetime DEFAULT '9999-12-31 23:59:59',
 CONSTRAINT States_pk PRIMARY KEY (StateID) 
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Table  
-- Description: 
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Locations( 
 LocationID int NOT NULL AUTO_INCREMENT,
 Name varchar(255) NOT NULL,
 DateArchived datetime DEFAULT '9999-12-31 23:59:59',
 StateID int NOT NULL,
 PLocationID int,
 CONSTRAINT Locations_pk PRIMARY KEY (LocationID)
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Tickets Table  
-- Description: Tickets are used to track assets and issues 
--    WITH the system
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Tickets( 
 TicketID int NOT NULL AUTO_INCREMENT,
 Number int NOT NULL UNIQUE,
 Subject varchar(150) NOT NULL,
 Description text NOT NULL,
 DateCreated datetime NOT NULL,
 DateLastUpdated datetime NOT NULL,
 DateDue datetime DEFAULT '9999-12-31 23:59:59',
 DateResolved datetime DEFAULT '9999-12-31 23:59:59',
 StatusID int NOT NULL,
 CategoryID int NOT NULL,
 CONSTRAINT Tickets_pk PRIMARY KEY (TicketID),
 CONSTRAINT Tickets_StatusID 
	FOREIGN KEY (StatusID) REFERENCES Statuses(StatusID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT Tickets_CategoryID
	FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Users Table  
-- Description: Someone who will interact with the software 
--   (Customers, Technicians, Owner, CC)
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Users( 
 UserID int NOT NULL AUTO_INCREMENT,
 Email varchar(60) NOT NULL UNIQUE,
 Password varchar(255) NOT NULL,
 Salt varchar(255) NOT NULL,
 FirstName varchar(25) NOT NULL,
 LastName varchar(25) NOT NULL,
 DateDisabled datetime DEFAULT '9999-12-31 23:59:59',
 RoleID int NOT NULL,
 CONSTRAINT Users_pk PRIMARY KEY (UserID),
 CONSTRAINT Users_RoleID
	FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Assets Table  
-- Description: Models that are being inventoried or tracked
--   (computers, printers, projecters, etc.)
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Assets( 
 AssetID int NOT NULL AUTO_INCREMENT,
 InventoryNumber varchar(30) NOT NULL UNIQUE,
 DatePurchased datetime NOT NULL,
 DateWarrantyExpires datetime DEFAULT '9999-12-31 23:59:59',
 DateArchived datetime DEFAULT '9999-12-31 23:59:59',
 IsLoanable tinyint NOT NULL,
 IsLoaned tinyint DEFAULT 0,
 ModelID int NOT NULL,
 StateID int NOT NULL,
 LocationID int NOT NULL,
 CONSTRAINT Assets_pk PRIMARY KEY (AssetID),
 CONSTRAINT Assets_ModelID
	FOREIGN KEY (ModelID) REFERENCES Models(ModelID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT Assets_StateID
	FOREIGN KEY (StateID) REFERENCES States(StateID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT Assets_LocationID
	FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- LoanedAssets Table  
-- Description: stores all assets that have been loaned
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS LoanedAssets(
 LoanedAssetID int NOT NULL AUTO_INCREMENT,
 UserID int NOT NULL,
 AssetID int NOT NULL unique,
 DateExpectedReturn datetime DEFAULT now(),
 CONSTRAINT LoanedAssets_pk PRIMARY KEY (LoanedAssetID),
 CONSTRAINT LoanedAssets_AssetID
	FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- Queues Table  
-- Description: customized queues for users
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Queues( 
 QueueID int NOT NULL AUTO_INCREMENT,
 TicketID int NOT NULL,
 UserID int NOT NULL,
 Name CHAR NOT NULL,
 CONSTRAINT Queues_pk PRIMARY KEY (QueueID), 
 CONSTRAINT Queues_TicketID
	FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
CONSTRAINT Queues_UserID
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB;
/* 
-- ---------------------------------------------------------- 
-- Emails 
-- Description: Stores sent emails for tickets
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS Emails(
EmailID int NOT NULL AUTO_INCREMENT,
DateSent datetime NOT NULL,
SenderID int NOT NULL,
SUBJECT varchar(255) NOT NULL,
Body longtext NOT NULL,
CONSTRAINT Emails_pk PRIMARY KEY (EmailID),
CONSTRAINT Emails_SenderID
	FOREIGN KEY (SenderID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB;

/*
-- --------------------------------------------------------------------------------------------------------------------
-- ASSOCIATIVE TABLES
-- --------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------- 
-- TicketNotes Table  
-- Description: This is an associative table that connects
--   notes to tickets by ID, A ticket can have many notes
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS TicketNotes( 
 TicketNoteID int NOT NULL AUTO_INCREMENT,
 TicketID int NOT NULL,
 NoteID int NOT NULL,
 CONSTRAINT TicektNotes_pk PRIMARY KEY (TicketNoteID),
 CONSTRAINT TicketNotes_TicketID
 FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT TicketNotes_NoteID
 FOREIGN KEY (NoteID) REFERENCES Notes(NoteID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- UserNotes Table  
-- Description: Associative table that connects notes to their
--  users.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS UserNotes( 
 UserNoteID int NOT NULL AUTO_INCREMENT,
 UserID int NOT NULL,
 NoteID int NOT NULL,
 CONSTRAINT UserNotes_pk PRIMARY KEY (UserNoteID),
 CONSTRAINT UserNotes_UserID
 FOREIGN KEY (UserID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserNotes_NoteID
 FOREIGN KEY (NoteID) REFERENCES Notes(NoteID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION 
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- UserTicketsRoles Table  
-- Description: Connects users to tickets with their Roles
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS UserTicketsRoles( 
 UserTicketsRoleID int NOT NULL AUTO_INCREMENT,
 UserID int NOT NULL,
 TicketID int NOT NULL,
 RoleID int NOT NULL,
 CONSTRAINT UserTicketsRoles_pk PRIMARY KEY (UserTicketsRoleID), 
 CONSTRAINT UserTicketsRoles_UserID
 FOREIGN KEY (UserID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserTicketsRoles_TicketID
 FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserTicketsRoles_RoleID
 FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION 
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- TicketAssets Table  
-- Description: This associative table connects all the Assets
--   that belong to a ticket. A ticket can have many assets.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS TicketAssets( 
 TicketAssetID int NOT NULL AUTO_INCREMENT,
 TicketID int NOT NULL,
 AssetID int NOT NULL,
 CONSTRAINT AssetsTickets_pk PRIMARY KEY (TicketAssetID), 
 CONSTRAINT AssetsTickets_TicketID
 FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT AssetsTickets_AssetID
 FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- TicketLocations Table  
-- Description: Associative table that connects tickets to 
--   Locations
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS TicketLocations( 
 TicketLocationID int NOT NULL AUTO_INCREMENT,
 TicketID int NOT NULL,
 LocationID int NOT NULL,
 CONSTRAINT TicketLocations_pk PRIMARY KEY (TicketLocationID), 
 CONSTRAINT TicketLocations_TicketID
 FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT TicketLocations_LocationID
 FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- LocationRoles Table  
-- Description: This associative table connects the locations
--   to their roles
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS LocationRoles( 
 LocationRoleID int NOT NULL AUTO_INCREMENT,
 LocationID int NOT NULL,
 RoleID int NOT NULL,
 CONSTRAINT LocationRoles_pk PRIMARY KEY (LocationRoleID), 
 CONSTRAINT LocationRoles_RoleID
 FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT LocationRoles_LocationID
 FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- AssetLocations Table  
-- Description: This is a table that connects assets to their
--   locations, it also stores if the asset is critical to 
--   the location
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS AssetLocations( 
 AssetLocationID int NOT NULL AUTO_INCREMENT,
 LocationID int NOT NULL,
 AssetID int NOT NULL,
 IsCritical tinyint NOT NULL,
 IsPermanent tinyint NOT NULL,
 CONSTRAINT AssetLocations_pk PRIMARY KEY (AssetLocationID), 
 CONSTRAINT AssetLocations_LocationID
 FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT AssetLocations_AssetID
 FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- UserOrganizationRoles Table  
-- Description: This is an associative table that connects 
--   Users to their organization and specifies their role
--   in that organization.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS UserOrganizationRoles( 
 UserOrganizationRoleID int NOT NULL AUTO_INCREMENT,
 UserID int NOT NULL,
 OrganizationID int NOT NULL,
 RoleID int NOT NULL,
 CONSTRAINT UserOrganizationRoles_pk PRIMARY KEY (UserOrganizationRoleID), 
 CONSTRAINT UserOrganizationRoles_UserID
 FOREIGN KEY (UserID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserOrganizationRoles_OrganizationID
 FOREIGN KEY (OrganizationID) REFERENCES Organizations(OrganizationID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserOrganizationRoles_RoleID
 FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
) ENGINE = InnoDB; 
/* 
-- ---------------------------------------------------------- 
-- UserLocationRoles Table  
-- Description: This is an associative table that connects 
--   Users to their Location and specifies their role
--   in that location.
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS UserLocationRoles (
 UserLocationRoleID int NOT NULL AUTO_INCREMENT,
 UserID int NOT NULL,
 LocationID int NOT NULL,
 RoleID int NOT NULL,
 CONSTRAINT UserLocationRoles_pk PRIMARY KEY (UserLocationRoleID), 
 CONSTRAINT UserLocationRoles_UserID
 FOREIGN KEY (UserID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserLocationRoles_LocationID
 FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserLocationRoles_RoleID
 FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
 ) ENGINE = InnoDB; 
 /* 
-- ---------------------------------------------------------- 
-- UserQueues Table  
-- Description: This is an associative table that connects 
--   Users to their queues
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS UserQueues (
 UserQueueID int NOT NULL AUTO_INCREMENT,
 UserID int NOT NULL,
 QueueID int NOT NULL,
 CONSTRAINT UserQueues_pk PRIMARY KEY (UserQueueID), 
 CONSTRAINT UserQueues_UserID
 FOREIGN KEY (UserID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT UserQueues_QueueID
 FOREIGN KEY (QueueID) REFERENCES Queues(QueueID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
 ) ENGINE = InnoDB; 

/* 
-- ---------------------------------------------------------- 
-- QueueTickets Table  
-- Description: This is an associative table that connects 
--   Tickets to their Queues
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS QueueTickets (
 QueueTicketID int NOT NULL AUTO_INCREMENT,
 TicketID int NOT NULL,
 QueueID int NOT NULL,
 CONSTRAINT QueueTickets_pk PRIMARY KEY (QueueTicketID), 
 CONSTRAINT QueueTickets_TicketID
 FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT QueueTickets_QueueID
 FOREIGN KEY (QueueID) REFERENCES Queues(QueueID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
 ) ENGINE = InnoDB; 
 /* 
-- ---------------------------------------------------------- 
-- TicketEmails Table  
-- Description: This is an associative table that connects 
--   Tickets to their emails
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS TicketEmails (
 TicketEmailID int NOT NULL AUTO_INCREMENT,
 TicketID int NOT NULL,
 EmailID int NOT NULL,
 CONSTRAINT TicketEmails_pk PRIMARY KEY (TicketEmailID), 
 CONSTRAINT TicketEmails_TicketID
 FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT TicketEmails_EmailID
 FOREIGN KEY (EmailID) REFERENCES Emails(EmailID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
 ) ENGINE = InnoDB; 

  /* 
-- ---------------------------------------------------------- 
-- EmailRecipients Table  
-- Description: This is an associative table that connects 
--   Emails to their recipients
-- ----------------------------------------------------------
*/
CREATE TABLE IF NOT EXISTS EmailRecipients (
 EmailRecipientID int NOT NULL AUTO_INCREMENT,
 EmailID int NOT NULL,
 RecipientID int NOT NULL,
 CONSTRAINT EmailRecipients_pk PRIMARY KEY (EmailRecipientID), 
 CONSTRAINT EmailRecipients_EmailID
 FOREIGN KEY (EmailID) REFERENCES Emails(EmailID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
 CONSTRAINT EmailRecipients_UserID
 FOREIGN KEY (RecipientID) REFERENCES Users(UserID)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
 ) ENGINE = InnoDB; 


 /*
-- --------------------------------------------------------------------------------------------------------------------
-- STORED PROCEDURES
-- --------------------------------------------------------------------------------------------------------------------
*/

 DELIMITER ;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Assets
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetAssets
-- Results: Gets all assets with FK IDs And Details From
-- 	database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetAssets;;

CREATE PROCEDURE sproc_GetAssets()
BEGIN  
	SELECT A.AssetID, A.InventoryNumber, A.DatePurchased, 
			A.DateWarrantyExpires, A. DateArchived, A.IsLoanable, A.IsLoaned, A.ModelID,			
			M.Name AS Model, A.StateID, S.Name, A.LocationID,
			L.Name AS Location
	FROM Assets AS A 
	LEFT JOIN Models AS M 
		ON A.ModelID = M.ModelID
	LEFT JOIN States AS S 
		ON A.StateID = S.StateID
	LEFT JOIN Locations AS L 
		ON A.LocationID = L.LocationID;
END;;
/*
-- ----------------------------------------------------------
-- GetAssetByID
-- Results: Gets by ID from assets with FK IDs And Details From
-- 	database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetAssetByID;;

CREATE PROCEDURE sproc_GetAssetByID(
 IN i_AssetID int
)
BEGIN 
	SELECT A.AssetID, A.InventoryNumber, A.DatePurchased, 
			A.DateWarrantyExpires, A.DateArchived, A.IsLoanable, A.IsLoaned, A.ModelID,			
			M.Name AS Model, A.StateID, S.Name, A.LocationID,
			L.Name AS Location
	FROM Assets AS A 
	LEFT JOIN Models AS M 
		ON A.ModelID = M.ModelID
	LEFT JOIN States AS S 
		ON A.StateID = S.StateID
	LEFT JOIN Locations AS L 
		ON A.LocationID = L.LocationID
	WHERE A.AssetID = i_AssetID;
END;;

/*
-- ----------------------------------------------------------
-- GetAssetLike
-- Results: Returns asset that has an inventoryNumber, Device name
-- Brand Name, or Model Name that matches the parameter
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetAssetsLike;;

CREATE PROCEDURE sproc_GetAssetsLike(
 IN i_search varchar(255)
)
BEGIN 
	SELECT A.AssetID, A.InventoryNumber, A.DatePurchased, 
		A.DateWarrantyExpires, A.DateArchived, A.IsLoanable, A.IsLoaned, A.ModelID,			
		M.Name AS Model, A.StateID, S.Name, A.LocationID, D.Name AS Device,
		B.Name AS Brand, L.Name AS Location
	FROM Assets AS A 
	LEFT JOIN Models AS M 
		ON A.ModelID = M.ModelID
	LEFT JOIN Devices AS D
		ON M.DeviceID = D.DeviceID
	LEFT JOIN Brands AS B
		ON D.BrandID = B.BrandID
	LEFT JOIN States AS S 
		ON A.StateID = S.StateID
	LEFT JOIN Locations AS L 
		ON A.LocationID = L.LocationID
	WHERE A.InventoryNumber LIKE CONCAT('%', i_search, '%') OR M.Name LIKE CONCAT('%', i_search, '%')
	   OR B.Name LIKE CONCAT('%', i_search, '%') OR D.name LIKE CONCAT('%', i_search, '%');
END;;

/*
-- ----------------------------------------------------------
-- GetAssetsByTicketID
-- Results: Gets by ID from assets with FK IDs And Details From
-- 	database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetAssetsByTicketID;;

CREATE PROCEDURE sproc_GetAssetsByTicketID(
 IN i_TicketID int
)
BEGIN 
	SELECT A.AssetID, A.InventoryNumber, A.DatePurchased, 
			A.DateWarrantyExpires, A.DateArchived, A.IsLoanable, A.IsLoaned, A.ModelID,			
			M.Name AS Model, A.StateID, S.Name AS State, A.LocationID,
			L.Name AS Location
	FROM TicketAssets AS TA
	LEFT JOIN Assets AS A
		ON TA.AssetID = A.AssetID
	LEFT JOIN Models AS M 
		ON A.ModelID = M.ModelID
	LEFT JOIN States AS S 
		ON A.StateID = S.StateID
	LEFT JOIN Locations AS L 
		ON A.LocationID = L.LocationID
	WHERE TA.TicketID = i_TicketID;
END;;
/*
-- ----------------------------------------------------------
-- AddAsset
-- Results: Add an asset with FK ID to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddAsset;;

CREATE PROCEDURE sproc_AddAsset(
IN i_InventoryNumber		 varchar(30)
,IN i_DatePurchased			 datetime
,IN i_DateWarrantyExpires	 datetime
,IN i_IsLoanable			 tinyint
,IN i_ModelID				 int
,IN i_StateID				 int
,IN i_LocationID			 int
)
BEGIN 
	INSERT INTO Assets (InventoryNumber, DatePurchased,
		DateWarrantyExpires, IsLoanable, ModelID, StateID,
		LocationID) 
	VALUES (i_InventoryNumber, i_DatePurchased, 
			i_DateWarrantyExpires, i_IsLoanable, i_ModelID,
			i_StateID, i_LocationID);
END;;
/*
-- ----------------------------------------------------------
-- UpdateAsset
-- Results: Update an asset by its ID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateAssetByID;;

CREATE PROCEDURE sproc_UpdateAssetByID(
IN i_AssetID 				 int
,IN i_InventoryNumber		 varchar(30)
,IN i_DatePurchased			 datetime
,IN i_DateWarrantyExpires 	 datetime
,IN i_IsLoanable			 tinyint
,IN i_ModelID				 int
,IN i_StateID				 int
,IN i_LocationID			 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Assets WHERE AssetID = i_AssetID) THEN 
		Update Assets 
			SET	InventoryNumber = i_InventoryNumber,
				DatePurchased = i_DatePurchased,
				DateWarrantyExpires = i_DateWarrantyExpires,
				IsLoanable = i_IsLoanable, 
				ModelID = i_ModelID,
				StateID = i_StateID,
				LocationID = i_LocationID
			WHERE AssetID = i_AssetID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ArchiveAssetByID
-- Result: Update the DateArchived field on an asset by AssetID
--		ASSET IS NOT DELETED COMPLETELY
-- ---------------------------------------------------------- 
*/

DROP PROCEDURE IF EXISTS sproc_ArchiveAssetByID;;

CREATE PROCEDURE sproc_ArchiveAssetByID(
IN i_AssetID int)
BEGIN 
	IF EXISTS (SELECT * FROM Assets WHERE AssetID = i_AssetID) THEN
		UPDATE Assets
			SET DateArchived = Now()
			WHERE AssetID = i_AssetID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateDateWarrantyExpiresByAssetID 
--  This PROCEDURE takes in an AssetID and a date for when
-- 		the warranty expires
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateDateWarrantyExpiresByAssetID;;

CREATE PROCEDURE sproc_UpdateDateWarrantyExpiresByAssetID(
IN i_AssetID 	int
,IN i_Date		datetime)
BEGIN
	IF EXISTS(SELECT * FROM Assets WHERE AssetID = i_AssetID) THEN
		UPDATE Assets
			SET DateWarrantyExpires = i_Date 
			WHERE AssetID = i_AssetID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;


/*
-- ----------------------------------------------------------
-- UpdateIsLoanableByAssetID
--  This Procedure takes in the AssetID and a 0 or 1 
-- 		0 = Not Loanable; 1 = Loanable
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateIsLoanableByAssetID;;

CREATE PROCEDURE sproc_UpdateIsLoanableByAssetID(
IN i_AssetID 		int
,IN i_IsLoanable 	tinyint)
BEGIN
	IF EXISTS(SELECT * FROM Assets WHERE AssetID = i_AssetID) THEN
		UPDATE Assets
			SET IsLoanable = i_IsLoanable 
			WHERE AssetID = i_AssetID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;


/*
-- ----------------------------------------------------------
-- UpdateLocationIDByAssetID 
--  This PROCEDURE takes in an assetID and a LocationID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateLocationIDByAssetID;;

CREATE PROCEDURE sproc_UpdateLocationIDByAssetID(
IN i_AssetID 		int
,IN i_LocationID	int)
BEGIN
	IF EXISTS(SELECT * FROM Assets WHERE AssetID = i_AssetID) THEN
		UPDATE Assets
			SET LocationID = i_LocationID 
			WHERE AssetID = i_AssetID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;


/*
-- ----------------------------------------------------------
-- UpdateStateIDByAssetID 
--  This PROCEDURE takes in an AssetID and a StateID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateStateIDByAssetID;;

CREATE PROCEDURE sproc_UpdateStateIDByAssetID(
IN i_AssetID 	int
,IN i_StateID 	int)
BEGIN
	IF EXISTS(SELECT * FROM Assets WHERE AssetID = i_AssetID) THEN
		UPDATE Assets
			SET StateID = i_StateID
			WHERE AssetID = i_AssetID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Assets - LoanedAssets
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- UpdateDateExpectedReturnByAssetID 
--  Update the expected returned date of a loaned asset by AssetID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateDateExpectedReturnByAssetID;;

CREATE PROCEDURE sproc_UpdateDateExpectedReturnByAssetID(
IN i_AssetID int
,IN i_DateExpectedReturn datetime)
BEGIN
	IF EXISTS(SELECT * FROM LoanedAssets WHERE AssetID = i_AssetID) THEN
		UPDATE LoanedAssets
			SET DateExpectedReturn = i_DateExpectedReturn
			WHERE AssetID= i_AssetID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- GetLoanedAssets 
--  Gets all Loaned assets
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetLoanedAssets;;

CREATE PROCEDURE sproc_GetLoanedAssets()
BEGIN
	SELECT * FROM LoanedAssets;
END;;

/*
-- ----------------------------------------------------------
-- AddLoanedAssets 
--  Adds Loaned asset
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddLoanedAsset;;

CREATE PROCEDURE sproc_AddLoanedAsset(
IN i_UserID int
,IN i_AssetID int
,IN i_DateExpectedReturn datetime
)
BEGIN
	IF EXISTS (SELECT * FROM LoanedAssets WHERE UserID = i_UserID AND AssetID = i_AssetID) THEN
		SELECT 'That association already exists' AS Error;
	ELSE
		IF EXISTS (SELECT * FROM Assets WHERE AssetID = i_AssetID AND IsLoanable = 1) THEN
			INSERT INTO LoanedAssets (UserID, AssetID, DateExpectedReturn) VALUES (i_UserID, i_AssetID, i_DateExpectedReturn);

			UPDATE Assets
				SET IsLoaned = 1
				WHERE AssetID = i_AssetID;
		ELSE
			SELECT 'Asset is not loanable' AS Error;
		END IF;
	END IF;
END;;


/*
-- ----------------------------------------------------------
-- GetLoanedAssetAndUserByAssetID 
--  Gets all fields of a loaned asset By AssetID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetLoanedAssetAndUserByAssetID;;

CREATE PROCEDURE sproc_GetLoanedAssetAndUserByAssetID(
IN i_AssetID int)
BEGIN
	IF EXISTS(SELECT * FROM LoanedAssets WHERE AssetID = i_AssetID) THEN
		SELECT A.AssetID, A.InventoryNumber, A.DatePurchased, 
				A.DateWarrantyExpires, A. DateArchived, A.IsLoanable, 
				A.IsLoaned, A.ModelID, M.Name AS Model, A.StateID, 
				S.Name, A.LocationID, L.Name AS Location,
				U.UserID, U.Email, U.FirstName, U.LastName, U.RoleID,
				U.DateDisabled, R.Title, LA.DateExpectedReturn
			FROM LoanedAssets AS LA
			LEFT JOIN Users AS U
				ON LA.UserID = U.UserID
			LEFT JOIN Roles AS R 
				ON U.RoleID = R.RoleID
			LEFT JOIN Assets AS A 
				ON LA.AssetID = A.AssetID
			LEFT JOIN Models AS M 
				ON A.ModelID = M.ModelID
			LEFT JOIN States AS S 
				ON A.StateID = S.StateID
			LEFT JOIN Locations AS L 
				ON A.LocationID = L.LocationID
			WHERE LA.AssetID = i_AssetID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- GetLoanedAssetAndUserByUserID 
--  Gets all fields of a loaned asset By UserID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetLoanedAssetAndUserByUserID;;

CREATE PROCEDURE sproc_GetLoanedAssetAndUserByUserID(
IN i_UserID int)
BEGIN
	IF EXISTS(SELECT * FROM LoanedAssets WHERE UserID = i_UserID) THEN
		SELECT A.AssetID, A.InventoryNumber, A.DatePurchased, 
				A.DateWarrantyExpires, A. DateArchived, A.IsLoanable, 
				A.IsLoaned, A.ModelID, M.Name AS Model, A.StateID, 
				S.Name, A.LocationID, L.Name AS Location,
				U.UserID, U.Email, U.FirstName, U.LastName, U.RoleID,
				U.DateDisabled, R.Title, LA.DateExpectedReturn
			FROM LoanedAssets AS LA
			LEFT JOIN Users AS U
				ON LA.UserID = U.UserID
			LEFT JOIN Roles AS R 
				ON U.RoleID = R.RoleID
			LEFT JOIN Assets AS A 
				ON LA.AssetID = A.AssetID
			LEFT JOIN Models AS M 
				ON A.ModelID = M.ModelID
			LEFT JOIN States AS S 
				ON A.StateID = S.StateID
			LEFT JOIN Locations AS L 
				ON A.LocationID = L.LocationID
			WHERE LA.UserID = i_UserID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- DeleteLoanedAssetByAssetID 
--  Delete a loaned asset entry by AssetID
--  and update the asset to not be loaned out
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_DeleteLoanedAssetByAssetID;;

CREATE PROCEDURE sproc_DeleteLoanedAssetByAssetID(
IN i_AssetID int)
BEGIN
	IF EXISTS(SELECT * FROM LoanedAssets WHERE AssetID = i_AssetID) THEN

		DELETE FROM LoanedAssets
			WHERE AssetID = i_AssetID;

		UPDATE Assets
			SET IsLoaned = 0
			WHERE AssetID = i_AssetID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- --------------------------------------------------------------------------------------------------------------------
-- Brands
-- --------------------------------------------------------------------------------------------------------------------
-- GetBrands
-- Results: Gets all Brands from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetBrands;;

CREATE PROCEDURE sproc_GetBrands()
BEGIN 
	SELECT * FROM Brands;
END;;
/*
-- ----------------------------------------------------------
-- GetBrandByID
-- Results: Gets by ID from Brands from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetBrandByID;;

CREATE PROCEDURE sproc_GetBrandByID(
 IN i_BrandID int
)
BEGIN 
	SELECT * FROM Brands 
	WHERE BrandID = i_BrandID;
END;;
/*
-- ----------------------------------------------------------
-- GetBrandByName
-- Results: Gets by name from Brands from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetBrandByName;;

CREATE PROCEDURE sproc_GetBrandByName(
 IN i_Name varchar(70)
)
BEGIN 
	SELECT * FROM Brands 
	WHERE Name = i_Name;
END;;
/*
-- ----------------------------------------------------------
-- AddBrand
-- Results: Add a Brand to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddBrand;;

CREATE PROCEDURE sproc_AddBrand(
IN i_Name varchar(70)
)
BEGIN 
	IF EXISTS (SELECT * FROM Brands WHERE Name = i_Name)THEN
		SELECT 'That Brand Already Exists' AS Error;
	ELSE
		INSERT INTO Brands (Name) VALUES (i_Name);
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateBrand
-- Results: Update a Brand in the database by ID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateBrandByID;;

CREATE PROCEDURE sproc_UpdateBrandByID(
IN i_BrandID int
,IN i_DateArchived datetime
,IN i_Name varchar(70)
)
BEGIN 
	IF EXISTS (SELECT * FROM Brands WHERE BrandID = i_BrandID) THEN 
		Update Brands
			SET Name = i_Name,
			DateArchived = i_DateArchived
		WHERE BrandID = i_BrandID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ArchiveBrandByID
-- Result: Archive Brand in database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ArchiveBrandByID;;

CREATE PROCEDURE sproc_ArchiveBrandByID(
IN i_BrandID int)
BEGIN 
	UPDATE Brands
	SET DateArchived = NOW()
	WHERE BrandID = i_BrandID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Categories
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetCategories
-- Results: Gets all categories from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetCategories;;

CREATE PROCEDURE sproc_GetCategories()
BEGIN 
	SELECT * FROM Categories;
END;;
/*
-- ----------------------------------------------------------
-- GetCategoryByID
-- Results: Gets by ID from categories from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetCategoryByID;;

CREATE PROCEDURE sproc_GetCategoryByID(
 IN i_CategoryID int
)
BEGIN 
	SELECT * FROM Categories 
	WHERE CategoryID = i_CategoryID;
END;;
/*
-- ----------------------------------------------------------
-- AddCategory
-- Results: Add a category to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddCategory;;

CREATE PROCEDURE sproc_AddCategory(
IN i_Name	 varchar(50)
)
BEGIN 
	INSERT INTO Categories (Name) VALUES (i_Name);
END;;
/*
-- ----------------------------------------------------------
-- UpdateCategory
-- Results: Update a category in the database by ID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateCategoryByID;;

CREATE PROCEDURE sproc_UpdateCategoryByID(
IN i_CategoryID int
,IN i_Name	 varchar(50)
)
BEGIN 
	IF EXISTS (SELECT * FROM Categories WHERE CategoryID = i_CategoryID) THEN 
		Update Categories
			SET Name = i_Name
		WHERE CategoryID = i_CategoryID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteCategoryByID
-- Result: Delete entry from database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteCategoryByID;;

CREATE PROCEDURE sproc_DeleteCategoryByID(
IN i_CategoryID int)
BEGIN 
	DELETE FROM Categories
	WHERE CategoryID = i_CategoryID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Devices
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetDevices7
-- Results: Gets all devices from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetDevices;;

CREATE PROCEDURE sproc_GetDevices()
BEGIN 
	SELECT D.DeviceID, D.Name, D.DateArchived, D.BrandID, B.Name AS Brand
		FROM Devices AS D
		LEFT JOIN Brands AS B
			ON D.BrandID = B.BrandID;
END;;
/*
-- ----------------------------------------------------------
-- GetDeviceByID
-- Results: Gets by ID from devices from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetDeviceByID;;

CREATE PROCEDURE sproc_GetDeviceByID(
 IN i_DeviceID int
)
BEGIN 
	SELECT D.DeviceID, D.Name, D.DateArchived, B.BrandID, B.Name AS Brand
		FROM Devices AS D
		LEFT JOIN Brands AS B
			ON D.BrandID = B.BrandID
	WHERE DeviceID = i_DeviceID;
END;;
/*
-- ----------------------------------------------------------
-- GetDeviceByName
-- Results: Gets by name from Devices from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetDeviceByName;;

CREATE PROCEDURE sproc_GetDeviceByName(
 IN i_Name varchar(70)
)
BEGIN 
	SELECT * FROM Devices 
	WHERE Name = i_Name;
END;;
/*
-- ----------------------------------------------------------
-- GetDeviceByBrandID
-- Results: Gets by BrandID from Devices and their BrandID with Brand Name
--  from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetDevicesByBrandID;;

CREATE PROCEDURE sproc_GetDevicesByBrandID(
 IN i_BrandID int
)
BEGIN 
	SELECT DeviceID, Name, DateArchived, BrandID
		FROM Devices 
	WHERE BrandID = i_BrandID;
END;;

/*
-- ----------------------------------------------------------
-- AddDevice
-- Results: Add a device to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddDevice;;

CREATE PROCEDURE sproc_AddDevice(
IN i_Name		 varchar(50)
,IN i_BrandID	 int
)
BEGIN 
	IF EXISTS( SELECT * FROM Devices WHERE Name = i_Name AND BrandID = i_BrandID) THEN
		SELECT 'That device already exists for the brand' AS Error;
	ELSE
		INSERT INTO Devices (Name, BrandID) VALUES (i_Name, i_BrandID);
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateDevice
-- Results: Update a device in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateDeviceByID;;

CREATE PROCEDURE sproc_UpdateDeviceByID(
IN i_DeviceID	 int
,IN i_Name		 varchar(50)
,IN i_DateArchived datetime
,IN i_BrandID	 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Devices WHERE DeviceID = i_DeviceID) THEN 
		Update Devices 
			SET Name = i_Name,
				DateArchived = i_DateArchived,
				BrandID = i_BrandID
			WHERE DeviceID = i_DeviceID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ArchiveDeviceByID
-- Result: Archive Device in database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ArchiveDeviceByID;;

CREATE PROCEDURE sproc_ArchiveDeviceByID(
IN i_DeviceID int)
BEGIN 
	UPDATE Devices
	SET DateArchived = NOW()
	WHERE DeviceID = i_DeviceID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Emails
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- AddRecipientToEmail
-- Results: Adds an association of a recipient (User) to an Email
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddRecipientToEmail;;

CREATE PROCEDURE sproc_AddRecipientToEmail(
IN i_EmailID int
,IN i_RecipientID int)
BEGIN
	IF NOT EXISTS (SELECT * FROM EmailRecipients WHERE RecipientID = i_RecipientID AND EmailID = i_EmailID) THEN
		INSERT INTO EmailRecipients (RecipientID, EmailID) VALUES (i_RecipientID, i_EmailID);
	ELSE
		SELECT 'That association already exists' AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- AddEmail
-- Results: Adds an association of a recipient (User) to an Email
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddEmail;;

CREATE PROCEDURE sproc_AddEmail(
IN i_DateSent datetime
,IN i_SenderID int
,IN i_Subject varchar(255)
,IN i_Body longtext)
BEGIN
	INSERT INTO EmailRecipients (DateSent, SenderID, Subject, Body) VALUES (i_DateSent, i_SenderID, i_Subject, i_Body);
END;;
/*
-- ----------------------------------------------------------
-- AddEmailToTicket
-- Results: Adds an association of a Email to a ticket
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddEmailToTicket;;

CREATE PROCEDURE sproc_AddEmailToTicket(
IN i_TicketID int
,IN i_EmailID int
)
BEGIN
	IF NOT EXISTS (SELECT * FROM TicketEmails WHERE TicketID = i_TicketID AND EmailID = i_EmailID) THEN
		INSERT INTO TicketEmails (TicketID, EmailID) VALUES (i_TicketID, i_EmailID);
	ELSE
		SELECT 'That Association already exists' AS Error;
	END IF;
END;;


/*
-- --------------------------------------------------------------------------------------------------------------------
-- Locations
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetLocations
-- Results: Gets all locations and foreign keys with their 
--  Parent location, if not parent exists it returns null for them
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetLocations;;

CREATE PROCEDURE sproc_GetLocations()
BEGIN  
	SELECT L.LocationID, L.Name, L.StateID, S.Name, L.DateArchived,
			L.PLocationID AS ParentLocationID, 
			PL.Name AS ParentLocation
		FROM Locations AS L  
		LEFT JOIN States AS S
			ON L.StateID = S.StateID
		LEFT JOIN Locations AS PL
			ON L.PLocationID = PL. LocationID;
END;;
/*
-- ----------------------------------------------------------
-- GetLocationByID
-- Results: Gets by ID from locations and foreign keys with their 
--  information
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetLocationByID;;

CREATE PROCEDURE sproc_GetLocationByID(
 IN i_LocationID int
)
BEGIN 
	SELECT L.LocationID, L.Name, L.StateID, L.DateArchived, S.Name,
			L.PLocationID AS ParentLocationID, 
			PL.Name AS ParentLocation
		FROM Locations AS L  
		LEFT JOIN States AS S
			ON L.StateID = S.StateID
		LEFT JOIN Locations AS PL
			ON L.PLocationID = PL.LocationID
	WHERE L.LocationID = i_LocationID;
END;;
/*
-- ----------------------------------------------------------
-- AddLocation
-- Results: Add a location and foreign keys
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddLocation;;

CREATE PROCEDURE sproc_AddLocation(
IN i_Name			 varchar(255)
,IN i_StateID		 int
,IN i_PLocationID	 int
)
BEGIN 
	IF NOT EXISTS ( SELECT * FROM Locations WHERE Name = i_Name)THEN
		INSERT INTO Locations (Name, StateID, PLocationID)
			VALUES (i_Name, i_StateID, i_PLocationID);
	ELSE
		SELECT 'That location already exists' AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateLocation
-- Results: Update a location in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateLocationByID;;

CREATE PROCEDURE sproc_UpdateLocationByID(
IN i_LocationID		 int
,IN i_Name			 varchar(255)
,IN i_StateID		 int
,IN i_PLocationID	 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Locations WHERE LocationID = i_LocationID) THEN 
		Update Locations 
			SET Name = i_Name,
				StateID = i_StateID,
				PLocationID = i_PLocationID
			WHERE LocationID = i_LocationID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ArchiveLocationByID
-- Result: Archive Location in database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ArchiveLocationByID;;

CREATE PROCEDURE sproc_ArchiveLocationByID(
IN i_LocationID int)
BEGIN 
	UPDATE Locations
	SET DateArchived = NOW()
	WHERE LocationID = i_LocationID;
END;;
/*
-- ----------------------------------------------------------
-- UpdatePLocationIDByLocationID
-- Result: Update the Parent LocationID of a location by its ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_UpdatePLocationIDByLocationID;;

CREATE PROCEDURE sproc_UpdatePLocationIDByLocationID(
IN i_LocationID int
,IN i_PLocationID int)
BEGIN
	IF EXISTS(SELECT * FROM Locations WHERE LocationID = i_LocationID) THEN
		UPDATE Locations
			SET PLocationID = i_PLocationID
			WHERE LocationID= i_LocationID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- UpdateStateIDByLocationID
-- Result: Update the StateID of a location by its ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_UpdateStateIDByLocationID;;

CREATE PROCEDURE sproc_UpdateStateIDByLocationID(
IN i_LocationID int
,IN i_StateID int)
BEGIN
	IF EXISTS(SELECT * FROM Locations WHERE LocationID = i_LocationID) THEN
		UPDATE Locations
			SET StateID = i_StateID
			WHERE LocationID= i_LocationID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Models
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetModels
-- Results: Gets all Models and their BrandID with Brand Name
--  from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetModels;;

CREATE PROCEDURE sproc_GetModels()
BEGIN 
	SELECT ModelID, Name, DateArchived, DeviceID
		FROM Models;
END;;
/*
-- ----------------------------------------------------------
-- GetModelByID
-- Results: Gets by ID from Models and their DeviceID with Device Name
--  from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetModelByID;;

CREATE PROCEDURE sproc_GetModelByID(
 IN i_ModelID int
)
BEGIN 
	SELECT ModelID, Name, DateArchived, DeviceID
		FROM Models 
	WHERE ModelID = i_ModelID;
END;;
/*
-- ----------------------------------------------------------
-- GetModelByDeviceID
-- Results: Gets by ID from Models and their DeviceID with Device Name
--  from database by the DeviceID passed in
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetModelsByDeviceID;;

CREATE PROCEDURE sproc_GetModelsByDeviceID(
 IN i_DeviceID int
)
BEGIN 
	SELECT ModelID, Name, DateArchived, DeviceID
		FROM Models 
	WHERE DeviceID = i_DeviceID;
END;;
/*
-- ----------------------------------------------------------
-- AddModel
-- Results: Add a Model its DeviceID to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddModel;;

CREATE PROCEDURE sproc_AddModel(
IN i_Name 		 varchar(120)
,IN i_DeviceID	 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Models WHERE Name = i_Name AND DeviceID = i_DeviceID)THEN
		SELECT 'That model already exists for the Device' AS Error;
	ELSE
		INSERT INTO Models (Name, DeviceID)
			VALUES (i_Name, i_DeviceID);
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateModel
-- Results: Update a Model its DeviceID in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateModelByID;;

CREATE PROCEDURE sproc_UpdateModelByID(
IN i_ModelID	 int
,IN i_Name 		 varchar(120)
,IN i_DateArchived datetime
,IN i_DeviceID	 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Models WHERE ModelID = i_ModelID) THEN 
		Update Models 
			SET Name = i_Name,
				DateArchived = i_DateArchived,
				DeviceID = i_DeviceID
			WHERE ModelID = i_ModelID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ArchiveModelByID
-- Result: Archive Model in database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ArchiveModelByID;;

CREATE PROCEDURE sproc_ArchiveModelByID(
IN i_ModelID int)
BEGIN 
	UPDATE Models
	SET DateArchived = NOW()
	WHERE ModelID = i_ModelID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Notes
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetNotes
-- Results: get all notes from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetNotes;;

CREATE PROCEDURE sproc_GetNotes()
BEGIN 
	SELECT * FROM Notes;
END;;
/*
-- ----------------------------------------------------------
-- GetNoteByID
-- Results: Gets all notes and their userID and FirstName from the database
	by TicketID
-- Error: Returns one column named "Error" if ticket is not found
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetNotesByTicketID;;

CREATE PROCEDURE sproc_GetNotesByTicketID(
 IN i_TicketID int
)
BEGIN 
	IF EXISTS ( SELECT * FROM Tickets WHERE TicketID = i_TicketID) THEN
		SELECT UN.UserID, UN.NoteID, N.Description, N.DateCreated FROM TicketNotes AS TN
			LEFT JOIN Notes AS N
				ON TN.NoteID = N.NoteID
			LEFT JOIN UserNotes AS UN
				ON N.NoteID = UN.NoteID
			WHERE TN.TicketID = i_TicketID;
	ELSE
		SELECT "No ticket with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- AddNote
-- Results: Add a note to the database with assciation to 
	UserNotes and TicketNotes, requires TicketID and UserID
-- Error: If the insert fails a column named "Error" will be
	returned, and message provided.
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddNote;;

CREATE PROCEDURE sproc_AddNote(
IN i_Note		text
,IN i_UserID	int
,IN i_TicketID	int
)
BEGIN 
	DECLARE NoteID int;
	IF EXISTS (SELECT * FROM Tickets WHERE TicketID = i_TicketID) THEN
		IF EXISTS ( SELECT * FROM Users WHERE UserID = i_UserID) THEN
			INSERT INTO Notes (Description, DateCreated) VALUES (i_Note, Now());	
			SET NoteID = LAST_INSERT_ID();
			INSERT INTO UserNotes (NoteID, UserID) VALUES (NoteID, i_UserID);
			INSERT INTO TicketNotes (NoteID, TicketID) VALUES (NoteID, i_TicketID);
		ELSE
			SELECT "No user by that ID exists in database" AS Error;
		END IF;
	ELSE
		SELECT "No ticket by that ID exists in database" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateNote
-- Results: Update a note in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateNoteByID;;

CREATE PROCEDURE sproc_UpdateNoteByID(
IN i_NoteID		 int
,IN i_Note		 text
)
BEGIN 
	IF EXISTS (SELECT * FROM Notes WHERE NoteID = i_NoteID) THEN 
		UPDATE Notes
			SET Note = i_Note
			WHERE NoteID = i_NoteID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteNoteByID
-- Result: Delete note and its associations to UserNotes and 
	TicketNotes by NoteID
-- Error: Returns a column named "ERROR" if no note is found.
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteNoteByID;;

CREATE PROCEDURE sproc_DeleteNoteByID(
IN i_NoteID int)
BEGIN 
	IF EXISTS (SELECT * FROM Notes WHERE NoteID = i_NoteID) THEN
		DELETE FROM Notes
			WHERE NoteID = i_NoteID;
		DELETE FROM UserNotes
			WHERE NoteID = i_NoteID;
		DELETE FROM TicketNotes
			WHERE NoteID = i_NoteID;
	ELSE
		SELECT "No Note with that ID exists" AS Error;
	END IF;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Organizations
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetOrganizations
-- Results: Gets all organizations from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetOrganizations;;

CREATE PROCEDURE sproc_GetOrganizations()
BEGIN 
	SELECT * FROM Organizations;
END;;
/*
-- ----------------------------------------------------------
-- GetOrganizationByID
-- Results: Gets by ID from organizations from database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetOrganizationByID;;

CREATE PROCEDURE sproc_GetOrganizationByID(
 IN i_OrganizationID int
)
BEGIN 
	SELECT * FROM Organizations 
	WHERE OrganizationID = i_OrganizationID;
END;;
/*
-- ----------------------------------------------------------
-- AddOrganization
-- Results: Add a organization to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddOrganization;;

CREATE PROCEDURE sproc_AddOrganization(
IN i_Name			 varchar (255)
,IN i_Description	 varchar(100)
)
BEGIN 
	INSERT INTO Organizations (Name, Description) VALUES (i_Name, 
		i_Description);
END;;
/*
-- ----------------------------------------------------------
-- UpdateOrganization
-- Results: Update a organization in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateOrganizationByID;;

CREATE PROCEDURE sproc_UpdateOrganizationByID(
IN OrganizationsID	 int
,IN i_Name			 varchar (255)
,IN i_Description	 varchar(100)
)
BEGIN 
	IF EXISTS (SELECT * FROM Organizations WHERE OrganizationID = i_OrganizationID) THEN 
		Update Organizations
			SET Name = i_Name, 
				Description = i_Description
			WHERE OrganizationID = i_OrganizationID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteOrganizationByID
-- Result: Delete entry from database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteOrganizationByID;;

CREATE PROCEDURE sproc_DeleteOrganizationByID(
IN i_OrganizationID int)
BEGIN 
	DELETE FROM Organizations
	WHERE OrganizationID = i_OrganizationID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Queues
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetQueues
-- Results: Gets all Queues with their foreign keys and 
--  information from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetQueues;;

CREATE PROCEDURE sproc_GetQueues()
BEGIN 
	SELECT QueueID, Name, TicketID, UserID
		FROM Queues;
END;;

/*
-- ----------------------------------------------------------
-- GetQueueByID
-- Results: Gets by ID from Queues with their foreign keys and 
--  information from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetQueueByID;;

CREATE PROCEDURE sproc_GetQueueByID(
 IN i_QueueID int
)
BEGIN 
	SELECT Name, TicketID, UserID
		FROM Queues
	WHERE QueueID = i_QueueID;
END;;
/*
-- ----------------------------------------------------------
-- GetQueuesByUserID
-- Results: Gets by ID from Queues with their foreign keys and 
--  information from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetQueuesByUserID;;

CREATE PROCEDURE sproc_GetQueuesByUserID(
 IN i_UserID int
)
BEGIN 
	SELECT Q.QueueID, Q.Name, UQ.UserID
		FROM UserQueues AS UQ
		LEFT JOIN Queues AS Q
			ON UQ.QueueID = Q.QueueID
		WHERE UQ.UserID = i_UserID;
END;;
/*
-- ----------------------------------------------------------
-- GetTicketsByQueueID
-- Results: Gets by ID from Queues with their foreign keys and 
--  information from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetTicketsByQueueID;;

CREATE PROCEDURE sproc_GetTicketsByQueueID(
 IN i_QueueID int
)
BEGIN 
	SELECT Q.Name, T.TicketID, T.Number, T.Subject,	T.Description,
		T.DateCreated, T.DateLastUpdated, T.DateDue, T.DateResolved, T.StatusID,
		S.Status, T.CategoryID, C.Name,TL.LocationID, L.Name AS Location 
		FROM QueueTickets AS QT
		LEFT JOIN Tickets AS T
			ON QT.TicketID = T.TicketID
		LEFT JOIN Queues AS Q
			ON QT.QueueID = Q.QueueID
		LEFT JOIN Statuses AS S 
			ON T.StatusID = S.StatusID
		LEFT JOIN Categories AS C 
			ON T.CategoryID = C.CategoryID
		LEFT JOIN TicketLocations AS TL 
			ON T.TicketID = TL.TicketID
		LEFT JOIN Locations AS L 
			ON TL.LocationID = L.LocationID
		WHERE QT.QueueID = i_QueueID;
END;;
/*
-- ----------------------------------------------------------
-- AddQueue
-- Results: Add a Queues with its foreign keys
-- to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddQueue;;

CREATE PROCEDURE sproc_AddQueue(
IN i_Name			 varchar(70)
,IN i_TicketID	 int
,IN i_UserID	 int
)
BEGIN 
	INSERT INTO Queues (Name, TicketID, UserID) 
		VALUES (i_Name, i_TicketID, i_UserID);
END;;
/*
-- ----------------------------------------------------------
-- AddQueueTicket
-- Results: Add a QueueTicket with its foreign keys
-- to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddQueueTicket;;

CREATE PROCEDURE sproc_AddQueueTicket(
i_TicketID	 int
,IN i_QueueID	 int
)
BEGIN 
	INSERT INTO QueueTickets (TicketID, QueueID) 
		VALUES (i_TicketID, i_QueueID);
END;;


/*
-- ----------------------------------------------------------
-- UpdateQueue
-- Results: Update a Queues with its foreign keys
-- in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateQueueByID;;
DROP PROCEDURE IF EXISTS sproc_UpdateQueue;;

CREATE PROCEDURE sproc_UpdateQueueByID(
IN i_QueueID	 int
,IN i_Name		 varchar(70)
,IN i_TicketID	 int
,IN i_UserID	 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Queues WHERE QueueID = i_QueueID) THEN 
		Update Queues
			SET Name = i_Name,
				TicketID = i_TicketID,
				UserID = i_UserID
			WHERE QueueID = i_QueueID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteQueueByID
-- Result: Delete entry from database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteQueueByID;;

CREATE PROCEDURE sproc_DeleteQueueByID(
IN i_QueueID int)
BEGIN 
	DELETE FROM Queues
	WHERE QueueID = i_QueueID;
END;;
/*
-- ----------------------------------------------------------
-- DeleteTicketFromQueue
-- Result: Delete a ticket from a queue with QueueID and TicketID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteTicketFromQueue;;

CREATE PROCEDURE sproc_DeleteTicketFromQueue(
IN i_QueueID int
,IN i_TicketID int)
BEGIN 
	DELETE FROM QueueTickets
	WHERE QueueID = i_QueueID AND TicketID = i_TicketID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Roles
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetRoles
-- Results: Gets all roles from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetRoles;;

CREATE PROCEDURE sproc_GetRoles()
BEGIN 
	SELECT * FROM Roles;
END;;
/*
-- ----------------------------------------------------------
-- GetRoleByID
-- Results: Gets a role by ID from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetRoleByID;;

CREATE PROCEDURE sproc_GetRoleByID(
 IN i_RoleID int
)
BEGIN 
	SELECT * FROM Roles 
	WHERE RoleID = i_RoleID;
END;;
/*
-- ----------------------------------------------------------
-- AddRole
-- Results: Adds a role to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddRole;;

CREATE PROCEDURE sproc_AddRole(
IN i_Title varchar(255)
,IN i_TicketsView tinyint
,IN i_TicketsComment tinyint
,IN i_TicketsResolve tinyint
,IN i_TicketsOpen tinyint
,IN i_TicketsEdit tinyint
,IN i_AssetsView tinyint
,IN i_AssetsAdd tinyint
,IN i_AssetsEdit tinyint
,IN i_AssetsArchive tinyint
,IN i_UsersView tinyint
,IN i_UsersAdd tinyint
,IN i_UsersEdit tinyint
,IN i_UsersDisable tinyint
,IN i_RolesView tinyint
,IN i_DeleteAsset tinyint 
)
BEGIN 
	INSERT INTO Roles (Title, TicketsView, TicketsComment, TicketsResolve, TicketsOpen,
	TicketsEdit, AssetsView, AssetsAdd, AssetsEdit, AssetsArchive, UsersView, UsersAdd, UsersEdit, UsersDisable, RolesView,DeleteAsset)
	VALUES (i_Title, i_TicketsView, i_TicketsComment, i_TicketsResolve, i_TicketsOpen, i_TicketsEdit,
	i_AssetsView, i_AssetsAdd, i_AssetsEdit, i_AssetsArchive, i_UsersView, i_UsersAdd, i_UsersEdit, i_UsersDisable, i_RolesView,i_DeleteAsset);
END;;
/*
-- ----------------------------------------------------------
-- UpdateRole
-- Results: Updates a role in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateRoleByID;;

CREATE PROCEDURE sproc_UpdateRoleByID(
IN i_RoleID	int
,IN i_Title varchar(255)
,IN i_TicketsView tinyint
,IN i_TicketsComment tinyint
,IN i_TicketsResolve tinyint
,IN i_TicketsOpen tinyint
,IN i_TicketsEdit tinyint
,IN i_AssetsView tinyint
,IN i_AssetsAdd tinyint
,IN i_AssetsEdit tinyint
,IN i_AssetsArchive tinyint
,IN i_UsersView tinyint
,IN i_UsersAdd tinyint
,IN i_UsersEdit tinyint
,IN i_UsersDisable tinyint
,IN i_RolesView tinyint
,IN i_DeleteAsset tinyint
)
BEGIN 
	IF EXISTS (SELECT * FROM Roles WHERE RoleID = i_RoleID) THEN 
		Update Roles
			SET Title = i_Title,
			TicketsView = i_TicketsView,
			TicketsComment = i_TicketsComment,
			TicketsResolve = i_TicketsResolve,
			TicketsOpen = i_TicketsOpen,
			TicketsEdit = i_TicketsEdit,
			AssetsView = i_AssetsView,
			AssetsAdd = i_AssetsAdd,
			AssetsEdit = i_AssetsEdit,
			AssetsArchive = i_AssetsArchive,
			UsersView = i_UsersView,
			UsersAdd = i_UsersAdd,
			UsersEdit = i_UsersEdit,
			UsersDisable = i_UsersDisable,
			RolesView = i_RolesView,
			DeleteAsset = i_DeleteAsset
			WHERE RoleID = i_RoleID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteRoleByID
-- Result: Delete entry from database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteRoleByID;;

CREATE PROCEDURE sproc_DeleteRoleByID(
IN i_RoleID int)
BEGIN 
	DELETE FROM Roles
	WHERE RoleID = i_RoleID;
END;;

/*
-- --------------------------------------------------------------------------------------------------------------------
-- States
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetStates
-- Results: Gets all states from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetStates;;

CREATE PROCEDURE sproc_GetStates()
BEGIN 
	SELECT * FROM States;
END;;
/*
-- ----------------------------------------------------------
-- GetStateByID
-- Results: Returns the state by ID from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetStateByID;;

CREATE PROCEDURE sproc_GetStateByID(
 IN i_StateID int
)
BEGIN 
	SELECT * FROM States 
	WHERE StateID = i_StateID;
END;;
/*
-- ----------------------------------------------------------
-- AddState
-- Results: Returna states from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddStates;;

CREATE PROCEDURE sproc_AddStates(
IN i_Name varchar(255)
)
BEGIN 
	IF NOT EXISTS(SELECT * FROM States WHERE Name = i_Name) THEN
		INSERT INTO States (Name) VALUES (i_Name);
	ELSE 
		SELECT 'State already exists' AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- UpdateState
-- Results: Update a states from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateStateByID;;

CREATE PROCEDURE sproc_UpdateStateByID(
IN i_StateID	 int
,IN i_DateArchived datetime
,IN i_Name		 varchar(255)
)
BEGIN 
	IF EXISTS (SELECT * FROM States WHERE StateID = i_StateID) THEN 
		Update States 
			SET Name = i_Name,
			DateArchived = i_DateArchived
			WHERE StateID = i_StateID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ArchiveStateByID
-- Result: Archive State in database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ArchiveStateByID;;

CREATE PROCEDURE sproc_ArchiveStateByID(
IN i_StateID int)
BEGIN 
	UPDATE States
	SET DateArchived = NOW()
	WHERE StateID = i_StateID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Statuses
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetStatuses
-- Results: Gets all statuses from the Database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetStatuses;;

CREATE PROCEDURE sproc_GetStatuses()
BEGIN 
	SELECT * FROM Statuses;
END;;
/*
-- ----------------------------------------------------------
-- GetStatusByID
-- Results: Gets a status by ID from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetStatusByID;;

CREATE PROCEDURE sproc_GetStatusByID(
 IN i_StatusID int
)
BEGIN 
	SELECT * FROM Statuses 
	WHERE StatusID = i_StatusID;
END;;
/*
-- ----------------------------------------------------------
-- AddStatus
-- Results: Add a statuse to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddStatus;;

CREATE PROCEDURE sproc_AddStatus(
IN i_Status varchar(255)
)
BEGIN 
	INSERT INTO Statuses (Status) VALUES (i_Status);
END;;
/*
-- ----------------------------------------------------------
-- UpdateStatus
-- Results: Update a statuse in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateStatusByID;;

CREATE PROCEDURE sproc_UpdateStatusByID(
IN i_StatusID	 int
,IN i_Status	 varchar(255)
)
BEGIN 
	IF EXISTS (SELECT * FROM Statuses WHERE StatusID = i_StatusID) THEN 
		UPDATE Statuses 
			SET Status = i_Status
			WHERE StatusID = i_StatusID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteStatusByID
-- Result: Delete entry from database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteStatusByID;;

CREATE PROCEDURE sproc_DeleteStatusByID(
IN i_StatusID int)
BEGIN 
	DELETE FROM Statuses
	WHERE StatusID = i_StatusID;
END;;
/*
-- --------------------------------------------------------------------------------------------------------------------
-- Tickets
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetTickets
-- Results: Gets all tickets and its foreign keys from the
-- database.
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetTickets;;

CREATE PROCEDURE sproc_GetTickets()
BEGIN 
	SELECT T.TicketID, T.Number, T.Subject,	T.Description,
		T.DateCreated, T.DateLastUpdated, T.DateDue, T.DateResolved, T.StatusID,
		S.Status, T.CategoryID, C.Name,TL.LocationID, L.Name AS Location 
	FROM Tickets AS T 
	LEFT JOIN Statuses AS S 
		ON T.StatusID = S.StatusID
	LEFT JOIN Categories AS C 
		ON T.CategoryID = C.CategoryID
	LEFT JOIN TicketLocations AS TL 
		ON T.TicketID = TL.TicketID
	LEFT JOIN Locations AS L 
		ON TL.LocationID = L.LocationID;
END;;
/*
-- ----------------------------------------------------------
-- GetTicketByID
-- Results: Gets a ticket by ID from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetTicketByID;;

CREATE PROCEDURE sproc_GetTicketByID(
 IN i_TicketID int
)
BEGIN 
	SELECT T.TicketID, T.Number, T.Subject, T.Description,
	T.DateCreated, T.DateLastUpdated, T.DateDue, T.DateResolved, T.StatusID,
		S.Status, T.CategoryID, C.Name,TL.LocationID, L.Name AS Location 
	FROM Tickets AS T 
	LEFT JOIN Statuses AS S 
		ON T.StatusID = S.StatusID
	LEFT JOIN Categories AS C 
		ON T.CategoryID = C.CategoryID
	LEFT JOIN TicketLocations AS TL 
		ON T.TicketID = TL.TicketID
	LEFT JOIN Locations AS L 
		ON TL.LocationID = L.LocationID
	WHERE T.TicketID = i_TicketID;
END;;

/*
-- ----------------------------------------------------------
-- AddAssetToTicket
--  Takes in an AssetID and a ticket ID
--	returns a single column named error if ticket or asset not found
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddAssetToTicket;;

CREATE PROCEDURE sproc_AddAssetToTicket(
IN i_TicketID 	int
,IN i_AssetID	int)
BEGIN
	IF EXISTS (SELECT *  FROM Tickets WHERE TicketID = i_TicketID) THEN
		IF EXISTS (SELECT *  FROM Assets WHERE AssetID = i_AssetID) THEN
			INSERT INTO TicketAssets (TicketID, AssetID) VALUES (i_TicketID, i_AssetID);
		ELSE
				SELECT "No Asset with that ID exists" AS Error;
		END IF;
	ELSE
		SELECT "No Ticket with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- RemoveAssetFromTicket
--  Takes in an AssetID and a ticket ID
--	returns a single column named error if ticket or asset not found
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_RemoveAssetFromTicket;;

CREATE PROCEDURE sproc_RemoveAssetFromTicket(
IN i_TicketID 	int
,IN i_AssetID	int)
BEGIN
	IF EXISTS (SELECT *  FROM Tickets WHERE TicketID = i_TicketID) THEN
		IF EXISTS (SELECT *  FROM Assets WHERE AssetID = i_AssetID) THEN
			DELETE FROM TicketAssets WHERE AssetID = i_AssetID AND TicketID = i_TicketID;
		ELSE
				SELECT "No Asset with that ID exists" AS Error;
		END IF;
	ELSE
		SELECT "No Ticket with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- AddTicket
-- Results: Add a ticket to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddTicket;;

CREATE PROCEDURE sproc_AddTicket(
IN i_Number			 int
,IN i_Subject		 varchar(150)
,IN i_Description	 text
,IN i_DateDue		 datetime
,IN i_CategoryID	 int
,IN i_UserID		 int
,IN i_RoleID		 int
)
BEGIN 
	INSERT INTO Tickets (Number, Subject, Description,
		DateLastUpdated, DateDue, 
		StatusID, CategoryID, DateCreated)
	VALUES (i_Number, i_Subject, i_Description,
		Now(), i_DateDue, 
		1, i_CategoryID, Now());

	INSERT INTO UserTicketsRoles (TicketID, UserID, RoleID) VALUES (LAST_INSERT_ID(), i_UserID, i_RoleID);

	SELECT LAST_INSERT_ID() AS NewInsertedID;
END;;
/*
-- ----------------------------------------------------------
-- UpdateTicket
-- Results: Update a ticket in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateTicketByID;;

CREATE PROCEDURE sproc_UpdateTicketByID(
IN i_TicketID			 int
,IN i_Number			 int
,IN i_Subject		 varchar(150)
,IN i_Description	 text
,IN i_DateCreated		 datetime
,IN i_DateLastUpdated	 datetime
,IN i_DateResolved		 datetime
,IN i_DateDue			 datetime
,IN i_StatusID			 int
,IN i_CategoryID		 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Tickets WHERE TicketID = i_TicketID) THEN 
		UPDATE Tickets 
			SET Number = i_Number, 
				Subject = i_Subject, 
				Description = i_Description,
				DateCreated = i_DateCreated, 
				DateLastUpdated = i_DateLastUpdated,
				DateResolved = i_DateResolved, 
				DateDue = i_DateDue, 
				StatusID = i_StatusID, 
				CategoryID = i_CategoryID
			WHERE TicketID = i_TicketID;
	
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- ResolveTicketByID
-- Result: DOES NOT actually delete ticket from database
--		Updates the DateResolved to current date by ID.
-- ---------------------------------------------------------- 
*/

DROP PROCEDURE IF EXISTS sproc_ResolveTicketByID;;

CREATE PROCEDURE sproc_ResolveTicketByID(
IN i_TicketID int)
BEGIN 
	IF EXISTS (SELECT * FROM Tickets WHERE TicketID = i_TicketID) THEN
		UPDATE Tickets
			SET DateResolved = Now()
			WHERE TicketID = i_TicketID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- CloseTicketByTicketID
-- This PROCEDURE takes in a ticketID and updates the DateResolved
-- 		to be the current date
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_CloseTicketByTicketID;;

CREATE PROCEDURE sproc_UpdateDateResolvedByTicketID(
IN i_TicketID int)
BEGIN
	IF EXISTS (SELECT *  FROM Tickets WHERE TicketID = i_TicketID) THEN 
		UPDATE Tickets
			SET DateResolved = Now()
			WHERE TicketID = i_TicketID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- UpdateDateDueByTicketID
-- This PROCEDURE takes in a ticketID and a Date
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateDateDueByTicketID;;

CREATE PROCEDURE sproc_UpdateDateDueByTicketID(
IN i_TicketID 	int
,IN i_DateDue	datetime)
BEGIN
	IF EXISTS (SELECT *  FROM Tickets WHERE TicketID = i_TicketID) THEN 
		UPDATE Tickets
			SET DateDue = i_DateDue 
			WHERE TicketID = i_TicketID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- UpdateDateLastUpdatedByTicketID 
-- This PROCEDURE takes in a ticketID and sets the DateLastUpdated
-- 		to the current date
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateDateLastUpdatedByTicketID;;

CREATE PROCEDURE sproc_UpdateDateLastUpdatedByTicketID(
IN i_TicketID int)
BEGIN
	IF EXISTS (SELECT *  FROM Tickets WHERE TicketID = i_TicketID) THEN 
		UPDATE Tickets
			SET DateLastUpdated = Now() 
			WHERE TicketID = i_TicketID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- UpdateStatusIDByTicketID 
--  This PROCEDURE takes in a TicketID and a StatusID
-- 		it then updates the StatusID for the ticket
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateStatusIDByTicketID;;

CREATE PROCEDURE sproc_UpdateStatusIDByTicketID(
IN i_TicketID 	int
,IN i_StatusID	int)
BEGIN
	IF EXISTS (SELECT *  FROM Tickets WHERE TicketID = i_TicketID) THEN 
		UPDATE Tickets
			SET StatusID = i_StatusID 
			WHERE TicketID = i_TicketID;
	ELSE
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;


/*
-- --------------------------------------------------------------------------------------------------------------------
-- Users
-- --------------------------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------
-- GetUsers
-- Results: Gets all users and their foreign keys from the 
-- database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUsers;;

CREATE PROCEDURE sproc_GetUsers()
BEGIN 
	SELECT U.UserID, U.Email, U.FirstName, U.LastName, U.RoleID,
			U.DateDisabled, R.Title
		FROM Users AS U
		LEFT JOIN Roles AS R 
			ON U.RoleID = R.RoleID;
END;;
/*
-- ----------------------------------------------------------
-- GetUserByID
-- Results: Gets a user by ID from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUserByID;;

CREATE PROCEDURE sproc_GetUserByID(
 IN i_UserID int
)
BEGIN 
	SELECT U.UserID, U.Email, U.FirstName, U.LastName, U.RoleID,
			U.DateDisabled, R.Title
		FROM Users AS U
		LEFT JOIN Roles AS R 
			ON U.RoleID = R.RoleID
		WHERE UserID = i_UserID;
END;;
/*
-- ----------------------------------------------------------
-- GetUserByNoteID
-- Results: Gets a user by NoteID from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUserByNoteID;;

CREATE PROCEDURE sproc_GetUserByNoteID(
 IN i_NoteID int
)
BEGIN 
	SELECT U.UserID, U.Email, U.FirstName, U.LastName, U.RoleID, U.DateDisabled,
			R.Title
		FROM Users AS U
		LEFT JOIN Roles AS R 
			ON U.RoleID = R.RoleID
		LEFT JOIN UserNotes AS UN
			ON U.UserID = UN.UserID
		LEFT JOIN Notes AS N
			ON UN.NoteID = N.NoteID
		WHERE N.NoteID = i_NoteID;
END;;
/*
-- ----------------------------------------------------------
-- GetUserByEmail
-- Results: Gets a user by email from the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUserByEmail;;

CREATE PROCEDURE sproc_GetUserByEmail(
 IN i_Email varchar(60), IN i_Password varchar(255)
)
BEGIN 
	SELECT U.UserID, U.Email, U.Password, U.Salt, U.FirstName, U.LastName, U.RoleID,
			U.DateDisabled, R.Title
		FROM Users AS U
		LEFT JOIN Roles AS R 
			ON U.RoleID = R.RoleID
		WHERE Email = i_Email;
END;;
/*
-- ----------------------------------------------------------
-- AddUser
-- Results: Add a user to the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_AddUser;;

CREATE PROCEDURE sproc_AddUser(
IN i_Email			 varchar(60)
,IN i_Password	 varchar(255)
,IN i_Salt		 varchar(255)
,IN i_FirstName	 varchar(25)
,IN i_LastName	 varchar(25)
,IN i_RoleID	 int
)
BEGIN 
	INSERT INTO Users (Email, Password, Salt, FirstName, LastName, RoleID)
		Values (i_Email, i_Password, i_Salt, i_FirstName, i_LastName, i_RoleID);
END;;
/*
-- ----------------------------------------------------------
-- UpdateUser
-- Results: Update a user in the database
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_UpdateUserByID;;

CREATE PROCEDURE sproc_UpdateUserByID(
IN i_UserID		 int
,IN i_Email		 varchar(60)
,IN i_FirstName	 varchar(25)
,IN i_LastName	 varchar(25)
,IN i_RoleID	 int
)
BEGIN 
	IF EXISTS (SELECT * FROM Users WHERE UserID = i_UserID) THEN 
		UPDATE Users 
			SET Email = i_Email, 
				FirstName = i_FirstName, 
				LastName = i_LastName, 
				RoleID = i_RoleID
			WHERE UserID = i_UserID;
	ELSE 
		SELECT "No entry with that ID exists" AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- DeleteUserByID
-- Result: Delete entry from database by ID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteUserByID;;

CREATE PROCEDURE sproc_DeleteUserByID(
IN i_UserID int)
BEGIN 
	DELETE FROM Users
	WHERE UserID = i_UserID;
END;;
/*
-- ----------------------------------------------------------
-- AddUserTicketsRoles
-- Result: Add a UserTicketsRoles Association
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_AddUserTicketsRoles;;

CREATE PROCEDURE sproc_AddUserTicketsRoles(
IN i_UserID int
,IN i_TicketID int
,IN i_RoleID int)
BEGIN 
	IF NOT EXISTS(SELECT * FROM UserTicketsRoles WHERE UserID = i_UserID AND TicketID = i_TicketID AND RoleID = i_RoleID) THEN
		INSERT INTO UserTicketsRoles (UserID, TicketID, RoleID) VALUES (i_UserID, i_Ticket, i_Role);
	ELSE
		SELECT 'That Association Already Exists' AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- DeleteUserTicketsRoles
-- Result: Delete a UserTicketsRoles Association
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteUserTicketsRoles;;

CREATE PROCEDURE sproc_DeleteUserTicketsRoles(
IN i_UserID int
,IN i_TicketID int
,IN i_RoleID int)
BEGIN 
	IF EXISTS(SELECT * FROM UserTicketsRoles WHERE UserID = i_UserID AND TicketID = i_TicketID AND RoleID = i_RoleID) THEN
		DELETE FROM UserTicketsRoles WHERE UserID = i_UserID AND TicketID = i_TicketID AND RoleID = i_RoleID;
	ELSE
		SELECT 'That Association Does Not Exists' AS Error;
	END IF;
END;;

/*
-- ----------------------------------------------------------
-- GetUserTicketsRolesByTicketID
-- Results: Gets UserTicketsRoles Associations by TicketID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUserTicketsRolesByTicketID;;

CREATE PROCEDURE sproc_GetUserTicketsRolesByTicketID(
 IN i_TicketID int
)
BEGIN
	IF EXISTS(SELECT * FROM UserTicketsRoles WHERE TicketID = i_TicketID)THEN
		SELECT U.FirstName, U.LastName, U.Email, U.DateDisabled, UTR.UserID,
			UTR.TicketID, T.Number, T.Subject, T.Description,
			T.DateCreated, T.DateLastUpdated, T.DateDue, T.DateResolved, T.StatusID,
			T.CategoryID,TL.LocationID, UTR.RoleID, R.Title,
			R.Title, R.TicketsView, R.TicketsComment, R.TicketsResolve, 
			R.TicketsOpen, R.TicketsEdit, R.AssetsView, R.AssetsAdd, R.AssetsEdit, 
			R.AssetsArchive, R.UsersView, R.UsersAdd, R.UsersEdit, R.UsersDisable, 
			R.RolesView, R.DeleteAsset
		FROM UserTicketsRoles AS UTR
		LEFT JOIN Users AS U
			ON UTR.UserID = U.UserID
		LEFT JOIN Tickets AS T
			ON UTR.TicketID = T.TicketID
		LEFT JOIN Roles AS R
			ON UTR.RoleID = R.RoleID
		LEFT JOIN TicketLocations AS TL
			ON T.TicketID = TL.TicketID
		WHERE UTR.TicketID = i_TicketID;
	ELSE
		SELECT 'No association exists with that ticket.' AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- GetUserTicketsRolesByUserID
-- Results: Gets UserTicketsRoles Associations by UserID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUserTicketsRolesByUserID;;

CREATE PROCEDURE sproc_GetUserTicketsRolesByUserID(
 IN i_UserID int
)
BEGIN
	IF EXISTS(SELECT * FROM UserTicketsRoles WHERE TicketID = i_TicketID)THEN
		SELECT U.FirstName, U.LastName, U.Email, U.DateDisabled, UTR.UserID,
			UTR.TicketID, T.Number, T.Subject, T.Description,
			T.DateCreated, T.DateLastUpdated, T.DateDue, T.DateResolved, T.StatusID,
			T.CategoryID,TL.LocationID, UTR.RoleID, R.Title,
			R.Title, R.TicketsView, R.TicketsComment, R.TicketsResolve, 
			R.TicketsOpen, R.TicketsEdit, R.AssetsView, R.AssetsAdd, R.AssetsEdit, 
			R.AssetsArchive, R.UsersView, R.UsersAdd, R.UsersEdit, R.UsersDisable, 
			R.RolesView, R.DeleteAsset
		FROM UserTicketsRoles AS UTR
		LEFT JOIN Users AS U
			ON UTR.UserID = U.UserID
		LEFT JOIN Tickets AS T
			ON UTR.TicketID = T.TicketID
		LEFT JOIN Roles AS R
			ON UTR.RoleID = R.RoleID
		LEFT JOIN TicketLocations AS TL
			ON T.TicketID = TL.TicketID
		WHERE UTR.UserID = i_UserID;
	ELSE
		SELECT 'No association exists with that user.' AS Error;
	END IF;
END;;
/*
-- ----------------------------------------------------------
-- GetUserTicketsRolesByRoleID
-- Results: Gets UserTicketsRoles Associations by RoleID
-- ----------------------------------------------------------
*/
DROP PROCEDURE IF EXISTS sproc_GetUserTicketsRolesByRoleID;;

CREATE PROCEDURE sproc_GetUserTicketsRolesByRoleID(
 IN i_RoleID int
)
BEGIN
	IF EXISTS(SELECT * FROM UserTicketsRoles WHERE RoleID = i_RoleID)THEN
		SELECT U.FirstName, U.LastName, U.Email, U.DateDisabled, UTR.UserID,
			UTR.TicketID, T.Number, T.Subject, T.Description,
			T.DateCreated, T.DateLastUpdated, T.DateDue, T.DateResolved, T.StatusID,
			T.CategoryID,TL.LocationID, UTR.RoleID, R.Title,
			R.Title, R.TicketsView, R.TicketsComment, R.TicketsResolve, 
			R.TicketsOpen, R.TicketsEdit, R.AssetsView, R.AssetsAdd, R.AssetsEdit, 
			R.AssetsArchive, R.UsersView, R.UsersAdd, R.UsersEdit, R.UsersDisable, 
			R.RolesView, R.DeleteAsset
		FROM UserTicketsRoles AS UTR
		LEFT JOIN Users AS U
			ON UTR.UserID = U.UserID
		LEFT JOIN Tickets AS T
			ON UTR.TicketID = T.TicketID
		LEFT JOIN Roles AS R
			ON UTR.RoleID = R.RoleID
		LEFT JOIN TicketLocations AS TL
			ON T.TicketID = TL.TicketID
		WHERE UTR.RoleID = i_RoleID;
	ELSE
		SELECT 'No association exists with that role.' AS Error;
	END IF;
END;;


/*
-- ----------------------------------------------------------
-- DeleteQueueFromUser
-- Result: Delete a User from a queue with QueueID and UserID
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DeleteQueueFromUser;;

CREATE PROCEDURE sproc_DeleteQueueFromUser(
IN i_QueueID int
,IN i_UserID int)
BEGIN 
	DELETE FROM QueueUsers
	WHERE QueueID = i_QueueID AND UserID = i_UserID;
END;;
-- ----------------------------------------------------------
-- ----------------------------------------------------------
/*
-- ----------------------------------------------------------
-- ResolveTicketByID
-- Result: Remove Ticket from list of active tickets
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ResolveTicketByID;;
CREATE PROCEDURE sproc_ResolveTicketByID(
IN i_TicketID int)
BEGIN
UPDATE Tickets
SET DateResolved = NOW(),
	StatusID = 4
WHERE TicketID = i_TicketID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreTicketByID
-- Result: Restore Ticket to list of active Tickets
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreTicketByID;;
CREATE PROCEDURE sproc_RestoreTicketByID(
IN i_TicketID int)
BEGIN
UPDATE Tickets
SET DateResolved = '9999-12-31 23:59:59'
WHERE TicketID = i_TicketID;
END;;

-- ----------------------------------------------------------
-- ----------------------------------------------------------
/*
-- ----------------------------------------------------------
-- GetLastTicket
-- Result: Gets last ticket created
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_GetLastTicket;;
CREATE PROCEDURE sproc_GetLastTicket()
BEGIN
SELECT * FROM Tickets ORDER BY TicketID DESC LIMIT 1;
END;;
-- ----------------------------------------------------------
-- ----------------------------------------------------------
/*
-- ----------------------------------------------------------
-- ArchiveAssetByID
-- Result: Remove Ticket from list of active tickets
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_ArchiveAssetByID;;
CREATE PROCEDURE sproc_ArchiveAssetByID(
IN i_AssetID int)
BEGIN
UPDATE Assets
SET DateArchived = NOW()
WHERE AssetID = i_AssetID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreAssetByID
-- Result: Restore Asset to list of active Assets
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreAssetByID;;
CREATE PROCEDURE sproc_RestoreAssetByID(
IN i_AssetID int)
BEGIN
UPDATE Assets
SET DateArchived = '9999-12-31 23:59:59'
WHERE AssetID = i_AssetID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreBrandByID
-- Result: Restore Brand to list of active Brands
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreBrandByID;;
CREATE PROCEDURE sproc_RestoreBrandByID(
IN i_BrandID int)
BEGIN
UPDATE Brands
SET DateArchived = '9999-12-31 23:59:59'
WHERE BrandID = i_BrandID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreDeviceByID
-- Result: Restore Device to list of active Devices
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreDeviceByID;;
CREATE PROCEDURE sproc_RestoreDeviceByID(
IN i_DeviceID int)
BEGIN
UPDATE Devices
SET DateArchived = '9999-12-31 23:59:59'
WHERE DeviceID = i_DeviceID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreModelByID
-- Result: Restore Model to list of active Models
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreModelByID;;
CREATE PROCEDURE sproc_RestoreModelByID(
IN i_ModelID int)
BEGIN
UPDATE Models
SET DateArchived = '9999-12-31 23:59:59'
WHERE ModelID = i_ModelID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreStateByID
-- Result: Restore State to list of active States
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreStateByID;;
CREATE PROCEDURE sproc_RestoreStateByID(
IN i_StateID int)
BEGIN
UPDATE States
SET DateArchived = '9999-12-31 23:59:59'
WHERE StateID = i_StateID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreLocationByID
-- Result: Restore Location to list of active Locations
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreLocationByID;;
CREATE PROCEDURE sproc_RestoreLocationByID(
IN i_LocationID int)
BEGIN
UPDATE Locations
SET DateArchived = '9999-12-31 23:59:59'
WHERE LocationID = i_LocationID;
END;;

/*
-- ----------------------------------------------------------
-- DisableUserByID
-- Result: Remove User from list of active Users
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_DisableUserByID;;
CREATE PROCEDURE sproc_DisableUserByID(
IN i_UserID int)
BEGIN
UPDATE Users
SET DateDisabled = NOW()
WHERE UserID = i_UserID;
END;;

/*
-- ----------------------------------------------------------
-- RestoreUserByID
-- Result: Restore User to list of active Users
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_RestoreUserByID;;
CREATE PROCEDURE sproc_RestoreUserByID(
IN i_UserID int)
BEGIN
UPDATE Users
SET DateDisabled = '9999-12-31 23:59:59'
WHERE UserID = i_UserID;
END;;

/*
-- ----------------------------------------------------------
-- GetLastAddedModel
-- Result: Gets last AddedModel created
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_GetLastAddedModel;;

CREATE PROCEDURE sproc_GetLastAddedModel()
BEGIN
SELECT * FROM Models ORDER BY ModelID DESC LIMIT 1;
END;;

/*
-- ----------------------------------------------------------
-- UpdatePasswordByUserID
-- Result: Change user password
-- ---------------------------------------------------------- 
*/
DROP PROCEDURE IF EXISTS sproc_UpdatePasswordByUserID;;

CREATE PROCEDURE sproc_UpdatePasswordByUserID(
IN i_UserID int,
IN i_Password varchar(255),
IN i_Salt varchar(255)
)
BEGIN
UPDATE Users AS u
SET u.Password = i_Password, u.Salt = i_Salt
WHERE u.UserID = i_UserID;
END;;
-- ----------------------------------------------------------
-- ----------------------------------------------------------

Delimiter ;
-- --------------------------------------------------------------------------------------------------------------------
-- End of STORED PROCEDURES
-- --------------------------------------------------------------------------------------------------------------------

-- --------------------------------------------------------------------------------------------------------------------
-- Grant AMSReadUser Priviledges to use read procudures
-- --------------------------------------------------------------------------------------------------------------------
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetAssetByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetAssets TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetAssetsByTicketID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetAssetsLike TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetBrandByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetBrandByName TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetBrands TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetCategories TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetCategoryByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetDeviceByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetDeviceByName TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetDevices TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetDevicesByBrandID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLastAddedModel TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLastTicket TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLoanedAssetAndUserByAssetID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLoanedAssetAndUserByUserID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLoanedAssets TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLocationByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetLocations TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetModelByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetModels TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetModelsByDeviceID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetNotes TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetNotesByTicketID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetOrganizationByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetOrganizations TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetQueueByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetQueues TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetQueuesByUserID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetRoleByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetRoles TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetStateByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetStates TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetStatusByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetStatuses TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetTicketByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetTickets TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetTicketsByQueueID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUserByEmail TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUserByID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUserByNoteID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUsers TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUserTicketsRolesByTicketID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUserTicketsRolesByUserID TO 'AMSReadUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_GetUserTicketsRolesByRoleID TO 'AMSReadUser'@'localhost';

---- --------------------------------------------------------------------------------------------------------------------
---- Grant AMSEditUser Priviledges to use Edit procudures
---- --------------------------------------------------------------------------------------------------------------------
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddAsset TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddAssetToTicket TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddBrand TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddCategory TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddDevice TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddEmail TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddEmailToTicket TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddLoanedAsset TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddLocation TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddModel TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddNote TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddOrganization TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddQueue TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddQueueTicket TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddRecipientToEmail TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddRole TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddStates TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddStatus TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddTicket TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddUser TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ArchiveAssetByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ArchiveBrandByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteCategoryByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ArchiveDeviceByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteLoanedAssetByAssetID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ArchiveLocationByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ArchiveModelByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteNoteByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteOrganizationByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteQueueByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteQueueFromUser TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteRoleByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ArchiveStateByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteStatusByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteTicketFromQueue TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteUserByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DisableUserByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RemoveAssetFromTicket TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_ResolveTicketByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreTicketByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreAssetByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreUserByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreBrandByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreDeviceByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreModelByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreStateByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_RestoreLocationByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateAssetByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateBrandByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateCategoryByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateDateDueByTicketID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateDateExpectedReturnByAssetID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateDateLastUpdatedByTicketID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateDateResolvedByTicketID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateDateWarrantyExpiresByAssetID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateDeviceByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateIsLoanableByAssetID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateLocationByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateLocationIDByAssetID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateModelByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateNoteByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateOrganizationByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdatePLocationIDByLocationID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateQueueByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateRoleByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateStateByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateStateIDByAssetID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateStateIDByLocationID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateStatusByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateStatusIDByTicketID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateTicketByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdateUserByID TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_DeleteUserTicketsRoles TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_AddUserTicketsRoles TO 'AMSEditUser'@'localhost';
--GRANT EXECUTE ON PROCEDURE inventorymanagement.sproc_UpdatePasswordByUserID TO 'AMSEditUser'@'localhost';

-- --------------------------------------------------------------------------------------------------------------------
-- Preloaded Data needed for the base system
-- --------------------------------------------------------------------------------------------------------------------
/* 
-- ---------------------------------------------------------- 
-- Statuses Dummy Data Insert  
-- ----------------------------------------------------------
*/
INSERT INTO Statuses (StatusID,Status) 
VALUES (1,"Open")
      ,(2,"Urgent")
      ,(3,"On Hold")
      ,(4, "Resolved");

/* 
-- ---------------------------------------------------------- 
-- Roles Dummy Data Insert  
-- ----------------------------------------------------------
*/
INSERT INTO Roles (RoleID,Title,TicketsView,TicketsComment,TicketsResolve,TicketsOpen,
                   TicketsEdit,AssetsView,AssetsAdd,AssetsEdit,AssetsArchive,UsersView,
                   UsersAdd,UsersEdit,UsersDisable,RolesView,DeleteAsset)
VALUES (1,"Administrator",1,1,1,1,1,1,1,1,1,1,1,1,1,1,1)
      ,(2,"Technician",1,1,1,1,1,1,1,1,1,1,0,0,0,0,0)
      ,(3,"Base",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);

/* 
-- ---------------------------------------------------------- 
-- Categories Dummy Data Insert  
-- ----------------------------------------------------------
*/
INSERT INTO Categories (CategoryID,Name)
VALUES (1,"Hardware")
      ,(2,"Software")
      ,(3,"Network");

/* 
-- ---------------------------------------------------------- 
-- Users Dummy Data Insert  
-- ----------------------------------------------------------
*/
INSERT INTO Users (UserID,Email,Password,Salt,FirstName,LastName,RoleID)
VALUES (1,"cobpcs@gmail.com","pG7A1V0Kw+WDcbGv5jZfiSngGVECa29Hj7mKJBkulf/je",
        "0JAy2rQFNXHrlMxTkZPxUayLvyiQU1PglYBAwQ9K3gEsemVYJ9","admin","admin",1);


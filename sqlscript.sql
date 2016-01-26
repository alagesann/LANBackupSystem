CREATE DATABASE LANBackupSystem;

CREATE TABLE USERS
(
USERID VARCHAR(50) NOT NULL PRIMARY KEY,
[PASSWORD] VARCHAR(50) NOT NULL,
DOMAIN VARCHAR(50)
)

CREATE TABLE CLIENT
(
CLIENTID VARCHAR(100) NOT NULL PRIMARY KEY,
ISAVAILABLE BIT NOT NULL DEFAULT 1,
)

CREATE TABLE BACKUPJOBS
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
[STATUS] INT NOT NULL,
ISENABLED BIT NOT NULL DEFAULT 1,
SOURCE_PATH VARCHAR(500) NOT NULL ,
DESTINATION_PATH VARCHAR(500) NOT NULL,
SOURCE_USERID VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES USERS(USERID),
DESTINATION_USERID VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES USERS(USERID),
CLIENT_ID VARCHAR(100) FOREIGN KEY REFERENCES CLIENT(CLIENTID)
)

CREATE TABLE BACKUPLOG
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
BACKUPJOBID UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES BACKUPJOBS(ID),
[MESSAGE] VARCHAR(1000) 
)
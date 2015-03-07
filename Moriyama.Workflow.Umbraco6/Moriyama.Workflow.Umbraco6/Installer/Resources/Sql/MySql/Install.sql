﻿CREATE TABLE workflowconfiguration (
	id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    name nvarchar(255) NOT NULL,
    typename nvarchar(255) NOT NULL,
    isconfigurationactive BIT NOT NULL DEFAULT 0,
    locked BIT NOT NULL DEFAULT 0);

CREATE TABLE point(
	ownerId nvarchar(255) NOT NULL,
    x int NOT NULL,
    y int NOT NULL);

CREATE TABLE workflowinstance(
	id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    name nvarchar(255) NOT NULL,
    typename nvarchar(255) NOT NULL,
    instantiationtime datetime,
    running BIT NOT NULL DEFAULT 0,
    currenttask nvarchar(255));

CREATE TABLE workflowcriteria(
	id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    name nvarchar(255) NOT NULL);
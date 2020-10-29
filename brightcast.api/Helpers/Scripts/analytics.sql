/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Username]
      ,[PasswordHash]
      ,[PasswordSalt]
      ,[CreatedBy]
      ,[CreatedAt]
      ,[UpdatedBy]
      ,[UpdatedAt]
      ,[Deleted]
      ,[BusinessName]
      ,[FullName]
  FROM [dbo].[Users]


  select count(*) from Users where CreatedAt > '2020-10-21 15:13:41.0217617'
  
  select count(*) from Users where CreatedAt > '2020-09-29 15:13:41.0217617'
  
  select count(*) from Users 


  select count(*) from Campaigns where CreatedAt > '2020-10-21 15:13:41.0217617'
  
  select count(*) from Campaigns where CreatedAt > '2020-09-29 15:13:41.0217617'
  
  select count(*) from Campaigns 


  select count(*) from Contacts where CreatedAt > '2020-10-21 15:13:41.0217617'
  
  select count(*) from Contacts where CreatedAt > '2020-09-29 15:13:41.0217617'
  
  select count(*) from Contacts 


  select count(*) from ReceiveMessages where CreatedAt > '2020-10-21 15:13:41.0217617'
  
  select count(*) from ReceiveMessages where CreatedAt > '2020-09-29 15:13:41.0217617'
  
  select count(*) from ReceiveMessages 


  select count(*) from ChatMessages where CreatedAt > '2020-10-21 15:13:41.0217617'
  
  select count(*) from ChatMessages where CreatedAt > '2020-09-29 15:13:41.0217617'
  
  select count(*) from ChatMessages 